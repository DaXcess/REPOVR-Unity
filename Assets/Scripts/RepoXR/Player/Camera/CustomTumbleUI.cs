using UnityEngine;

namespace RepoXR.Player.Camera
{
    public class CustomTumbleUI : MonoBehaviour
    {
        public Color canNotExitColor1;
        public Color canNotExitColor2;
        public Color canExitColor1;
        public Color canExitColor2;

        [Space] public AnimationCurve introCurve;
        public float introSpeed;

        [Space] public AnimationCurve outroCurve;
        public float outroSpeed;

        [Space] public float updateTime;

        [Space] public GameObject[] parts1;
        public GameObject[] parts2;
    }
}