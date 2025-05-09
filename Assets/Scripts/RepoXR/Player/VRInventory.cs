using UnityEngine;

namespace RepoXR.Player
{
    public class VRInventory : MonoBehaviour
    {
        [SerializeField] private Transform visualsTransform;

        [SerializeField] private VRInventorySlot[] slots;

        [SerializeField] private Color holdColor;
        [SerializeField] private Color hoverColor;
        [SerializeField] private Color equippedColor;

        public VRRig rig;
    }
}