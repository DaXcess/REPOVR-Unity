using System;
using TMPro;
using UnityEngine;

namespace RepoXR.UI.Settings
{
    public class SettingOption : MonoBehaviour
    {
        public string settingCategory;
        public string settingName;
        public TextMeshProUGUI settingText;

        public RectTransform rectTransform;

        public void FetchBoolOption()
        {
            throw new NotImplementedException();
        }

        public void UpdateBool(bool value)
        {
            throw new NotImplementedException();
        }

        public void UpdateInt(int value)
        {
            throw new NotImplementedException();
        }

        public void UpdateFloat(float value)
        {
            throw new NotImplementedException();
        }

        public void UpdateSlider()
        {
            throw new NotImplementedException();
        }
    }
}