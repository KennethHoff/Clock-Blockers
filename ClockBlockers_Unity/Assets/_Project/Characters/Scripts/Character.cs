using System.Collections;

using ClockBlockers.DataStructures;
using ClockBlockers.ReplaySystem.ReplayRunner;
using ClockBlockers.ReplaySystem.ReplaySpawner;
using ClockBlockers.ReplaySystem.ReplayStorage;
using ClockBlockers.ToBeMoved;
using ClockBlockers.Utility;

using UnityEngine;


namespace ClockBlockers.Characters
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
	
	// [25.02.2020]Store Health
		// NEW Health

	
	[DisallowMultipleComponent]
	[RequireComponent(typeof(IReplayStorage))]
	[RequireComponent(typeof(IReplayRunner))]
	[RequireComponent(typeof(IReplaySpawner))] 
	[RequireComponent(typeof(HealthComponent))]
	[RequireComponent(typeof(CharacterBody))]
	
	public class Character : MonoBehaviour, IInteractable
	{
		private WaitForFixedUpdate _waitForFixedFrame;

		private IReplayStorage _replayStorage;

		private IReplaySpawner _replaySpawner;
		
		// There's a fundamental difference in architecture; They can't mix.
		private ActionReplayRunner _replayRunner;

		private HealthComponent _healthComponent;

		private float _diedTime;
		
		private CharacterBody _body;

		private Renderer _bodyRenderer;
		
		[SerializeField]
		private Material deadMaterial;


		protected virtual void Awake()
		{
			_replayStorage = GetComponent<IReplayStorage>();
			if (_replayStorage == null) Logging.LogIncorrectInstantiation("Replay Storage", this);

			_replaySpawner = GetComponent<IReplaySpawner>();
			if (_replaySpawner == null) Logging.LogIncorrectInstantiation("Replay Spawner", this);

			_replayRunner = GetComponent<ActionReplayRunner>();
			if (_replayRunner == null) Logging.LogIncorrectInstantiation("Replay Runner", this);

			_healthComponent = GetComponent<HealthComponent>();
			if (_healthComponent == null) Logging.LogIncorrectInstantiation("Health Component", this);

			_body = GetComponentInChildren<CharacterBody>();
			_bodyRenderer = _body.GetComponent<Renderer>();

			_waitForFixedFrame = new WaitForFixedUpdate(); 
		}

		// protected virtual void Start()
		// {
			// AssignDelegates();

			// Transform camTransform = Cam.transform;

			// Transform charTransform = transform;

			// StartPos = charTransform.position;
			// StartRot = charTransform.rotation;

			// CamStartRot = camTransform.rotation;
		// }

		// public void RotateCharacter(float yRotation)
		// {
		// 	var angle = new Vector3(0, yRotation, 0);
		// 	transform.Rotate(angle);
		// }

		// public void RotateCamera(float rotation)
		// {
		// 	Vector3 currentAngle = Cam.transform.rotation.eulerAngles;
		//
		// 	float newX = currentAngle.x - rotation;
		//
		// 	// ReSharper disable once IdentifierTypo
		// 	float preclampedX = newX > 180 ? newX - 360 : newX;
		// 	float clampedX = Mathf.Clamp(preclampedX, minCamAngle, maxCamAngle);
		//
		// 	var newAngle = new Vector3(clampedX, 0, 0);
		//
		// 	Cam.transform.localRotation = Quaternion.Euler(newAngle);
		// }

		// public void AttemptToShoot()
		// {
		// 	// TODO: Add Ammo checks etc..
		//
		// 	if (gun.CanShoot)
		// 	{
		// 		Gun.PullTrigger();
		// 	}
		// }

		// public void SpawnReplay()
		// {
			// replaySpawner.SpawnLatestReplay();
		// }


		//	private void AssignDelegates()
		// {
		// 	replayRunner.MoveAction += Action_MoveCharacter;
		// 	replayRunner.RotateCharacterAction += Action_RotateCharacter;
		// 	replayRunner.RotateCameraAction += Action_RotateCamera;
		// 	replayRunner.ShootAction += Action_AttemptToShoot;
		// 	replayRunner.SpawnReplayAction += Action_SpawnReplay;
		// 	replayRunner.CompletedAllActions += Action_CompletedAll;
		// }
		//
		// private void UnassignDelegates()
		// {
		// 	replayRunner.MoveAction -= Action_MoveCharacter;
		// 	replayRunner.RotateCharacterAction -= Action_RotateCharacter;
		// 	replayRunner.RotateCameraAction -= Action_RotateCamera;
		// 	replayRunner.ShootAction -= Action_AttemptToShoot;
		// 	replayRunner.SpawnReplayAction -= Action_SpawnReplay;
		// 	replayRunner.CompletedAllActions -= Action_CompletedAll;
		//
		// }

		// private void OnDisable()
		// {
			// UnassignDelegates();
		// }

		// private void Action_MoveCharacter(float[] value)
		// {
		// 	MoveCharacterForward(value);
		// }
		//
		// private void Action_RotateCharacter(float[] value)
		// {
		// 	float rotation = value[0];
		// 	RotateCharacter(rotation);
		// }
		//
		// private void Action_RotateCamera(float[] value)
		// {
		// 	float rotation = value[0];
		// 	RotateCamera(rotation);
		// }
		//
		// private void Action_AttemptToShoot(float[] value)
		// {
		// 	AttemptToShoot();
		// }
		//
		// private void Action_SpawnReplay(float[] value)
		// {
		// 	SpawnReplay();
		// }
		//
		// private void Action_CompletedAll()
		// {
		// 	const float removalTime = 5f;
		//
		// 	Destroy(gameObject, removalTime);
		// 	_diedTime = Time.fixedDeltaTime;
		//
		// 	StartCoroutine(Co_FallThroughFloor(removalTime));
		// }

		private IEnumerator Co_FallThroughFloor(float removalTime)
		{
			_diedTime = Time.time;
			// Get the height of the body.
			float height = _body.GetComponent<MeshFilter>().mesh.bounds.extents.y;

			// Multiply it by 2 (not sure why; I think the 'base height' is half, either that or it's radius or something
			const int heightMultiplier = 2;
			float multipliedHeight = height * heightMultiplier;

			// Get the position in order to know how far to fall; Fall until under y=0
			float deathHeight = transform.position.y;
			float totalDistance = deathHeight + (multipliedHeight * -1);

			while (_diedTime + removalTime >= Time.time)
			{
				float fallDistance = (totalDistance / removalTime) * Time.fixedDeltaTime;
				transform.position -= new Vector3(0, fallDistance, 0);
				yield return _waitForFixedFrame;
			}
			
			gameObject.SetActive(false);
		}
		
		public void OnHit(DamagePacket damagePacket, Vector3 hitPosition)
		{
			_healthComponent.DealDamage(damagePacket);
		}
		
		
		public void Kill()
		{
			_bodyRenderer.material = deadMaterial;
			
			const float removalTime = 1.25f;
			
			_replayRunner.End();
			
			// Destroy(gameObject, removalTime);

			StartCoroutine(Co_FallThroughFloor(removalTime));
		}
	}
}