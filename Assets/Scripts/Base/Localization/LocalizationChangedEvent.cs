using UnityEngine;
using UnityEngine.Events;

public class LocalizationChangedEvent : MonoBehaviour
{
    public LocalizedAsset localizedAsset;
    public UnityEvent<string> onLocalizationChanged;
    public UnityEvent<bool> onTextDirectionalityChanged;
    public bool localizedAssetRequired = true;
}
