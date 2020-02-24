using System.Linq;

using ClockBlockers.Actions;
using ClockBlockers.Components;
using ClockBlockers.DataStructures;
using ClockBlockers.GameControllers;
using ClockBlockers.Utility;

using UnityEngine;



namespace ClockBlockers.Characters
{
	// TODO: Add a simple "Aim towards the middle of a character" AI for weapons.
	// TODO: Add Dot Product aiming, as opposed to RayCast aiming, for better-feeling aiming
	public abstract class BaseController : MonoBehaviour, IInteractable
	{
		internal Camera Cam { get; private set; }

		private CharacterController Controller { get; set; }

		internal ActionStorage actionStorage;


		protected float SpawnTime { get; set; }

		protected float TimeAlive
		{
			get => Time.fixedTime - SpawnTime;
		}

		protected Vector3 StartPos { get; private set; }

		protected Quaternion StartRot { get; private set; }


		internal Vector3 moveVector;

		private float CurrHealth { get; set; }


		#region Set in inspector

		protected bool DebugLogEveryAction
		{
			get => debugLogEveryAction;
		}


		private float MoveSpd
		{
			get => moveSpd;
			set => moveSpd = value;
		}

		private float JumpVelocity
		{
			get => jumpVelocity;
			set => jumpVelocity = value;
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


		private float MaxHealth
		{
			get => maxHealth;
			set => maxHealth = value;
		}


		private float Armor
		{
			get => armor;
		}

		private float Shielding
		{
			get => shielding;
		}

		private bool IsAlive { get; set; }


		private GameObject ClonePrefab
		{
			get => clonePrefab;
		}


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
			Cam = GetComponentInChildren<Camera>();
			Controller = GetComponent<CharacterController>();
			Gun.Holder = this;
		}

		protected virtual void Start()
		{
			Transform transform1 = transform;
			StartPos = transform1.position;
			StartRot = transform1.rotation;

			SpawnTime = Time.fixedTime;
		}

		protected virtual void FixedUpdate()
		{
			//AffectGravity();
			//characterController.Move(moveVector);
		}

		protected void AffectGravity()
		{
			//if (characterController.velocity.y == 0) moveVector.y = 0;
			if (Controller.isGrounded)
			{
				Vector3 tempGravity = Physics.gravity * 0.1f;
				if (moveVector.y < tempGravity.y) moveVector = tempGravity;
				return;
			}

			moveVector += Physics.gravity * Time.fixedDeltaTime;
		}


		protected virtual void AttemptToJump()
		{
			if (Controller.isGrounded)
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
			Vector3 roundedVector = move.Round(GameController.Instance.FloatingPointPrecision);

			MoveCharacter(roundedVector);
		}

		protected void MoveCharacter(Vector3 vector)
		{
			Controller.Move(vector);

			// transform.position += vector;
		}

		protected virtual void AttemptToShoot()
		{
			// TODO: Add Ammo checks etc..
			Gun.PullTrigger();
		}


		protected void HealToFull()
		{
			CurrHealth = MaxHealth;
		}

		private void AttemptDealDamage(DamagePacket damagePacket)
		{
			DealDamage(damagePacket);
		}

		/// <summary>
		///     Deal Damage means the entity in question is dealing damage, not dealing damage to other entities.
		///     ie: This deals damage to itself.
		/// </summary>
		private void DealDamage(DamagePacket damagePacket)
		{
			float finalDamage = damagePacket.damage - Armor;
			CurrHealth -= finalDamage;
			if (CurrHealth <= 0)
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
			GetComponentInChildren<CharacterBody>().GetComponent<Renderer>().material =
				GameController.Instance.DeadMaterial;

			StopAllCoroutines();
			Destroy(gameObject, 1.25f);
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

		[ContextMenu("Move forward 10")]
		public void MoveForward10()
		{
			MoveCharacterForward(new[] {0f, 10f});
		}
	}
}