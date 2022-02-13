Shader "Retro Game Effects/CRT/Postprocess" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
//		_BlurTex ("Texture", 2D) = "white" {}   // bullshit... if you use 'SetGlobalTexture' never ever write 'Properties'...
	}
	SubShader {
		Cull Off ZWrite Off ZTest Always
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_TexelSize;
			sampler2D _BlurTex;
			float4 _BlurTex_TexelSize;

			float _BleedDist, _BleedStr, _BlurStr;
			float _RgbMaskSub, _RgbMaskSep, _RgbMaskStr;
			int _ColorNoiseMode;
			float _ColorNoiseStr;
			int _MonoNoiseMode;
			float _MonoNoiseStr;
			float4x4 _ColorMat;
			half3 _MinLevels, _MaxLevels, _BlackPoint, _WhitePoint;
			float _InterWidth, _InterSpeed, _InterStr, _InterSplit;
			float _AberStr;

			half4 alphaBlend (half4 top, half4 bottom)
			{
				half4 result;
				result.a   = top.a + bottom.a * (1.0 - top.a);
				result.rgb = (top.rgb * top.aaa + bottom.rgb * bottom.aaa * (half3(1.0, 1.0, 1.0) - top.aaa)) / result.aaa;
				return result;
			}
			half4 blur (float2 uv)
			{
				if (_AberStr == 0.0)
				{
					return tex2D(_BlurTex, uv);
				}
				else
				{
					float dtw = _MainTex_TexelSize.x;
					return half4(
						tex2D(_BlurTex, uv + half2(-dtw * _AberStr, 0.0)).r,
						tex2D(_BlurTex, uv).g,
						tex2D(_BlurTex, uv + half2(dtw * _AberStr, 0.0)).b,
						1.0);
				}
			}
			half4 bleed (float2 uv)
			{
				float dtw = _MainTex_TexelSize.x;
				float dth = _MainTex_TexelSize.y;
				half4 a = blur(uv + half2(0, _BleedDist * dth));
				half4 b = blur(uv + half2(0, -_BleedDist * dth));
				half4 c = blur(uv + half2(_BleedDist * dtw, 0));
				half4 d = blur(uv + half2(-_BleedDist * dtw, 0));
				return max(max(a, b), max(c, d));
			}
			half noise (float n)
			{
				return frac(cos(n * 89.42) * 343.42);
			}
			half3 interference (float2 coord, half3 screen)
			{
				float dth = _MainTex_TexelSize.y;
				screen.r += sin((_InterSplit * dth + coord.y / (_InterWidth * dth) + (_Time.y * _InterSpeed))) * _InterStr;
				screen.g += sin((coord.y / (_InterWidth * dth) + (_Time.y * _InterSpeed))) * _InterStr;
				screen.b += sin((-_InterSplit + coord.y / (_InterWidth * dth) + (_Time.y * _InterSpeed))) * _InterStr;
				screen = saturate(screen);
				return screen;
			}
			half4 frag (v2f_img i) : SV_Target
			{
				half4 base    = tex2D(_MainTex, i.uv);
				half4 blured  = blur(i.uv);
				half4 bleeded = bleed(i.uv);
				half4 final;

				float dtw = _MainTex_TexelSize.x;
				float dth = _MainTex_TexelSize.y;

				final.a = 1;
				half3 tmp;

				// 1. mix tmp with blured in lighten mode
				tmp = max(base.rgb, blured.rgb);
				final.rgb = alphaBlend(half4(tmp, _BlurStr), blured).rgb;

				// 2. mix bleeded with base in lighten mode
				tmp = max(bleeded.rgb, final.rgb);
				final.rgb = alphaBlend(half4(tmp, _BleedStr), final).rgb;

				float delta = fmod(_Time.y, 60);

				// 3. add color noise
				half3 colorNoise = half3(
					noise(sin(i.uv.x / dtw) * i.uv.y / dth + delta), 
					noise(sin(i.uv.y / dth) * i.uv.x / dtw + delta),
					noise(sin(i.uv.x / dtw) * sin(i.uv.y / dth) + delta));

				if (_ColorNoiseMode == 0)
					tmp = final.rgb + colorNoise;
				else if (_ColorNoiseMode == 1)
					tmp = max(colorNoise, final.rgb);

				tmp = saturate(tmp);
				final.rgb = alphaBlend(half4(tmp, _ColorNoiseStr), final).rgb;

				// 4. add monochromatic noise
				float monoNoise = noise(sin(i.uv.x / dtw) * i.uv.y / dth + delta);

				if (_MonoNoiseMode == 0)
					tmp = final.rgb + monoNoise;
				else if (_MonoNoiseMode == 1)
					tmp = max(monoNoise, final.rgb);
				else if (_MonoNoiseMode == 2)
					tmp = min(monoNoise, final.rgb);

				tmp = saturate(tmp);
				final.rgb = alphaBlend(half4(tmp, _MonoNoiseStr), final).rgb;

				// 5. mix rgb mask with final
				float modulo = floor(fmod(i.uv.x / dtw, 3));
				tmp = final.rgb;

				if (modulo == 0)
					tmp -= half3(0, _RgbMaskSub * _RgbMaskSep, _RgbMaskSub * _RgbMaskSep * 2);
				else if (modulo == 1)
					tmp -= half3(_RgbMaskSub * _RgbMaskSep, 0, _RgbMaskSub * _RgbMaskSep);
				else
					tmp -= half3(_RgbMaskSub * _RgbMaskSep * 2, _RgbMaskSub * _RgbMaskSep, 0);

				final.rgb = alphaBlend(half4(tmp, _RgbMaskStr), final).rgb;

				// 6. interference
				final.rgb = interference(i.uv, final.rgb);

				// 7. color adjustment
				final = mul(_ColorMat, final);

				// 8. levels adjustment
				final.rgb = lerp(0, 1, final.rgb / (_MaxLevels - _MinLevels) + _MinLevels);
				final.rgb = clamp(final.rgb, _BlackPoint, _WhitePoint);
				return final;
			}
			ENDCG
		}
	}
	Fallback Off
}