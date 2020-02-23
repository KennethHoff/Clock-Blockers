using UnityEngine;

namespace ClockBlockers.UI {
    public abstract class BaseGameMenu : MonoBehaviour
    {
        public void CloseMenu()
        {
            this.gameObject.SetActive(false);
        }

        public void QuitGame()
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