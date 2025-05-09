Shader "Hidden/PostProcessing/VignetteVR"
{
    HLSLINCLUDE
        
        #pragma vertex VertDefault
        #pragma fragment FragVignette

        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/Colors.hlsl"
        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/Sampling.hlsl"
        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/Builtins/Distortion.hlsl"
        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/Builtins/Dithering.hlsl"

        CBUFFER_START(UnityPerFrame)
        #if !defined(USING_STEREO_MATRICES)
            float4x4 glstate_matrix_projection;
        #endif
        CBUFFER_END

        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
        float4 _MainTex_TexelSize;

        // Vignette
        half3 _Vignette_Color;
        half4 _Vignette_Settings; // x: intensity, y: smoothness, z: roundness, w: rounded

        half4 FragVignette(VaryingsDefault i) : SV_Target
        {
            float2 uv = i.texcoord;
            half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo);

            half2 center = (0.5).xx;
            center.x -= glstate_matrix_projection[0][2];
            
            float dis = distance(center, (0.5).xx);
            float scale = 1.0 - dis * 0.5;
            float intensity = _Vignette_Settings.x * saturate(scale);
            
            half2 d = abs(uv - center) * intensity;
            d.x *= lerp(1.0, _ScreenParams.x / _ScreenParams.y, _Vignette_Settings.w);
            d = pow(saturate(d), _Vignette_Settings.z); // Roundness
            half vfactor = pow(saturate(1.0 - dot(d, d)), _Vignette_Settings.y);
            color.rgb *= lerp(_Vignette_Color, (1.0).xxx, vfactor);
            color.a = lerp(1.0, color.a, vfactor);

            half4 output = color;

            // Output RGB is still HDR at that point (unless range was crunched by a tonemapper)
            return output;
        }

    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM
                #pragma exclude_renderers gles vulkan

                #pragma multi_compile __ COLOR_GRADING_LDR_2D COLOR_GRADING_HDR_2D COLOR_GRADING_HDR_3D
                #pragma multi_compile __ STEREO_INSTANCING_ENABLED STEREO_DOUBLEWIDE_TARGET
            ENDHLSL
        }
    }

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM
                #pragma only_renderers vulkan

                #pragma multi_compile __ COLOR_GRADING_LDR_2D COLOR_GRADING_HDR_2D COLOR_GRADING_HDR_3D
                #pragma multi_compile __ STEREO_DOUBLEWIDE_TARGET // disabled for Vulkan because of shader compiler issues in older Unity versions: STEREO_INSTANCING_ENABLED
            ENDHLSL
        }
    }
    
    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM
                #pragma only_renderers gles

                #pragma multi_compile __ COLOR_GRADING_LDR_2D COLOR_GRADING_HDR_2D // not supported by OpenGL ES 2.0: COLOR_GRADING_HDR_3D
                #pragma multi_compile __ STEREO_DOUBLEWIDE_TARGET // not supported by OpenGL ES 2.0: STEREO_INSTANCING_ENABLED
            ENDHLSL
        }
    }
}