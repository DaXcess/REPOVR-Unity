using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MenuSlider : MonoBehaviour
{
    [Serializable]
    public class CustomOption
    {
        [Space(25f)]
        [Header("____ Custom Option ____")]
        public string customOptionText;

        public UnityEvent onOption;

        public int customValueInt;
    }

    public string elementName = "Element Name";

    public TextMeshProUGUI elementNameText;

    public Transform sliderBG;

    public Transform barSize;

    public Transform barPointer;

    public RectTransform barSizeRectTransform;

    public Transform settingsBar;

    public Transform extraBar;

    private int settingSegments;

    public int startValue;

    public int endValue;

    public string stringAtStartOfValue;

    public string stringAtEndOfValue;

    public int buttonSegmentJump = 1;

    public int pointerSegmentJump = 1;

    public TextMeshProUGUI segmentText;

    public TextMeshProUGUI segmentMaskText;

    public RectTransform maskRectTransform;

    public bool wrapAround;

    public bool hasBar = true;

    public bool hasCustomOptions;

    public bool hasCustomValues;

    [Space]
    public UnityEvent onChange;

    public List<CustomOption> customOptions;

    public void OnIncrease()
    {
    }

    public void OnDecrease()
    {
    }
}
