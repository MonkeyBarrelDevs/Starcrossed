Shader "Retro Game Effects/Psx/Lit" {
	Properties {
		[Header(Surface)]
		_BaseColor        ("Base Color", Color) = (1, 1, 1, 1)
		_BaseMap          ("Base Map", 2D) = "white" {}
		[HDR]_Emission    ("Emission Color", Color) = (0, 0, 0, 1)
		[Header(Psx)]
		_VertexSnappingX  ("Vertex Snapping X", Float) = 160
		_VertexSnappingY  ("Vertex Snapping Y", Float) = 120
		_PixelationWidth  ("Pixelation Width", Float) = 16
		_PixelationHeight ("Pixelation Height", Float) = 16
		_ColorPrecision   ("Color Precision", Float) = 8
	}
	SubShader {
		Tags { "RenderPipeline" = "UniversalRenderPipeline" "IgnoreProjector" = "True" }

		HLSLINCLUDE
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

		CBUFFER_START(UnityPerMaterial)
		float4 _BaseMap_ST;
		half4 _BaseColor;
		half _PixelationWidth, _PixelationHeight, _ColorPrecision;
		half4 _Emission;
		CBUFFER_END
		ENDHLSL

		Pass {
			Tags { "LightMode" = "UniversalForward" }

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

				half3 baseColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv).rgb * _BaseColor.rgb;
				baseColor = floor(baseColor * _ColorPrecision) / _ColorPrecision;   // color precision

				surfaceData.diffuse = baseColor.rgb;
				surfaceData.normalWS = normalize(IN.normalWS);
				surfaceData.emission = _Emission.rgb;
				surfaceData.alpha = 1.0;
				surfaceData.positionWS = IN.positionWS;
			}
			half4 LightingFunction (CustomSurfaceData surfaceData, LightingData lightingData)
			{
				half3 c = (surfaceData.diffuse + lightingData.environmentLighting + surfaceData.emission) * lightingData.light.color;
				return half4(c * lightingData.NdotL * lightingData.light.shadowAttenuation, surfaceData.alpha);
			}
			ENDHLSL
		}
		UsePass "Universal Render Pipeline/Lit/ShadowCaster"
		UsePass "Universal Render Pipeline/Lit/DepthOnly"
		UsePass "Universal Render Pipeline/Lit/Meta"
	}
}