using UnityEngine;

namespace RepoXR.Data
{
    [CreateAssetMenu(fileName = "AnimationCurve", menuName = "RepoXR/Animation Curve", order = 1)]
    public class AnimationCurveData : ScriptableObject
    {
        public AnimationCurve curve;
    }   
}