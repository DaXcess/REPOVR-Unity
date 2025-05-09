using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace RepoXR.UI.Settings
{
    public class FloatMenuSlider : MonoBehaviour
    {
        [Serializable]
        public class CustomOption
        {
            public string customOptionText;
            public UnityEvent onOption;
            public int customValueInt;
        }
        
        public Transform sliderBackground;
        public Transform barSize;
        public Transform barPointer;
        public RectTransform barSizeRectTransform;
        public Transform settingsBar;

        public TextMeshProUGUI segmentText;
        public TextMeshProUGUI segmentMaskText;
        public RectTransform maskRectTransform;

        public float startValue;
        public float endValue;

        public string stringAtStartOfValue;
        public string stringAtEndOfValue;

        public float buttonSegmentJump = 1;
        public float pointerSegmentJump = 1;

        public bool hasBar = true;
        public bool hasCustomOptions;
        public bool hasCustomValues;

        [Space] 
        public UnityEvent onChange;
        public List<CustomOption> customOptions;

        public void OnIncrease()
        {
            throw new NotImplementedException();
        }

        public void OnDecrease()
        {
            throw new NotImplementedException();
        }
    }
}