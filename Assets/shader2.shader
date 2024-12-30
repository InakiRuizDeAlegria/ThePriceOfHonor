Shader "Custom/OverlayLitShaderWithDistinction"
{
    Properties
    {
        _BaseMap("Base Map", 2D) = "white" {}
        _BaseColor("Base Color", Color) = (1,1,1,1)
        _InsideColor("Inside Color", Color) = (0.5,0.5,0.5,0.5) // Semi-transparent inside color
        _Cutoff("Alpha Cutoff", Range(0, 1)) = 0.5
    }

    SubShader
    {
        Tags { "Queue" = "Overlay+100" "RenderType" = "Transparent" }
        Pass
        {
            Name "ForwardLit"

            Tags { "LightMode" = "UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha // Transparent blending
            ZWrite Off                    // Do not write to the depth buffer
            ZTest Always                   // Always render this material on top

            Cull Off                       // Render both front and back faces

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // Include URP's lighting functions
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float3 viewDirWS : TEXCOORD2;
            };

            sampler2D _BaseMap;
            float4 _BaseColor;
            float4 _InsideColor;

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS);
                output.uv = input.uv;

                float3 normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.normalWS = normalWS;

                float3 viewDirWS = GetWorldSpaceViewDir(input.positionOS);
                output.viewDirWS = viewDirWS;

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                // Determine if we're rendering the front or back face
                float facing = dot(input.normalWS, input.viewDirWS) > 0 ? 1.0 : 0.0;

                // Blend colors based on whether it's front-facing or back-facing
                half4 baseColor = tex2D(_BaseMap, input.uv) * _BaseColor;
                half4 insideColor = tex2D(_BaseMap, input.uv) * _InsideColor;

                return lerp(insideColor, baseColor, facing);
            }
            ENDHLSL
        }
    }

    Fallback "Hidden/InternalErrorShader"
}