using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Scripting;

namespace RepoXR.Rendering
{
    [Preserve]
    public class CustomVignetteRenderer : PostProcessEffectRenderer<CustomVignette>
    {
        private static readonly int VignetteMode = Shader.PropertyToID("_Vignette_Mode");
        private static readonly int VignetteColor = Shader.PropertyToID("_Vignette_Color");
        private static readonly int VignetteSettings = Shader.PropertyToID("_Vignette_Settings");
        private static readonly int VignetteMask = Shader.PropertyToID("_Vignette_Mask");
        private static readonly int VignetteOpacity = Shader.PropertyToID("_Vignette_Opacity");
        private static readonly int VignetteActiveEye = Shader.PropertyToID("_Vignette_Active_Eye");
        
        private Shader shader;
        
        public override void Init()
        {
            shader = Shader.Find("Hidden/PostProcessing/VignetteVR");
        }

        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(shader);

            sheet.properties.SetColor(VignetteColor, settings.color.value);
            sheet.properties.SetFloat(VignetteActiveEye, context.xrActiveEye);
            
            if (settings.mode == UnityEngine.Rendering.PostProcessing.VignetteMode.Classic)
            {                
                var num = (1f - settings.roundness.value) * 6f + settings.roundness.value;

                sheet.properties.SetFloat(VignetteMode, 0f);
                sheet.properties.SetVector(VignetteSettings, new Vector4(settings.intensity.value * 3f, settings.smoothness.value * 5f, num, settings.rounded.value ? 1f : 0f));
                
                context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
                
                return;
            }
            sheet.properties.SetFloat(VignetteMode, 1f);
            sheet.properties.SetTexture(VignetteMask, settings.mask.value);
            sheet.properties.SetFloat(VignetteOpacity, Mathf.Clamp01(settings.opacity.value));
            
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}