using System;
using System.Collections;
using System.Linq;

using ClockBlockers.Actions;
using ClockBlockers.Components;
using ClockBlockers.DataStructures;
using ClockBlockers.GameControllers;
using ClockBlockers.Targetting;
using ClockBlockers.Utility;

using UnityEngine;


namespace ClockBlockers.Characters
{
	// TODO: Add a simple "Aim towards the middle of a character" AI for weapons.
	// TODO: Add Dot Product aiming, as opposed to RayCast aiming, for better-feeling aiming


	[RequireComponent(typeof(IRayProvider))]
	[RequireComponent(typeof(ITargeter))]
	public abstract partial class BaseController : MonoBehaviour, IInteractable
	{
		internal Camera Cam { get; private set; }

		protected CharacterController characterController;

		internal ActionStorage actionStorage;

		internal ActionRunner actionRunner;

		internal CharacterBody body;

		internal Renderer bodyRenderer;

		[SerializeField]
		private IRayProvider rayProvider;

		[SerializeField]
		private ITargeter targeter;

		protected float spawnTime;

		private float _diedTime;

/*
		protected float TimeAlive
		{
			get => Time.fixedTime - SpawnTime;
		}
*/

		protected Vector3 StartPos { get; private set; }

		protected Quaternion StartRot { get; private set; }


		protected Vector3 moveVector;


		#region Set in inspector

		protected bool DebugLogEveryAction
		{
			get => debugLogEveryAction;
		}


		private float MoveSpd
		{
			get => moveSpd;
/*
			set => moveSpd = value;
*/
		}

		private float JumpVelocity
		{
			get => jumpVelocity;
/*
			set => jumpVelocity = value;
*/
		}

		private float MinCamAngle
		{
			get => minCamAngle;
		}

		private float MaxCamAngle
		{
			get => maxCamAngle;
		}

		protected bool RecordActions
		{
			get => recordActions;
		}

		protected bool EnableRecursiveReplays
		{
			get => enableRecursiveReplays;
		}

		private float Health
		{
			get => _health;
			set => _health = Mathf.Clamp(value, 0, maxHealth);
		}

		private float _health;

		private float MaxHealth
		{
			get => maxHealth;
/*
			set => maxHealth = value;
*/
		}


		private float Armor
		{
			get => armor;
		}

		private float Shielding
		{
			get => shielding;
		}

/*
		private bool IsAlive { get; set; }
*/


/*
		private GameObject ClonePrefab
		{
			get => clonePrefab;
		}
*/


		private GunController Gun
		{
			get => gun;
		}

		[SerializeField]
		[Header("Setup Variables")]
		private GameObject clonePrefab;

		[SerializeField]
		private GunController gun;

		[SerializeField]
		private float minCamAngle;

		[SerializeField]
		private float maxCamAngle;

		[SerializeField]
		private bool recordActions;

		[SerializeField]
		private bool enableRecursiveReplays;

		[SerializeField]
		[Space(10)]
		private float maxHealth;

		[SerializeField]
		private float armor;

		[SerializeField]
		private float shielding;

		[SerializeField]
		[Header("Debug Variables")]
		private bool debugLogEveryAction;

		[SerializeField]
		[Header("Character Variables")]
		private float moveSpd;

		[SerializeField]
		private float jumpVelocity;

		#endregion


		protected virtual void Awake()
		{
			actionStorage = actionStorage ?? new ActionStorage();
			actionRunner = actionRunner ? actionRunner : gameObject.AddComponent<ActionRunner>();
			Cam = GetComponentInChildren<Camera>();
			characterController = GetComponent<CharacterController>();
			Gun.Holder = this;
			AssignDelegates();

			body = GetComponentInChildren<CharacterBody>();
			bodyRenderer = body.GetComponent<Renderer>();


			targeter = GetComponent<ITargeter>();
			rayProvider = GetComponent<IRayProvider>();
		}

		protected virtual void Start()
		{
			Transform transform1 = transform;
			StartPos = transform1.position;
			StartRot = transform1.rotation;

			spawnTime = Time.fixedTime;
		}

		// protected virtual void FixedUpdate()
		// {
		// 	//AffectGravity();
		// 	//characterController.Move(moveVector);
		// }


		protected void AffectGravity()
		{
			if (characterController.isGrounded)
			{
				Vector3 tempGravity = Physics.gravity * 0.1f;
				if (moveVector.y < tempGravity.y) moveVector = tempGravity;
				return;
			}

			moveVector += Physics.gravity * Time.fixedDeltaTime;
		}


		protected virtual void AttemptToJump()
		{
			if (characterController.isGrounded)
			{
				ExecuteJump();
			}
		}

		protected virtual void ExecuteJump()
		{
			moveVector = Vector3.up * JumpVelocity;

			//Logging.Log("Jumped!");
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
			Vector3 roundedVector = move.Round(GameController.instance.FloatingPointPrecision);

			MoveCharacter(roundedVector);
		}

