using Between_Names.Property_References;

using TMPro;

using Unity.Burst;

using UnityEngine;
using UnityEngine.UI;


namespace ClockBlockers.UI.Main_Menu
{
	// TODO: Split up each 'section' into separate classes..

	[BurstCompile]
	public class OptionsMenu : BaseGameMenu
	{
		private BaseGameMenu Main { get; set; }
		
		[Header("Mouse Sensitivity")]
		[SerializeField]
		private FloatReference horizontalSensitivity = null;

		[SerializeField]
		private FloatReference verticalSensitivity = null;

		[SerializeField]
		private TMP_InputField horizontalSensitivityInputField = null;

		[SerializeField]
		private TMP_InputField verticalSensitivityInputField = null;

		[SerializeField]
		private Slider horizontalSensitivitySlider = null;

		[SerializeField]
		private Slider verticalSensitivitySlider = null;

		private void Awake()
		{
			Main = ConnectedMenus[0];
		}

		private void OnEnable()
		{
			horizontalSensitivityInputField.text = horizontalSensitivity.ToString();
			verticalSensitivityInputField.text = verticalSensitivity.ToString();
			
			verticalSensitivitySlider.value = verticalSensitivity * 10;
			horizontalSensitivitySlider.value = horizontalSensitivity * 10;
		}

		public void SetVerticalSensitivity()
		{
			verticalSensitivity.Value = verticalSensitivitySlider.value / 10;
			verticalSensitivityInputField.text = verticalSensitivity.ToString("F1");
		}

		public void SetHorizontalSensitivity()
		{
			horizontalSensitivity.Value = horizontalSensitivitySlider.value / 10;
			horizontalSensitivityInputField.text = horizontalSensitivity.ToString("F1");
		}

		// private void OnGUI()
		// {
			// horizontalSensitivityInputField.text = horizontalSensitivity.ToString("F1");
			// verticalSensitivityInputField.text = verticalSensitivity.ToString("F1");
		// }

		public void OnPressBack()
		{
			OpenMenu(Main);
			CloseMenu();
		}
	}
}