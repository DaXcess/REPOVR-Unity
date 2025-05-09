using UnityEngine;

public class MenuScrollBox : MonoBehaviour
{
    public RectTransform scrollSize;
    public RectTransform scroller;
    public RectTransform scrollHandle;
    public RectTransform scrollBarBackground;
    public GameObject scrollBar;
    
    public MenuSelectionBox menuSelectionBox;
    public MenuElementHover menuElementHover;
 
    public float heightPadding;
}
