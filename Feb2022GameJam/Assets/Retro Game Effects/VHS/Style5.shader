Shader "Retro Game Effects/VHS/Style5" {
	Properties {
		[HideInInspector]_MainTex ("Main", 2D) = "white" {}
		_NoiseTex ("Noise", 2D) = "black" {}
		_Wobble   ("Wobble", Range(0.001, 0.02)) = 0.002
		_Grade    ("Grade", Range(0, 1)) = 0.5
		_Scanline ("Scanline", Range(0, 0.1)) = 0.015
		_Vignette ("Vignette", Range(0, 1)) = 0.2
	}
	SubShader {
		ZTest Always Cull Off ZWrite Off Fog { Mode Off }
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex, _NoiseTex;
			float4 _MainTex_TexelSize, _NoiseTex_TexelSize;
			half _Wobble, _Vignette, _Grade, _Scanline;

			float2 mod (float2 x, float2 y) { return x - y * floor(x / y); }
			float random (float2 v) { return frac(sin(dot(v, float2(12.9898, 78.233))) * 43758.5453); }
			float sampleNoise (float2 fragCoord)
			{
				float2 uv = mod(fragCoord + float2(0, 100 * _Time.y), _NoiseTex_TexelSize.zw * 6.0);
				float value = tex2D(_NoiseTex, uv / _MainTex_TexelSize.zw).r;
				return pow(value, 7.0);
			}
			half4 frag (v2f_img input) : SV_Target
			{
				const float PI = 3.14159265;
				const float line_intensity = 2.0;

				float2 uv = input.uv;
				float2 fragCoord = input.uv * _MainTex_TexelSize.zw;
				float rnd = random(float2(_Time.y, fragCoord.y));

				// wobble
				float2 wobble = float2(_Wobble * rnd, 0.0);

				// band distortion
				float t = tan(0.25 * _Time.y + uv.y * PI * 0.67);
				float2 tan_off = float2(wobble.x * min(0.0, t), 0.0);

				// chromab
				float4 c1 = tex2D(_MainTex, uv + wobble + tan_off);
				float4 c2 = tex2D(_MainTex, (uv + (wobble * 1.5) + (tan_off * 1.3)) * 1.005);
				float4 color = float4(c2.rg, pow(c1.b, 0.67), 1.0);  // combine
				color.rgb = lerp(tex2D(_MainTex, uv + tan_off).rgb, color.rgb, _Grade);  // grade

				// scanline sim
				color += ((sin(2.0 * PI * uv.y + _Time.y * 20.0) + sin(2.0 * PI * uv.y)) / 2.0) * _Scanline * sin(_Time.y);

				// noise lines
				float ival = _MainTex_TexelSize.w / 4.0;
				float on = floor(float(int(fragCoord.y + (_Time.y * rnd * 1000.0)) % uint(ival + line_intensity)) / ival);
				float wh = sampleNoise(fragCoord) * on;   // hack to avoid conditional
				color = float4(min(1.0, color.r + wh), min(1.0, color.g + wh), min(1.0, color.b + wh), 1.0);

				// vignette
				float vig = 1.0 - sin(PI * uv.x) * sin(PI * uv.y);
				color -= (vig * _Vignette);
				return color;
			}
			ENDCG
		}
	}
	Fallback Off
}
