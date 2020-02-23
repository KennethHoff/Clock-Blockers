namespace ClockBlockers.UI {
    public class OptionsMenu : BaseGameMenu
    {
        public BaseGameMenu mainMenu;
        public void OnPressBack()
        {
            // TODO: Add a way to go back to "Previous game menu".

            mainMenu.gameObject.SetActive(true);
            CloseMenu();

        }
    }
}
