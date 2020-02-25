using ClockBlockers.Characters;
using ClockBlockers.NewReplaySystem.ReplayStorage;

using UnityEngine;


namespace ClockBlockers.NewReplaySystem.ReplaySaver
{
	public class IntervalReplaySaver : MonoBehaviour, IReplaySaver
	{
		private Character _character;

		private Transform _characterTransform;

		private IntervalReplayStorage _replayStorage;

		[SerializeField]
		private float saveInterval = 0.1f;

		private float _timer;

		private void Awake()
		{
			_character = GetComponent<Character>();
			_replayStorage = GetComponent<IntervalReplayStorage>();
			_characterTransform = _character.transform;
		}

		private void Update()
		{
			_timer++;

			if (!(_timer >= saveInterval)) return;

			_timer = 0;
			SaveCurrentTransform();
		}

		private void SaveCurrentTransform()
		{
			var currentTranslation = new Translation(_characterTransform.position, _characterTransform.rotation);
			_replayStorage.SaveTranslationData(currentTranslation);
		}

		public void SaveAction(CharacterAction characterAction)
		{			
			_replayStorage.SaveCharacterAction(characterAction);
		}

		public void PushActDataToRound()
		{
			throw new System.NotImplementedException();
		}
	}
}