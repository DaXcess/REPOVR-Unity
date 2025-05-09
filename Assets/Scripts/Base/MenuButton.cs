using UnityEngine;

public class MenuButton : MonoBehaviour
{
	private enum ButtonState
	{
		Hover = 0,
		Clicked = 1,
		Normal = 2
	}

	public string buttonTextString = "BUTTON";

	public bool customHoverArea;

	public bool doButtonEffect = true;

	public bool holdLogic = true;

	public bool hasHold;

	private MenuPage parentPage;

	public bool middleAlignFix;

	public bool customColors;

	[Header("Custom Colors")]
	public Color colorNormal;

	public Color colorHover;

	public Color colorClick;
}
