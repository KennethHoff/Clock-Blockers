using UnityEngine;


namespace ClockBlockers.UI
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
			this.gameObject.SetActive(false);
		}

		protected void OpenMenu(BaseGameMenu menu)
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