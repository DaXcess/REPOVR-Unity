// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using UnityEngine;

namespace RepoXR.ThirdParty.MRTK
{
    /// <summary>
    /// This class switches back and forth between two symbol boards that otherwise do not fit on the keyboard entirely
    /// </summary>
    public class SymbolKeyboard : MonoBehaviour
    {
        [SerializeField] 
        private NonNativeKeyboard m_Keyboard;
        
        [SerializeField]
        private UnityEngine.UI.Button m_PageBck = null;

        [SerializeField]
        private UnityEngine.UI.Button m_PageFwd = null;

        private void Update()
        {
            // Visual reflection of state.
            m_PageBck.interactable = m_Keyboard.IsShifted;
            m_PageFwd.interactable = !m_Keyboard.IsShifted;
        }
    }
}
