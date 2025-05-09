using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace RepoXR.Rendering
{
    [PostProcess(typeof(CustomVignetteRenderer), PostProcessEvent.AfterStack, "Custom/Vignette")]
    [Serializable]
    public class CustomVignette : PostProcessEffectSettings
    {
        public override bool IsEnabledAndSupported(PostProcessRenderContext context)
            => enabled.value && intensity.value > 0f;

        [Tooltip("Use the \"Classic\" mode for parametric controls. Use the \"Masked\" mode to use your own texture mask.")]
        public VignetteModeParameter mode = new()
        {
            value = VignetteMode.Classic
        };

        [Tooltip("Vignette color.")]
        public ColorParameter color = new()
        {
            value = new Color(0f, 0f, 0f, 1f)
        };

        [Range(0f, 1f)]
        [Tooltip("Amount of vignetting on screen.")]
        public FloatParameter intensity = new()
        {
            value = 0f
        };

        [Range(0.01f, 1f)]
        [Tooltip("Smoothness of the vignette borders.")]
        public FloatParameter smoothness = new()
        {
            value = 0.2f
        };

        [Range(0f, 1f)]
        [Tooltip("Lower values will make a square-ish vignette.")]
        public FloatParameter roundness = new()
        {
            value = 1f
        };

        [Tooltip("Set to true to mark the vignette to be perfectly round. False will make its shape dependent on the current aspect ratio.")]
        public BoolParameter rounded = new()
        {
            value = false
        };

        [Tooltip("A black and white mask to use as a vignette.")]
        public TextureParameter mask = new()
        {
            value = null
        };

        [Range(0f, 1f)]
        [Tooltip("Mask opacity.")]
        public FloatParameter opacity = new()
        {
            value = 1f
        };
    }
}