using Between_Names.Property_References;

using ClockBlockers.AI.AiControllers;
using ClockBlockers.Characters;
using ClockBlockers.Events;
using ClockBlockers.ReplaySystem;
using ClockBlockers.ReplaySystem.ReplayStorage;
using ClockBlockers.ToBeMoved;
using ClockBlockers.Utility;
using ClockBlockers.Visualizations;
using ClockBlockers.Weapons;

using Unity.Burst;

using UnityEngine;
using UnityEngine.InputSystem;


namespace ClockBlockers.Input
{
	[RequireComponent(typeof(Character))]
	[RequireComponent(typeof(CharacterMovement))]
	[BurstCompile]
	public class PlayerInputController : MonoBehaviour
	{
		private CharacterMovementNew _characterMovement;

		private Character _character;

		private IntervalReplayStorage _replayStorage;

		[SerializeField]
		private CameraController cameraController = null;

		[SerializeField]
		private Gun gun = null;

		[SerializeField]
		private FloatReference verticalMouseSensitivity = null;

		[SerializeField]
		private FloatReference horizontalMouseSensitivity = null;

		private Vector2 _moveInput;
		private float _sideToSideCharacterRotation;
		private float _upDownCameraRotation;
		private bool _inputEnabled;

		[SerializeField]
		private GameEvent forceEndActEvent = null;

		[SerializeField]
		private VisualizerBase testVisualizer = null;




		private AiController _controlledAi;

		private void Awake()
		{
			_replayStorage = GetComponent<IntervalReplayStorage>();
			Logging.CheckIfCorrectMonoBehaviourInstantiation(ref _replayStorage, this, "Replay Storage");

			_characterMovement = GetComponent<CharacterMovementNew>();
			Logging.CheckIfCorrectMonoBehaviourInstantiation(ref _characterMovement, this, "Character Movement");

			_character = GetComponent<Character>();
			Logging.CheckIfCorrectMonoBehaviourInstantiation(ref _character, this, "Character");

			_character.onKilled += CharacterDied;
		}

		private void CharacterDied()
		{
			SetCursorActive(true);
		}

		private void Update()
		{
			_characterMovement.Rotate(_sideToSideCharacterRotation * Time.deltaTime);
			cameraController.Rotate(_upDownCameraRotation * Time.deltaTime);

			if (_moveInput.magnitude > 0)
			{
				_characterMovement.SetForwardInputVelocity(_moveInput);
			}
		}

		private void OnEnable()
		{
			SetCursorActive(false);
		}

		private void OnLook(InputValue ctx)
		{
			if (!_inputEnabled)
			{
				_sideToSideCharacterRotation = 0;
				_upDownCameraRotation = 0;
				return;
			}
			var value = ctx.Get<Vector2>();

			_sideToSideCharacterRotation = value.x * horizontalMouseSensitivity;
			_upDownCameraRotation = value.y * verticalMouseSensitivity;
			
		}
		private void OnJump()
		{
			_characterMovement.Jump();
		}

		
		private void OnMovement(InputValue ctx)
		{
			if (!_inputEnabled)
			{
				_moveInput = Vector2.zero;
				return;
			}
			_moveInput = ctx.Get<Vector2>();
		}

		private void OnShoot()
		{
			if (!_inputEnabled) return;
			gun.PullTrigger();

			_replayStorage.SaveAction(Actions.Shoot);
		}

		private void OnStartNewAct() // Obviously a test feature
		{
			if (!_inputEnabled) return;
			forceEndActEvent.Raise();
		}

		private void OnIncreaseTimescale() // Obviously a test feature
		{
			Time.timeScale += 1;
			Logging.Log("Increasing timescale. Now at: " + Time.timeScale);
		}

		private void OnDecreaseTimescale() // Obviously a test feature
		{
			Time.timeScale -= 1;
			Logging.Log("Decreasing timescale. Now at: " + Time.timeScale);
		}

		private void OnToggleCursor()
		{
			ToggleCursor();
		}

		private void OnAim()
		{
			TakeControlOverTargetAi();
		}

		private void OnMiddleClick()
		{
			CreateVisualizerAtTargetPosition();
		}

		private void TakeControlOverTargetAi()
		{
			_controlledAi?.WithdrawControl(this);

			// Not a fan of using the Gun's CreateRay here, as it's not related to the gun.
			Ray ray = gun.CreateRay();

			bool hitSomething = RayCaster.CastRay(ray, float.MaxValue, out RaycastHit hit);

			if (!hitSomething) return;

			var aiController = hit.transform.GetComponent<AiController>();
			if (aiController == null) return;


			_controlledAi = aiController;

			Logging.Log($"Gotcha, {_controlledAi.name}!");

			_controlledAi.TakeControl(this);
		}

		private void CreateVisualizerAtTargetPosition()
		{
			// Not a fan of using the Gun's CreateRay here, as it's not related to the gun.
			Ray ray = gun.CreateRay();
			
			bool hitSomething = RayCaster.CastRay(ray, float.MaxValue, out RaycastHit hit);

			if (!hitSomething) return;

			Vector3 hitPoint = hit.point;

			const float yDistAboveGround = 5;
			
			hitPoint.y += yDistAboveGround;

			VisualizerBase visualizer = Instantiate(testVisualizer, hitPoint, Quaternion.identity);

			Transform visualizerTransform = visualizer.transform;
			
			visualizerTransform.LookAt(transform);
			
			Vector3 currRot = visualizerTransform.rotation.eulerAngles;
			visualizer.transform.rotation = Quaternion.Euler(0, currRot.y, 0);

			if (_controlledAi == null) return;

			_controlledAi.aiPathfinder.EndCurrentPath();
			_controlledAi.aiPathfinder.RequestPath(hitPoint);
		}

		private void ToggleCursor()
		{
			SetCursorActive(!_inputEnabled);
		}

		private void SetCursorActive(bool toggle)
		{
			LockCursor(!toggle);
			_inputEnabled = !toggle;
		}


		private static void LockCursor(bool locked)
		{
			Cursor.lockState = locked
				? CursorLockMode.Locked
				: CursorLockMode.None;
		}
	}
}