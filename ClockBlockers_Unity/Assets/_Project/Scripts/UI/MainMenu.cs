using UnityEngine.SceneManagement;


namespace ClockBlockers.UI
{
	public class MainMenu : BaseGameMenu
	{
		private BaseGameMenu _optionsMenu;

		private void Awake()
		{
			_optionsMenu = ConnectedMenus[0];
		}

		public void OnPressPlay()
		{
			StartGame();
		}

		public void OnPressOptions()
		{
			OpenOptionsMenu();
		}

		public void OnPressQuit()
		{
			QuitGame();
		}

		private void OpenOptionsMenu()
		{
			OpenMenu(_optionsMenu);
			CloseMenu();
		}

		private static void StartGame()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
		}
	}
}