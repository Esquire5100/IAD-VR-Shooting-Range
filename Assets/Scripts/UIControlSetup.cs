using TMPro;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Use this class to present locomotion control schemes and configuration preferences,
/// and respond to player input in the UI to set them.
/// </summary>
/// <seealso cref="LocomotionManager"/>

public class UIControlSetup : MonoBehaviour {
	//[SerializeField]
	//[Tooltip("Stores the behavior that will be used to configure locomotion control schemes and configuration preferences.")]
	//LocomotionManager locomotionManager;

	[SerializeField]
	[Tooltip("Stores the slider used to set linear value.")]
	XRSlider slider;

	[SerializeField]
	[Tooltip("The label that shows the current movement speed value.")]
	TextMeshPro sliderLabel;

	[SerializeField]
	[Tooltip("Stores the knob used to set angular value.")]
	XRKnob dial;

	[SerializeField]
	[Tooltip("The label that shows the current turn speed value.")]
	TextMeshPro dialLabel;

	[SerializeField]
    [Tooltip("Stores the wheel used to set angular value.")]
    XRKnob wheel;

    [SerializeField]
    [Tooltip("The label that shows the current turn speed value.")]
    TextMeshPro wheelLabel;

	Vector2 joystickValue;

	//[SerializeField]
	//[Tooltip("Stores the wheel used to set turn speed.")]
	//XRJoyStick joystick;

	[SerializeField]
	[Tooltip("The label that shows the current turn speed value.")]
	TextMeshPro joystickXLabel;

	[SerializeField]
	[Tooltip("The label that shows the current turn speed value.")]
	TextMeshPro joystickYLabel;

	public void SetSlider() {
		sliderLabel.text = slider.value.ToString("f1");
	}

	public void SetDial() {
		dialLabel.text = dial.value.ToString("f1");
	}

	public void SetWheel() {
		wheelLabel.text = wheel.value.ToString("f1");
	}

	public void OnJoystickValueChangeX(float x) {
		joystickValue.x = x;
		joystickXLabel.text = x.ToString("f1");
	}

	public void OnJoystickValueChangeY(float y) {
        joystickValue.y = y;
        joystickYLabel.text = y.ToString("f1");
    }
}