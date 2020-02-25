using System;
using System.Collections;

using ClockBlockers.DataStructures;
using ClockBlockers.GameControllers;
using ClockBlockers.NewReplaySystem.ReplayRunner;
using ClockBlockers.NewReplaySystem.ReplaySpawner;
using ClockBlockers.NewReplaySystem.ReplayStorage;
using ClockBlockers.Targetting;
using ClockBlockers.Utility;
using ClockBlockers.Weapons;

using UnityEngine;


namespace ClockBlockers.Characters.Scripts
{
	// TODO: Add a simple "Aim towards the middle of a character" AI for weapons.
	// TODO: Add Dot Product aiming, as opposed to RayCast aiming, for better-feeling aiming


	public abstract partial class Character : MonoBehaviour, IInteractable
	{
		protected WaitForFixedUpdate waitForFixedFrame;

		internal Camera Cam { get; private set; }

		protected CharacterController characterController;

		internal IReplayStorage replayStorage;

		internal IReplaySpawner replaySpawner;

		
		// There's a fundamental difference in architecture; They can't mix.
		internal ActionReplayRunner replayRunner;


		private CharacterBody _body;

		private Renderer _bodyRenderer;

		private IRayProvider _rayProvider;

		private ITargeter _targeter;

		private float _diedTime;

		protected Vector3 startPos;
		protected Quaternion startRot;

		protected Quaternion camStartRot;


		#region Set in inspector

		private float MoveSpd
		{
			get => moveSpd;
		}

		private float MinCamAngle
		{
			get => minCamAngle;
		}

		private float MaxCamAngle
		{
			get => maxCamAngle;
		}

		private float Health
		{
			get => _health;
			set => _health = Mathf.Clamp(value, 0, maxHealth);
		}

		private float _health;

		private float Armor
		{
			get => armor;
		}

		private float Shielding
		{
			get => shielding;
		}

		private Gun Gun
		{
			get => gun;
		}

		[SerializeField]
		private Gun gun;

		[SerializeField]
		private float minCamAngle;

		[SerializeField]
		private float maxCamAngle;

		[SerializeField]
		[Space(10)]
		private float maxHealth;

		[SerializeField]
		private float armor;

		[SerializeField]
		private float shielding;

		[SerializeField]
		[Header("Character Variables")]
		private float moveSpd;

		#endregion


		public GameController gameController;

		protected virtual void Awake()
		{
			replayStorage = GetComponent<IReplayStorage>();
			if (replayStorage == null)
			{
				Logging.LogWarning("No Replay Storage!");
			}

			replaySpawner = GetComponent<IReplaySpawner>();
			if (replaySpawner == null)
			{
				Debug.LogWarning("No Replay Spawner!");
			}


			replayRunner = GetComponent<ActionReplayRunner>();
			if (replayRunner == null)
			{
				Debug.LogWarning("No Replay Runner!");
			}

			Cam = GetComponentInChildren<Camera>();
			characterController = GetComponent<CharacterController>();
			Gun.Holder = this;

			_body = GetComponentInChildren<CharacterBody>();
			_bodyRenderer = _body.GetComponent<Renderer>();

			_targeter = GetComponent<ITargeter>();
			_rayProvider = GetComponent<IRayProvider>();
		}

		protected virtual void Start()
		{
			if (gameController == null) Logging.LogIncorrectInstantiation("GameController", this);

			AssignDelegates();

			Transform camTransform = Cam.transform;

			Transform charTransform = transform;

			startPos = charTransform.position;
			startRot = charTransform.rotation;

			camStartRot = camTransform.rotation;
		}

		protected virtual void RotateCharacter(float yRotation)
		{
			var angle = new Vector3(0, yRotation, 0);
			transform.Rotate(angle);
		}

		protected virtual void RotateCamera(float rotation)
		{
			Vector3 currentAngle = Cam.transform.rotation.eulerAngles;

			float newX = currentAngle.x - rotation;

			// ReSharper disable once IdentifierTypo
			float preclampedX = newX > 180 ? newX - 360 : newX;
			float clampedX = Mathf.Clamp(preclampedX, MinCamAngle, MaxCamAngle);

			var newAngle = new Vector3(clampedX, 0, 0);

			Cam.transform.localRotation = Quaternion.Euler(newAngle);
		}

		protected virtual void MoveCharacterForward(float[] vector)
		{
			Transform transform1 = transform;
			Vector3 forward = transform1.forward;
			Vector3 right = transform1.right;
			forward.y = 0;
			right.y = 0;
			forward.Normalize();
			right.Normalize();

			Vector3 prelimMove = (forward * vector[1]) + (right * vector[0]);
			Vector3 move = prelimMove * MoveSpd;
			Vector3 roundedVector = move.Round(gameController.FloatingPointPrecision);

			MoveCharacter(roundedVector);
		}

