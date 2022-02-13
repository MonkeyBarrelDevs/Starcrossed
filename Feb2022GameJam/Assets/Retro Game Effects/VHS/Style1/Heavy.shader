Shader "Retro Game Effects/VHS/Heavy" {
	Properties {
		[HideInInspector]_MainTex  ("Main", 2D) = "white" {}
		[Header(Blur)][Space(5)]
		_Amount    ("Amount", Range(0, 10)) = 1
		_ROffset   ("R Channel Offset", Range(1, 10)) = 4
		[Header(CRT)][Space(5)]
		_PixelSize ("Pixel Size", Range(1, 10)) = 3
		_CRTFade   ("CRT Fade", Range(0, 1)) = 0
		[Header(Glitch)][Space(5)]
		_Layer1Scale      ("Layer1 Scale", Float) = 9
		_Layer2Scale      ("Layer2 Scale", Float) = 5
		_Layer1Intensity  ("Layer1 Intensity", Float) = 8
		_Layer2Intensity  ("Layer2 Intensity", Float) = 4
		_RGBSplit         ("RGBSplit", Float) = 0.5
		_Offset           ("Amount", Float) = 3
		_Fade             ("Fade", Range(0, 1)) = 1
	}
	SubShader {
		ZTest Always Cull Off ZWrite Off Fog { Mode Off }
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_TexelSize;
			float _Amount, _ROffset;

			half4 frag (v2f_img input) : SV_Target
			{
				float blurH = _Amount / _MainTex_TexelSize.w;
				float blurV = _Amount / _MainTex_TexelSize.z;
				half2 offsets[8] = {
					half2(blurH, 0),
					half2(-blurH, 0),
					half2(0, blurV),
					half2(0, -blurV),
					half2(blurH, blurV),
					half2(blurH, -blurV),
					half2(-blurH, blurV),
					half2(-blurH, -blurV),
				};
				half4 c = tex2D(_MainTex, input.uv);
				half4 samples[8];
				half4 samplesRed[8];
				for (int n = 0; n < 8; n++)
				{
					samples[n] = tex2D(_MainTex, input.uv + offsets[n]);
					samplesRed[n] = tex2D(_MainTex, input.uv + offsets[n] + half2(_ROffset / _MainTex_TexelSize.z, 0.0));
					c.r += samplesRed[n].r;
					c.gb += samples[n].gb;
				}
				c /= 9.0;
				return c;
			}
			ENDCG
		}
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float _PixelSize, _CRTFade;

			half4 frag (v2f_img input) : SV_Target
			{
				float2 cor;
				cor.x =  (input.uv.x * _ScreenParams.x) / _PixelSize;
				cor.y = ((input.uv.y * _ScreenParams.y) + _PixelSize * fmod(floor(cor.x), 2)) / (_PixelSize * 3);

				float2 ico = floor(cor);
				float2 fco = frac(cor);

				float3 pix;
				pix.x = step(1.5, fmod(ico.x, 3));
				pix.y = step(1.5, fmod(1 + ico.x, 3));
				pix.z = step(1.5, fmod(2 + ico.x, 3));

				float3 ima = tex2D(_MainTex, _PixelSize * ico * float2(1, 3) / _ScreenParams.xy).xyz;

				fixed3 c = pix * dot(pix, ima);
				c *= step(abs(fco.x - 0.5), 0.4);
				c *= step(abs(fco.y - 0.5), 0.4);
				c = lerp(c, ima, _CRTFade);
				return half4(c, 1);
			}
			ENDCG
		}
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			half _Layer1Scale, _Layer2Scale;
			float _Layer1Intensity, _Layer2Intensity, _RGBSplit, _Offset, _Fade;

			float randomNoise (float2 seed)
			{
				return frac(sin(dot(seed * floor(_Time.y * 30.0), float2(127.1, 311.7))) * 43758.5453123);
			}
			float randomNoise (float seed)
			{
				return randomNoise(float2(seed, 1.0));
			}
			float4 frag (v2f_img input) : SV_Target
			{
				float2 uv = input.uv;
				float2 layer1 = floor(uv * _Layer1Scale);
				float2 layer2 = floor(uv * _Layer2Scale);

				float nis1 = pow(randomNoise(layer1), _Layer1Intensity);
				float nis2 = pow(randomNoise(layer2), _Layer2Intensity);
				float rgbSplitNoise = pow(randomNoise(5.1379), 7.1) * _RGBSplit;
				float nis = nis1 * nis2 * _Offset - rgbSplitNoise;

				float4 cR = tex2D(_MainTex, uv);
				float4 cG = tex2D(_MainTex, uv + float2(nis * 0.05 * randomNoise(7.0), 0));
				float4 cB = tex2D(_MainTex, uv - float2(nis * 0.05 * randomNoise(23.0), 0));

				float4 r = float4(float3(cR.r, cG.g, cB.b), 1.0);
				r = lerp(cR, r, _Fade);
				return r;
			}
			ENDCG
		}
	}
	Fallback Off
}
