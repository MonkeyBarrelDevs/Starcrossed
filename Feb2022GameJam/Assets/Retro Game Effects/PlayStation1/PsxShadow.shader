Shader "Retro Game Effects/Psx/Shadow" {
	Properties {
		[Header(Surface)]
		_BaseColor        ("Base Color", Color) = (1, 1, 1, 1)
		_BaseMap          ("Base Map", 2D) = "white" {}
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend  ("Blend Src", Float) = 1.0
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend  ("Blend Dst", Float) = 0.0
		[Header(Psx)]
		_VertexSnappingX  ("Vertex Snapping X", Float) = 160
		_VertexSnappingY  ("Vertex Snapping Y", Float) = 120
		_PixelationWidth  ("Pixelation Width", Float) = 16
		_PixelationHeight ("Pixelation Height", Float) = 16
		_ColorPrecision   ("Color Precision", Float) = 8
	}
	SubShader {
		Tags { "RenderPipeline" = "UniversalRenderPipeline" "IgnoreProjector" = "True" "Queue" = "Transparent" }

		HLSLINCLUDE
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

		CBUFFER_START(UnityPerMaterial)
		float4 _BaseMap_ST;
		half4 _BaseColor;
		half _PixelationWidth, _PixelationHeight, _ColorPrecision;
		CBUFFER_END
		ENDHLSL

		Pass {
			Tags { "LightMode" = "UniversalForward" }
			Blend [_SrcBlend] [_DstBlend]

			HLSLPROGRAM
			#pragma vertex SurfaceVertex
			#pragma fragment SurfaceFragment
			#include "Psx.hlsl"

			// -------------------------------------
			// Universal Render Pipeline keywords
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT

			// -------------------------------------
			// Textures are declared in global scope
			TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap);

			void SurfaceFunction (Varyings IN, out CustomSurfaceData surfaceData)
			{
				surfaceData = (CustomSurfaceData)0;
				float2 uv = TRANSFORM_TEX(IN.uv, _BaseMap);

				// pixelation
				uv.x = floor(uv.x * _PixelationWidth) / _PixelationWidth;
				uv.y = floor(uv.y * _PixelationHeight) / _PixelationHeight;

				half3 baseColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv).rgb;
				baseColor = floor(baseColor * _ColorPrecision) / _ColorPrecision;   // color precision

				surfaceData.diffuse = baseColor.rgb;
				surfaceData.normalWS = normalize(IN.normalWS);
				surfaceData.emission = 0.0;
				surfaceData.alpha = baseColor.r;
				surfaceData.positionWS = IN.positionWS;
			}
			half4 LightingFunction (CustomSurfaceData surfaceData, LightingData lightingData)
			{
				return half4(surfaceData.diffuse * _BaseColor.rgb, surfaceData.alpha);
			}
			ENDHLSL
		}
		UsePass "Universal Render Pipeline/Lit/ShadowCaster"
		UsePass "Universal Render Pipeline/Lit/DepthOnly"
		UsePass "Universal Render Pipeline/Lit/Meta"
	}
}