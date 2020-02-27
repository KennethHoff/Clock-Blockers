using System;
using System.Collections;

using ClockBlockers.DataStructures;
using ClockBlockers.GameControllers;
using ClockBlockers.ReplaySystem.ReplayRunner;
using ClockBlockers.ReplaySystem.ReplaySpawner;
using ClockBlockers.ReplaySystem.ReplayStorage;
using ClockBlockers.Targetting;
using ClockBlockers.ToBeMoved;
using ClockBlockers.Utility;
using ClockBlockers.Weapons;

using UnityEngine;


namespace ClockBlockers.Characters.Scripts
{
	// TODO: Add a simple "Aim towards the middle of a character" AI for weapons.
	// TODO: Add Dot Product aiming, as opposed to RayCast aiming, for better-feeling aiming


	// This class's jobs are:
	
	// Move Camera																				< NEW CameraController
	// Move Character																			< NEW CharacterMovement
	// Shoot, Jump, Crouch etc..																< ??
	// Deal Damage																				< ??
	// Complete the actions proposed by the IReplayRunner Interface								< ??
	// Reset IReplayStorage data on new round.													< On the IReplayStorage itself
	
	
	// That's clearly way too many jobs.
	
	
	// Fixed the following:
	
	// Store Health																				< NEW HealthComponent

	
	[DisallowMultipleComponent]
	public partial class Character : MonoBehaviour, IInteractable
	{
		private WaitForFixedUpdate _waitForFixedFrame;

		internal Camera Cam { get; private set; }

		public CharacterController characterController;

		internal IReplayStorage replayStorage;

		internal IReplaySpawner replaySpawner;

		private HealthComponent _healthComponent;

		
		// There's a fundamental difference in architecture; They can't mix.
		internal ActionReplayRunner replayRunner;


		private CharacterBody _body;

		private IRayProvider _rayProvider;

		private ITargeter _targeter;

		private float _diedTime;

		public Vector3 StartPos { get; set; }
		public Quaternion StartRot { get; set; }

		public Quaternion CamStartRot { get; private set; }


		#region Set in inspector




		private Gun Gun
		{
			get => gun;
		}

		[SerializeField]
		private Gun gun;

		
		#endregion


		internal GameController gameController;
		[SerializeField]
		private FloatReference minCamAngle;

		[SerializeField]
		private FloatReference maxCamAngle;

		[SerializeField]
		[Header("Character Variables")]
		private FloatReference moveSpd;

		protected virtual void Awake()
		{
			
			replayStorage = GetComponent<IReplayStorage>();
			if (replayStorage == null) Logging.instance.LogIncorrectInstantiation("Replay Storage", this);

			replaySpawner = GetComponent<IReplaySpawner>();
			if (replaySpawner == null) Logging.instance.LogIncorrectInstantiation("Replay Spawner", this);

			replayRunner = GetComponent<ActionReplayRunner>();
			if (replayRunner == null) Logging.instance.LogIncorrectInstantiation("Replay Runner", this);

			_healthComponent = GetComponent<HealthComponent>();
			if (_healthComponent == null) Logging.instance.LogIncorrectInstantiation("Health Component", this);

			Cam = GetComponentInChildren<Camera>();
			characterController = GetComponent<CharacterController>();
			Gun.Holder = this;

			_body = GetComponentInChildren<CharacterBody>();

			_targeter = GetComponent<ITargeter>();
			_rayProvider = GetComponent<IRayProvider>();
			
			
			_waitForFixedFrame = new WaitForFixedUpdate(); 
		}

		protected virtual void Start()
		{
			if (gameController == null)
			{
				Logging.instance.LogIncorrectInstantiation("GameController", this);
			}

			AssignDelegates();

			Transform camTransform = Cam.transform;

			Transform charTransform = transform;

			StartPos = charTransform.position;
			StartRot = charTransform.rotation;

			CamStartRot = camTransform.rotation;
		}

		public void RotateCharacter(float yRotation)
		{
			var angle = new Vector3(0, yRotation, 0);
			transform.Rotate(angle);
		}

		public void RotateCamera(float rotation)
		{
			Vector3 currentAngle = Cam.transform.rotation.eulerAngles;

			float newX = currentAngle.x - rotation;

			// ReSharper disable once IdentifierTypo
			float preclampedX = newX > 180 ? newX - 360 : newX;
			float clampedX = Mathf.Clamp(preclampedX, minCamAngle, maxCamAngle);

			var newAngle = new Vector3(clampedX, 0, 0);

			Cam.transform.localRotation = Quaternion.Euler(newAngle);
		}

		public void MoveCharacterForward(float[] vector)
		{
			Transform transform1 = transform;
			Vector3 forward = transform1.forward;
			Vector3 right = transform1.right;
			forward.y = 0;
			right.y = 0;
			forward.Normalize();
			right.Normalize();

			Vector3 prelimMove = (forward * vector[1]) + (right * vector[0]);
			Vector3 move = prelimMove * moveSpd;

			MoveCharacter(move);
		}

		private void MoveCharacter(Vector3 vector)
		{
			characterController.Move(vector);
		}

		public void AttemptToShoot()
		{
			// TODO: Add Ammo checks etc..

			if (gun.CanShoot)
			{
				Gun.PullTrigger();
			}
		}

		

		public void SpawnReplay()
		{
			replaySpawner.SpawnLatestReplay();
		}

		public void OnHit(DamagePacket damagePacket, Vector3 hitPosition)
		{
			_healthComponent.DealDamage(damagePacket);
		}

		private Ray CreateRay()
		{
			return _rayProvider.CreateRay();
		}

		public Tuple<IInteractable, RaycastHit> GetTarget(float range)
		{
			return _targeter.GetInteractableFromRay(CreateRay(), range);
		}
				private void AssignDelegates()
		{
			replayRunner.MoveAction += Action_MoveCharacter;
			replayRunner.RotateCharacterAction += Action_RotateCharacter;
			replayRunner.RotateCameraAction += Action_RotateCamera;
			replayRunner.ShootAction += Action_AttemptToShoot;
			replayRunner.SpawnReplayAction += Action_SpawnReplay;
			replayRunner.CompletedAllActions += Action_CompletedAll;
		}

		private void UnassignDelegates()
		{
			replayRunner.MoveAction -= Action_MoveCharacter;
			replayRunner.RotateCharacterAction -= Action_RotateCharacter;
			replayRunner.RotateCameraAction -= Action_RotateCamera;
			replayRunner.ShootAction -= Action_AttemptToShoot;
			replayRunner.SpawnReplayAction -= Action_SpawnReplay;
			replayRunner.CompletedAllActions -= Action_CompletedAll;

		}

		private void OnDisable()
		{
			UnassignDelegates();
		}

		private void Action_MoveCharacter(float[] value)
		{
			MoveCharacterForward(value);
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
				yield return _waitForFixedFrame;
			}
		}

		public void RemoveFromGame()
		{
			Logging.instance.Log("Removed " + name, this);
		}
	}
}