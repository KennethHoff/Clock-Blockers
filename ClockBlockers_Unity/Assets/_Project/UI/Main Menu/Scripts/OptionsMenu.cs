namespace ClockBlockers.UI.Main_Menu.Scripts
{
	public class OptionsMenu : BaseGameMenu
	{
		private BaseGameMenu Main { get; set; }

		private void Awake()
		{
			Main = ConnectedMenus[0];
		}

		public void OnPressBack()
		{
			OpenMenu(Main);
			CloseMenu();
		}
	}
}