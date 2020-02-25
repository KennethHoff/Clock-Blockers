using System;
using System.Collections.Generic;

using ClockBlockers.Characters;
using ClockBlockers.Utility;

using UnityEngine;
using UnityEngine.SceneManagement;


namespace ClockBlockers.GameControllers
{
	// TODO: Completely remove this Script. What the fuck is a "Game Controller" - this entire thing is a game..

	public class GameController : MonoBehaviour
	{
		[HideInInspector]
		public Action newActStarted;

		[HideInInspector]
		public Action actEnded;

		public float TimeWhenActStarted { get; private set; }
		public int currentAct;

		private List<Player> _players;

		[SerializeField]
		private Player playerPrefab;

		public Transform CloneParent
		{
			get => cloneParent;
		}

		public Material DeadMaterial
		{
			get => deadMaterial;
		}

		public Material CompletedMaterial
		{
			get => completedMaterial;
		}

		public GameObject[] BulletHoles
		{
			get => bulletHoles;
		}

		public int FloatingPointPrecision
		{
			get => floatingPointPrecision;
		}

		[SerializeField]
		private string battleArena;

		[SerializeField]
		private Transform cloneParent;

		[SerializeField]
		private Material deadMaterial;

		[SerializeField]
		[Range(1, 6)]
		private int floatingPointPrecision = 6;

		[SerializeField]
		private GameObject[] bulletHoles;

		[SerializeField]
		private Material completedMaterial;

		private void Awake()
		{
			SceneManager.LoadScene(battleArena, LoadSceneMode.Additive);
			_players = new List<Player>();
			CreateNewPlayer();
		}

		private void CreateNewPlayer()
		{
			Player newPlayer = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
			newPlayer.gameController = this;
			_players.Add(newPlayer);
		}

		private static void SetCursorMode(bool locked)
		{
			Cursor.lockState = locked
				? CursorLockMode.Locked
				: CursorLockMode.None;
		}

		public static void ToggleCursorMode()
		{
			SetCursorMode(Cursor.lockState != CursorLockMode.Locked);
		}

		public void ClearClones()
		{
			Logging.Log("Clearing children");
			for (var i = 0; i < CloneParent.childCount; i++)
			{
				Destroy(CloneParent.GetChild(i).gameObject);
			}
		}

		public void EndCurrentAct()
		{
			actEnded();
		}

		public void StartNewAct()
		{
			newActStarted();
			TimeWhenActStarted = Time.time;
			currentAct++;
		}

		public void ResetAllPlayer()
		{
			_players.ForEach(controller => controller.ResetCharacter());
		}

		public Character SpawnCharacter(Character character, Vector3 position, Quaternion rotation)
		{
			return Instantiate(character, position, rotation);
		}
	}
}