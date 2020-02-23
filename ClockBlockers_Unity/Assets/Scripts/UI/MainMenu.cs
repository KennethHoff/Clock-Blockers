using UnityEngine.SceneManagement;

namespace ClockBlockers.UI
{
    public class MainMenu : BaseGameMenu
    {

        public BaseGameMenu optionsMenu;
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
            optionsMenu.gameObject.SetActive(true);
            CloseMenu();
        }

        private void StartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1, LoadSceneMode.Single);
        }
    }
}