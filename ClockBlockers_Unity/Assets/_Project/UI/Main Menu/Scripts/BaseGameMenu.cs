using UnityEngine;


namespace ClockBlockers.UI.Main_Menu.Scripts
{
	public abstract class BaseGameMenu : MonoBehaviour
	{
		[SerializeField]
		private BaseGameMenu[] connectedMenus;

		protected BaseGameMenu[] ConnectedMenus
		{
			get => connectedMenus;
		}

		protected void CloseMenu()
		{
			gameObject.SetActive(false);
		}

		protected static void OpenMenu(BaseGameMenu menu)
		{
			menu.gameObject.SetActive(true);
		}

		protected static void QuitGame()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
			    Application.OpenURL(webplayerQuitURL);
#else
			    Application.Quit();
#endif
		}
	}
}