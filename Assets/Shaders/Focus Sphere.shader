Shader "RepoXR/Focus Sphere"
{
    Properties
    {
        _MainTex ("Noise Texture", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (0.0, 0.0, 0.0, 1.0)
        _DistortStrength ("Distortion Strength", Range(0, 0.05)) = 0.02
        _FadeStart ("Fade Start", Range(0, 1)) = 0.6
        _FadeEnd ("Fade End", Range(0, 1)) = 0.9
        _Speed ("Animation Speed", Range(0, 2)) = 0.3

        _GlitchFrequency ("Glitch Frequency", Range(1, 20)) = 15
        _GlitchIntensity ("Glitch Intensity", Range(0, 0.5)) = 0.05
        _GlitchBlockSize ("Glitch Block Size", Range(5, 100)) = 10
        _GlitchColorMix ("Glitch Color Mix", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        GrabPass { "_GrabTexture" }

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Front

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _GrabTexture;
            fixed4 _BaseColor;
            float _DistortStrength;
            float _FadeStart;
            float _FadeEnd;
            float _Speed;

            float _GlitchFrequency;
            float _GlitchIntensity;

            float _GlitchBlockSize;
            float _GlitchColorMix;

            float rand (float2 uv, float time)
            {
                return frac(sin(dot(uv.xy, float2(12.9898, 78.233)) + time) * 43758.5453);
            }

            float GlitchTimeStep(float speed)
            {
                float2 noiseCoord = float2(_Time.y * speed * 0.01, 0.5);
                float noiseVal = tex2D(_MainTex, noiseCoord).r;

                return floor(noiseVal * 100.0);
            }

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 localDir : TEXCOORD1;
                float4 grabPos : TEXCOORD2;
                float3 positionOS : TEXCOORD3;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.localDir = normalize(v.vertex.xyz);
                o.grabPos = ComputeGrabScreenPos(o.pos);
                o.positionOS = v.vertex.xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float ndotf = dot(i.localDir, float3(0, 0, 1));
                float fade = smoothstep(_FadeEnd, _FadeStart, ndotf * 0.5 + 0.5);

                if (_FadeEnd == 0.0 && _FadeStart == 0.0)
                    fade = 0;

                float2 noiseUV = i.uv * 3 + _Time.y * _Speed;
                float2 noise = (tex2D(_MainTex, noiseUV).rg - 0.5) * 2.0;

                float2 grabUV = i.grabPos.xy / i.grabPos.w;
                grabUV += noise * _DistortStrength * fade;

                fixed4 sceneColor = tex2D(_GrabTexture, grabUV);
                fixed4 finalColor = lerp(sceneColor, _BaseColor, fade);

                if (fade > 0.99)
                {
                    float intervalTime = GlitchTimeStep(_GlitchFrequency);
                    float2 blockUV = floor(i.uv * rand(_GlitchBlockSize, intervalTime) * 50);
                    float blockRandom = rand(blockUV, intervalTime);

                    if (blockRandom > 0.98)
                    {
                        float randR = rand(blockUV + float2(1, 0), intervalTime);
                        float randG = rand(blockUV + float2(0, 1), intervalTime);
                        float randB = rand(blockUV + float2(1, 1), intervalTime);

                        float3 randomColor = float3(randR, randG, randB) * _GlitchColorMix;
                        finalColor.rgb += randomColor;
                    }
                }

                finalColor.a *= fade + (noise * _DistortStrength * 5) * (1.0 - fade);

                return finalColor;
            }
            ENDCG
        }
    }
}
