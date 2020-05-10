using System.Globalization;

using Between_Names.Property_References;

using TMPro;

using Unity.Burst;

using UnityEngine;
using UnityEngine.UI;


namespace ClockBlockers.UI.Main_Menu
{
	
	[BurstCompile]
	// TODO: Split up each 'section' into separate classes..
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
			verticalSensitivityInputField.text = (verticalSensitivity * 10f).ToString(CultureInfo.CurrentCulture);
		}

		public void SetHorizontalSensitivity()
		{
			horizontalSensitivity.Value = horizontalSensitivitySlider.value / 10;
			horizontalSensitivityInputField.text = (horizontalSensitivity * 10f).ToString(CultureInfo.CurrentCulture);
		}

		// private void OnGUI()
		// {
		// 	horizontalMovementInputField.text = horizontalMovement.ToString();
		// 	verticalMovementInputField.text = verticalMovement.ToString();
		// }

		public void OnPressBack()
		{
			OpenMenu(Main);
			CloseMenu();
		}
	}
}