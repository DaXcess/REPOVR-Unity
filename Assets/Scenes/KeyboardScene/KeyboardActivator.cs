using Microsoft.MixedReality.Toolkit.Experimental.UI;
using RepoXR.ThirdParty.MRTK;
using UnityEngine;

public class KeyboardActivator : MonoBehaviour
{
    private NonNativeKeyboard keyboard;
    
    private void Awake()
    {
        keyboard = GetComponent<NonNativeKeyboard>();
    }

    private void Start()
    {
        keyboard.PresentKeyboard();
    }
}
