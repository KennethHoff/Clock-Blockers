using System;
using System.Collections;

using ClockBlockers.MatchData;
using ClockBlockers.ToBeMoved;
using ClockBlockers.ToBeMoved.DataStructures;
using ClockBlockers.Utility;

using Unity.Burst;

using UnityEngine;


namespace ClockBlockers.Characters
{
	// TODO: Completely remove this class.

	// TODO: Add a simple "Aim towards the middle of a character" AI for weapons.
	
	// DONE: Add Dot Product aiming, as opposed to RayCast aiming, for better-feeling aiming
		// I did this. Doesn't really work on non-spheres.


	
	
	// This class's jobs are:
	
	// Move Camera																				< NEW CameraController
	// Shoot, Jump, Crouch etc..																< ??
	
	
	// That's clearly way too many jobs.
	
	
	// Fixed the following:
	
	// [25.02.2020]Store Health
		// NEW Health
		
	// >> The following were altered some time between 25.02 and 25.04 <<
		
	//  Reset IReplayStorage data on new round.	
		// No longer a task. The character is no longer reset, and instead is stored within the act object.
		
	// Replay Translations and actions
		// IReplayRunner
		
	// Deal Damage
		// Gun
		
	// Move Character
		// NEW CharacterMovement





	[BurstCompile]
	public class Character : MonoBehaviour, IInteractable
	{
		private WaitForFixedUpdate _waitForFixedFrame;

		private HealthComponent _healthComponent;

		private float _diedTime;
		
		private CharacterBodyTag _body;

		private Renderer _bodyRenderer;
		
		[SerializeField]
		private Material deadMaterial = null;

		public Action onKilled;
		
		// This does not feel like the best solution
		public Act Act { get; set; }

		protected void Awake()
		{
			_healthComponent = GetComponent<HealthComponent>();
			Logging.CheckIfCorrectMonoBehaviourInstantiation(_healthComponent, this, "Health Component");

			_body = GetComponentInChildren<CharacterBodyTag>();
			Logging.CheckIfCorrectMonoBehaviourInstantiation(_body, this, "Character Body Tag");

			_bodyRenderer = _body.GetComponent<Renderer>();
			Logging.CheckIfCorrectComponentInstantiation(ref _bodyRenderer, this, "Renderer");

			_waitForFixedFrame = new WaitForFixedUpdate(); 
		}

		private IEnumerator FallThroughFloorRoutine(float removalTime)
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
			
			onKilled?.Invoke();

			StartCoroutine(FallThroughFloorRoutine(removalTime));
		}

		public void Inject(Act act)
		{
			Act = act;
		}
	}
}