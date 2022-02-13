Shader "Retro Game Effects/Dither" {
	Properties {
		[HideInInspector]_MainTex ("Main", 2D) = "white" {}
	}
	SubShader {
		ZTest Always Cull Off ZWrite Off Fog { Mode Off }
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma multi_compile _ RGE_Palette
			#include "UnityCG.cginc"

			#define MAX_PALETTE_SIZE 32

			sampler2D _MainTex, _PatternTex;
			float _Pixelate, _Quantize;
			int _PaletteSize;
			float4 _Palette[MAX_PALETTE_SIZE];

			float bayer8x8 (float2 uv)
			{
				return tex2D(_PatternTex, uv / (_Pixelate * 8)).r;
			}
			float3 closestColor (float3 c)
			{
				float3 old = 100.0 * 255.0;
				for (int i = 0; i < _PaletteSize; i++)
				{
					old = lerp(_Palette[i].rgb, old, step(length(old - c), length(_Palette[i].rgb - c)));
				}
				return old;
			}
			half4 frag (v2f_img input) : SV_Target
			{
				float2 uv = input.uv;
				uv.x = floor((uv.x * _ScreenParams.x) / _Pixelate) * (_Pixelate / _ScreenParams.x);
				uv.y = floor((uv.y * _ScreenParams.y) / _Pixelate) * (_Pixelate / _ScreenParams.y);

				half4 c = tex2D(_MainTex, uv);
				c += float4((bayer8x8(input.uv * _ScreenParams) - 0.5) * _Quantize.xxx, 1);
#if RGE_Palette
				c = float4(closestColor(c.rgb), 1);
#endif
				return c;
			}
			ENDCG
		}
	}
	Fallback Off
}
