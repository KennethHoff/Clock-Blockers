using System.Collections.Generic;

using Between_Names.Property_References;

using ClockBlockers.Characters;
using ClockBlockers.MatchData;

using UnityEngine;


namespace ClockBlockers.StateMachines.AI.Conditions
{
	internal class EnemySpotted : ICondition
	{
		private readonly Character _character;
		private readonly Transform _thisTransform;
		private readonly FloatReference _viewAngle;
		private readonly FloatReference _viewRange;
		private readonly Act _act;

		public EnemySpotted(Character character, Transform thisTransform, FloatReference viewAngle, FloatReference viewRange, Act act)
		{
			_character = character;
			_thisTransform = thisTransform;
			_viewAngle = viewAngle;
			_viewRange = viewRange;
			_act = act;
		}

		public bool Fulfilled()
		{
			Vector3 position = _thisTransform.position;
			Vector3 forwardVector = _thisTransform.forward;

			IEnumerable<Character> enemyCharacters = _act.GetEnemyCharacters(_character);

			foreach (Character character in enemyCharacters)
			{
				Vector3 targetDir = character.transform.position - position;

				float distance = targetDir.magnitude;

				if (distance > _viewRange) continue;

				float angle = Vector3.Angle(targetDir, forwardVector);

				if (angle > _viewAngle) continue;

				return true;
			}

			return false;
		}
	}
}