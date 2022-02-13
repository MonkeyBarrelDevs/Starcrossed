Shader "Retro Game Effects/VHS/TV" {
	Properties {
		[HideInInspector]_MainTex ("Main", 2D) = "white" {}
		[NoScaleOffset]_NoiseTex  ("Noise", 2D) = "white" {}
		_Distortion      ("Distortion", Range(0, 1)) = 0.5
		_Fisheye         ("Fisheye", Range(0, 1)) = 0.5
		_StripesStrength ("Stripes", Range(0, 1)) = 0.5
		_NoiseStrength   ("Noise", Range(0, 1)) = 0.5
		_Vignette        ("Vignette", Range(0, 4)) = 3
	}
	SubShader {
		ZTest Always Cull Off ZWrite Off Fog { Mode Off }
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex, _NoiseTex;
			float _Distortion, _Fisheye, _StripesStrength, _NoiseStrength, _Vignette;

			float noise (float2 p)
			{
				float2 ss = 65 * float2(1, 2 * cos(4 * _Time.y));
				float nis = tex2D(_NoiseTex, 65 * float2(1, 2 * cos(4 * _Time.y)) + float2(p.x, p.y * 0.25)).x;
				nis *= nis;
				return nis;
			}
			float onOff (float a, float b, float c)
			{
				float t = _Time.x * 16;
				return step(c, sin(t + a * cos(t * b)));
			}
			float ramp (float y, float start, float end)
			{
				float inside = step(start, y) - step(end, y);
				float fact = (y - start) / (end - start) * inside;
				return (1 - fact) * inside;
			}
			float stripes (float2 uv)
			{
				float t = _Time.x * 24;
				float noi = noise(uv * float2(0.5, 1) + float2(1, 3));
				return ramp(fmod(uv.y * 4 + t / 2 + sin(t + sin(t * 0.63)), 1), 0.5, 0.6) * noi;
			}
			float3 getVideo (float2 uv)
			{
				float2 olduv = uv;

				float t = _Time * 16;
				float2 look = uv;
				float window = 1 / (1 + 20 * (look.y - fmod(t / 4, 1)) * (look.y - fmod(t / 4, 1)));
				look.x = look.x + sin(look.y * 10 + t) / 50 * onOff(4, 4, 0.3) * (1 + cos(t * 80)) * window;
				float vShift = onOff(2.0, 3.0, 0.9) * (sin(t) * sin(t * 20.0) + (0.5 + 0.1 * sin(t * 200.0) * cos(t)));
				look.y = fmod(look.y + vShift, 1.0);

				look = lerp(olduv, look, _Distortion);
				return float3(tex2D(_MainTex, look).rgb);
			}
			float2 fishEyeDistort (float2 uv)
			{
				float2 newuv = uv;
				newuv -= 0.5;
				newuv = newuv * 1.2 * (1.0/1.2 + 2.0*newuv.x*newuv.x*newuv.y*newuv.y);
				newuv += 0.5;
				newuv = lerp(uv, newuv, _Fisheye);
				return newuv;
			}
			half4 frag (v2f_img input) : SV_Target
			{
				float2 uv = fishEyeDistort(input.uv);
				float3 video = getVideo(uv) * 1.5;
				float vigAmt = _Vignette;
				float vignette = (1 - vigAmt * (uv.y - 0.5) * (uv.y - 0.5)) * (1 - vigAmt * (uv.x - 0.5) * (uv.x - 0.5));
				video += _StripesStrength * stripes(uv);
				video += _NoiseStrength * noise(uv * 2) / 2;
				video *= vignette;
				video *= (12 + fmod(uv.y * 30 + _Time, 1))/13;
				return fixed4(video, 1);
			}
			ENDCG
		}
	}
	Fallback Off
}
