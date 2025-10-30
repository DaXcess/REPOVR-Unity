using UnityEngine;

namespace RepoXR.UI
{
    public class FocusSphere : MonoBehaviour
    {
        [SerializeField] private Renderer renderer;
        [SerializeField] private AnimationCurve animIn;
        [SerializeField] private AnimationCurve animOut;
    }
}