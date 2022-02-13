Shader "Retro Game Effects/VHS/Style3" {
	Properties {
		[HideInInspector]_MainTex ("Main", 2D) = "white" {}
		_NoiseIntensity   ("Noise Intensity", Range(0, 1)) = 0.2
		_Brightness       ("Brightness", Range(-1, 1)) = 0.3
		_Contrast         ("Contrast", Range(0, 1.5)) = 1
		[NoScaleOffset]_NoiseTex ("Noise", 2D) = "black" {}
	}
	SubShader {
		ZTest Always Cull Off ZWrite Off Fog { Mode Off }
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex, _NoiseTex;
			float _NoiseIntensity, _Contrast, _Brightness;

			float3 YUV2RGB (float3 c)
			{
				float r = c.r + 1.140 * c.b;
				float g = c.r - 0.395 * c.g - 0.581 * c.b;
				float b = c.r + 2.032 * c.g;
				return float3(r, g, b);
			}
			float3 RGB2YUV (float3 c)
			{
				float y = 0.299 * c.r + 0.587 * c.g + 0.114 * c.b;
				float u =-0.147 * c.r - 0.289 * c.g + 0.436 * c.b;
				float v = 0.615 * c.r - 0.515 * c.g - 0.100 * c.b;
				return float3(y, u, v);
			}
			float hardLight (float s, float d) { return (s < 0.5) ? 2.0 * s * d : 1.0 - 2.0 * (1.0 - s) * (1.0 - d); }
			float3 hardLight (float3 s, float3 d)
			{
				float3 c;
				c.x = hardLight(s.x, d.x);
				c.y = hardLight(s.y, d.y);
				c.z = hardLight(s.z, d.z);
				return c;
			}
			float mod (float x, float y) { return x - y * floor(x / y); }
			float random (float2 v)
			{
				float sn = mod(dot(v, float2(12.9898, 78.233)), 3.14);
				return frac(sin(sn) * 43758.5453);
			}
			half4 frag (v2f_img input) : SV_Target
			{
				half t = _Time.x;
				float2 uvst = input.uv;
				float2 uv = uvst;
				if (uv.y < 0.025) uv.x += (uv.y - 0.05) * (sin(uv.y * 512 + t * 12));
				if (uv.y < 0.015) uv.x += (uv.y - 0.05) * (sin(uv.y * 512 + t * 64));

				float uvy = floor(uv.y * 288) / 288;
				float uvx = random(float2(t * 0.013, uvy * 0.42)) * 0.004;
				uvx += sin(random(float2(t * 0.4, uvy))) * 0.005;
				uv.x += (uvx * (1 - uv.x));
				float3 col = tex2D(_MainTex, uv).rgb;
				col = clamp(col, 0.08, 0.95);
				float3 yuv = RGB2YUV(col);
				float s = sin(t * 128) / 128;
				float c = cos(t * 128) / 128;
				uv.x = floor(uv.x * 52) / 52;
				uv.y = floor(uv.y * 288) / 288;
				col = tex2D(_MainTex, uv + float2(-0.01 + s, c) * s).rgb;
				float3 yuv2 = RGB2YUV(col) / (_Contrast + 1);
				float wave = max(sin(uv.y * 24 + t * 64), 0);
				wave += max(sin(uv.y * 14.0 + t * 16.0), 0.0);
				wave /= 2.0;
				col = YUV2RGB(float3(yuv.r, yuv2.g * (wave + 0.5), yuv2.b * (wave + 0.5)));
				col = clamp(col, 0.08, 0.95);
				col *= 1.05;
				uv = uvst.xy / 8.0;
				float tm = t * 30;
				uv.x += floor(fmod(tm, 1.0) * 8.0) / 8.0;
				uv.y += (1.0 - floor(fmod(tm / 8.0, 1.0) * 8.0) / 8.0);

				col = hardLight(col, _Brightness.xxx);
				uv = uvst.xy / 8.0;
				uv.y = 1.0 - uv.y;

				uv.x += floor(fmod(tm, 1.0) * 8.0) / 8.0;
				uv.y += (1.0 - floor(fmod(tm / 8.0, 1.0) * 8.0) / 8.0);

				float4 t2 = tex2D(_NoiseTex, uv * 20.0);
				col = lerp(col, col + t2, _NoiseIntensity * (1.0 - uvst.y));
				return float4(col, 1.0);
			}
			ENDCG
		}
	}
	Fallback Off
}
