// I have no fucking idea what is happening in this shader but it works so don't touch unless you know what you're doing (that includes you, DaXcess!)

Shader "RepoXR/Dashed Outline Cube"
{
    Properties
    {
        _BaseColor ("Base Color (RGB) & Transparency (A)", Color) = (0.0, 0.5, 1.0, 0.2)

        _EdgeColor ("Edge Dash Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _EdgeWidth ("Edge Width (Local Scale)", Range(0.005, 0.1)) = 0.05
        _DashLength ("Dash Count Per Unit Length", Range(5, 50)) = 15.0
        _DashSpeed ("Dash Speed", Range(0, 5)) = 1.0
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Back

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _BaseColor;
            fixed4 _EdgeColor;
            float _EdgeWidth;
            float _DashLength;
            float _DashSpeed;

            float3 _ObjectScale;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 localPos : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float3 objectScale : TEXCOORD2;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.localPos = v.vertex.xyz;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);

                float3 scale;
                scale.x = length(unity_ObjectToWorld[0].xyz);
                scale.y = length(unity_ObjectToWorld[1].xyz);
                scale.z = length(unity_ObjectToWorld[2].xyz);
                o.objectScale = scale;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 absLocalPos = abs(i.localPos);

                float distX = 0.5 - absLocalPos.x;
                float distY = 0.5 - absLocalPos.y;
                float distZ = 0.5 - absLocalPos.z;

                float2 surfaceDist;

                if (distX < distY && distX < distZ)
                    surfaceDist = float2(distY, distZ);
                else if (distY < distX && distY < distZ)
                    surfaceDist = float2(distX, distZ);
                else
                    surfaceDist = float2(distX, distY);

                float distToEdge = min(surfaceDist.x, surfaceDist.y);
                float faceMask = step(_EdgeWidth, distToEdge);

                if (faceMask < 0.5)
                {
                    float coordAlongEdge;
                    float flowDirection;
                    float scaleFactor;

                    if (absLocalPos.x > absLocalPos.y && absLocalPos.x > absLocalPos.z)
                    {
                        if (distY < distZ)
                        {
                            coordAlongEdge = i.localPos.z;
                            scaleFactor = i.objectScale.z;

                            flowDirection = (i.localPos.y < 0) ? 1.0 : -1.0;
                        }
                        else
                        {
                            coordAlongEdge = i.localPos.y;
                            scaleFactor = i.objectScale.y;

                            flowDirection = (i.localPos.z < 0) ? -1.0 : 1.0;
                        }
                    }
                    else if (absLocalPos.y > absLocalPos.x && absLocalPos.y > absLocalPos.z)
                    {
                        if (distX < distZ)
                        {
                            coordAlongEdge = i.localPos.z;
                            scaleFactor = i.objectScale.z;
                            flowDirection = (i.localPos.x > 0) ? 1.0 : -1.0;
                        }
                        else
                        {
                            coordAlongEdge = i.localPos.x;
                            scaleFactor = i.objectScale.x;
                            flowDirection = (i.localPos.z > 0) ? -1.0 : 1.0;
                        }
                    }
                    else
                    {
                        if (distX < distY)
                        {
                            coordAlongEdge = i.localPos.y;
                            scaleFactor = i.objectScale.y;
                            flowDirection = (i.localPos.x > 0) ? -1.0 : 1.0;
                        }
                        else
                        {
                            coordAlongEdge = i.localPos.x;
                            scaleFactor = i.objectScale.x;
                            flowDirection = (i.localPos.y > 0) ? 1.0 : -1.0;
                        }
                    }

                    float normalizedCoord = coordAlongEdge + 0.5;
                    float scaledCoord = normalizedCoord * scaleFactor;

                    float animatedCoord = scaledCoord * _DashLength * flowDirection + _Time.y * _DashSpeed;

                    float dash_mask = frac(animatedCoord);
                    dash_mask = step(0.6, dash_mask);

                    fixed4 edge_color = _EdgeColor;
                    edge_color.a *= dash_mask;

                    fixed4 final_color = lerp(_BaseColor, edge_color, dash_mask);
                    final_color.a = max(_BaseColor.a, edge_color.a);

                    return final_color;
                }

                return _BaseColor;
            }
            ENDCG
        }
    }
}