using ClockBlockers.Utility;

using UnityEngine;
using UnityEngine.SceneManagement;


namespace ClockBlockers.GameControllers
{
	[RequireComponent(typeof(Transform))]
	public class GameController : MonoBehaviour
	{
		public static GameController instance;

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
		private string firstScene;

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
			SceneManager.LoadScene(firstScene, LoadSceneMode.Additive);
			instance = this;
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

		public static void ClearClones()
		{
			Logging.Log("Clearing children");
			for (var i = 0; i < GameController.instance.CloneParent.childCount; i++)
			{
				Destroy(GameController.instance.CloneParent.GetChild(i).gameObject);
			}
		}
	}
}