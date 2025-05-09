using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuBigButton : MonoBehaviour
{
	public enum State
	{
		Main = 0,
		Edit = 1
	}

	public string buttonTitle = "";

	public string buttonName = "NewButton";

	public RawImage mainButtonBG;

	public RawImage behindButtonBG;

	public MenuButton menuButton;

	public TextMeshProUGUI buttonTitleTextMesh;

	private Color mainButtonMainColor;

	private Color behindButtonMainColor;

	public State state;
}