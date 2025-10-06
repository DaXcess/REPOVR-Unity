using UnityEngine;

public class OnScreenVisualizer : MonoBehaviour
{
    [SerializeField] private float paddingWidth;
    [SerializeField] private float paddingHeight;

    private void OnGUI()
    {
        var padW = Screen.width * paddingWidth;
        var padH = Screen.height * paddingHeight;

        var rect = new Rect(-padW, -padH, Screen.width + 2 * padW, Screen.height + 2 * padH);

        GUI.color = Color.green;
        GUI.DrawTexture(rect, Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 0, Color.red, 2, 2);
    }
}
