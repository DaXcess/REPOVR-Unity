using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MenuPage : MonoBehaviour
{
    public string menuHeaderName;
    public MenuPageIndex menuPageIndex;
    public TextMeshProUGUI menuHeader;
    public bool disableIntroAnimation;
    public bool disableOutroAnimation;

    public UnityEvent onPageEnd;
}

public enum MenuPageIndex
{
    VRSettings = 37449,
    VRSettingsCategory,
    VRShowcase
}