		protected void MoveCharacter(Vector3 vector)
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


		protected void HealToFull()
		{
			Health = MaxHealth;
		}

		private void AttemptDealDamage(DamagePacket damagePacket)
		{
			DealDamage(damagePacket);
		}

		private void DealDamage(DamagePacket damagePacket)
		{
			float finalDamage = damagePacket.damage - Armor;
			float remainingDamage = finalDamage;

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
			bodyRenderer.material = GameController.instance.DeadMaterial;

			float removalTime = 1.25f;

			// StopAllCoroutines();

			// Man, that felt satisfying to replace.
			// I never wanted to call StopAllCoroutines() on this class in the first place, because:
			// I knew that, if I wanted to do other non-action related Coroutines in the future, I'd have to find another way to only stop the action ones.
			actionRunner.StopAllCoroutines();

			Destroy(gameObject, removalTime);

			StartCoroutine(Co_FallThroughFloor(removalTime));
		}


		protected virtual void SpawnReplay()
		{
			CharacterAction[] actions;

			// On the off-chance that both the characterActions *and* actionArray have values, take the values from the characterActions.
			if (actionStorage.NewlyAddedActions.Count > 0 || actionStorage.ReplayActions.Length > 0)
			{
				actions = actionStorage.NewlyAddedActions.Count > 0
					? actionStorage.NewlyAddedActions.ToArray()
					: actionStorage.ReplayActions;
			}
			else
			{
				actions = new CharacterAction[0];
				Logging.LogWarning("No actionArray or characterAction", this);
			}

			SpawnReplay(actions);
		}

		protected virtual void SpawnReplay(CharacterAction[] actions)
		{
			GameObject clone = ActionSystems.SpawnReplay(clonePrefab, StartPos, StartRot, actions);
			var cloneController = clone.GetComponent<CloneController>();

			cloneController.moveSpd = moveSpd;
		}

		public void OnHit(DamagePacket damagePacket, Vector3 hitPosition)
		{
			AttemptDealDamage(damagePacket);
		}

		private Ray CreateRay()
		{
			return rayProvider.CreateRay(Cam);
		}

		public Tuple<IInteractable, RaycastHit> GetTarget()
		{
			return targeter.GetInteractableFromRay(CreateRay());
		}
	}


	public abstract partial class BaseController
	{
		protected virtual void AssignDelegates()
		{
			actionRunner.moveAction = Action_MoveCharacter;
			actionRunner.rotateCharacterAction = Action_RotateCharacter;
			actionRunner.rotateCameraAction = Action_RotateCamera;
			actionRunner.jumpAction = Action_AttemptToJump;
			actionRunner.shootAction = Action_AttemptToShoot;
			actionRunner.spawnReplayAction = Action_SpawnReplay;
			actionRunner.completedAllActions = Action_CompletedAll;
		}

		protected virtual void Action_MoveCharacter(float[] value)
		{
			float[] move = {value[0], value[1]};
			MoveCharacterForward(move);
		}

		protected virtual void Action_RotateCharacter(float[] value)
		{
			float rotation = value[0];
			RotateCharacter(rotation);
		}

		protected virtual void Action_RotateCamera(float[] value)
		{
			float rotation = value[0];
			RotateCamera(rotation);
		}

		protected virtual void Action_AttemptToJump(float[] value)
		{
			AttemptToJump();
		}

		protected virtual void Action_AttemptToShoot(float[] value)
		{
			AttemptToShoot();
		}

		protected virtual void Action_SpawnReplay(float[] value)
		{
			SpawnReplay();
		}

		protected virtual void Action_CompletedAll()
		{
			const float removalTime = 5f;

			Destroy(gameObject, removalTime);
			_diedTime = Time.fixedDeltaTime;

			StartCoroutine(Co_FallThroughFloor(removalTime));
		}

		private IEnumerator Co_FallThroughFloor(float removalTime)
		{
			var yieldInstruction = new WaitForFixedUpdate();

			// Get the height of the body.
			float height = body.GetComponent<MeshFilter>().mesh.bounds.extents.y;

			// Multiply it by 2 (not sure why; I think the 'base height' is half, either that or it's radius or something
			const int heightMultiplier = 2;
			float multipliedHeight = height * heightMultiplier;

			// Get the position in order to know how far to fall; Fall until under y=0
			float startPos = transform.position.y;
			float totalDistance = startPos + multipliedHeight;

			while (_diedTime + removalTime >= Time.fixedDeltaTime)
			{
				float fallDistance = (totalDistance / removalTime) * Time.fixedDeltaTime;
				transform.position -= new Vector3(0, fallDistance, 0);
				yield return yieldInstruction;
			}
		}
	}
}