using Unity.Burst;

using UnityEngine;


namespace ClockBlockers.UI.Main_Menu
{
	[BurstCompile]
	public abstract class BaseGameMenu : MonoBehaviour
	{
		[SerializeField]
		private BaseGameMenu[] connectedMenus = null;

		protected BaseGameMenu[] ConnectedMenus => connectedMenus;

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