		private void MoveCharacter(Vector3 vector)
		{
			characterController.Move(vector);
		}

		protected virtual void AttemptToShoot()
		{
			// TODO: Add Ammo checks etc..

			if (gun.CanShoot)
			{
				Gun.PullTrigger();
			}
		}

		private void AttemptDealDamage(DamagePacket damagePacket)
		{
			DealDamage(damagePacket);
		}

		private void DealDamage(DamagePacket damagePacket)
		{
			float finalDamage = damagePacket.damage - Armor;
			float remainingDamage = finalDamage;
			if (remainingDamage <= 0) return;

			if (Shielding > 0)
			{
				if (shielding >= remainingDamage)
				{
					shielding -= remainingDamage;
				}
				else
				{
					remainingDamage -= shielding;
					shielding = 0;
				}
			}

			Health -= remainingDamage;
			if (Health <= 0)
			{
				AttemptKill();
			}
		}

		private void AttemptKill()
		{
			Kill();
		}

		private void Kill()
		{
			_bodyRenderer.material = gameController.DeadMaterial;

			const float removalTime = 1.25f;

			// StopAllCoroutines();

			// Man, that felt satisfying to replace.
			// I never wanted to call StopAllCoroutines() on this class in the first place, because:
			// I knew that, if I wanted to do other non-action related Coroutines in the future, I'd have to find another way to only stop the action ones.
			
			// BUG: If you restart the round after a character has died, but before he despawns, some error occurs. One that sticks forever for some reason.
			replayRunner.Stop();

			Destroy(gameObject, removalTime);

			StartCoroutine(Co_FallThroughFloor(removalTime));
		}

		protected virtual void SpawnReplay()
		{
			replaySpawner.SpawnLatestReplay();
		}

		public void OnHit(DamagePacket damagePacket, Vector3 hitPosition)
		{
			AttemptDealDamage(damagePacket);
		}

		private Ray CreateRay()
		{
			return _rayProvider.CreateRay();
		}

		public Tuple<IInteractable, RaycastHit> GetTarget(float range)
		{
			return _targeter.GetInteractableFromRay(CreateRay(), range);
		}
	}


	public abstract partial class Character
	{
		private void AssignDelegates()
		{
			replayRunner.moveAction = Action_MoveCharacter;
			replayRunner.rotateCharacterAction = Action_RotateCharacter;
			replayRunner.rotateCameraAction = Action_RotateCamera;
			replayRunner.shootAction = Action_AttemptToShoot;
			replayRunner.spawnReplayAction = Action_SpawnReplay;
			replayRunner.completedAllActions = Action_CompletedAll;

			gameController.newActStarted += OnNewActStart;
			gameController.actEnded += OnActEnded;
		}


		private void Action_MoveCharacter(float[] value)
		{
			float[] move = {value[0], value[1]};
			MoveCharacterForward(move);
		}

		private void Action_RotateCharacter(float[] value)
		{
			float rotation = value[0];
			RotateCharacter(rotation);
		}

		private void Action_RotateCamera(float[] value)
		{
			float rotation = value[0];
			RotateCamera(rotation);
		}

		private void Action_AttemptToShoot(float[] value)
		{
			AttemptToShoot();
		}

		private void Action_SpawnReplay(float[] value)
		{
			SpawnReplay();
		}

		private void Action_CompletedAll()
		{
			const float removalTime = 5f;

			Destroy(gameObject, removalTime);
			_diedTime = Time.fixedDeltaTime;

			StartCoroutine(Co_FallThroughFloor(removalTime));
		}

		private IEnumerator Co_FallThroughFloor(float removalTime)
		{
			// Get the height of the body.
			// ReSharper disable once Unity.PerformanceCriticalCodeInvocation
			float height = _body.GetComponent<MeshFilter>().mesh.bounds.extents.y;

			// Multiply it by 2 (not sure why; I think the 'base height' is half, either that or it's radius or something
			const int heightMultiplier = 2;
			float multipliedHeight = height * heightMultiplier;

			// Get the position in order to know how far to fall; Fall until under y=0
			float deathHeight = transform.position.y;
			float totalDistance = deathHeight + (multipliedHeight * -1);

			while (_diedTime + removalTime >= Time.fixedDeltaTime)
			{
				float fallDistance = (totalDistance / removalTime) * Time.fixedDeltaTime;
				transform.position -= new Vector3(0, fallDistance, 0);
				yield return waitForFixedFrame;
			}
		}

		protected virtual void OnNewActStart() { }

		protected virtual void OnActEnded() { }
	}
}