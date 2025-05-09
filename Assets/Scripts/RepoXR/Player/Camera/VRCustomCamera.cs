using UnityEngine;
using UnityEngine.UI;

namespace RepoXR.Player.Camera
{
    public class VRCustomCamera : MonoBehaviour
    {
        [SerializeField] protected UnityEngine.Camera mainCamera;
        [SerializeField] protected UnityEngine.Camera topCamera;
        [SerializeField] protected UnityEngine.Camera uiCamera;

        [SerializeField] protected Image overlayImage;
    }
}