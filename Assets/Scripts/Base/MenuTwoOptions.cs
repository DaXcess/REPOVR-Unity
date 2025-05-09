using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MenuTwoOptions : MonoBehaviour
{
    public string option1Text = "ON";

    public string option2Text = "OFF";

    public RectTransform optionsBox;

    public RectTransform optionsBoxBehind;

    public Vector3 targetPosition;

    public Vector3 targetScale;

    public DataDirector.Setting setting;

    public bool customEvents = true;

    public bool settingSet;

    public bool customFetch = true;

    public UnityEvent onOption1;

    public UnityEvent onOption2;

    public UnityEvent fetchSetting;

    public TextMeshProUGUI option1TextMesh;

    public TextMeshProUGUI option2TextMesh;

    public bool startSettingFetch = true;

    public void OnOption1()
    {
    }

    public void OnOption2()
    {
    }

    private void OnEnable()
    {
    }
}
