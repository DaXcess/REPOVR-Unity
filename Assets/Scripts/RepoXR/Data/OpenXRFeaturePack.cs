using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.OpenXR.Features;

namespace RepoXR.Data
{
    [CreateAssetMenu(fileName = "FeaturePack", menuName = "RepoXR/Feature Pack")]
    public class OpenXRFeaturePack : ScriptableObject
    {
        [SerializeReference] private List<OpenXRFeature> features = new();

        public IReadOnlyList<OpenXRFeature> Features => features;
    }
}