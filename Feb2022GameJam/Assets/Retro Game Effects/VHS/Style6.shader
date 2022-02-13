Shader "Retro Game Effects/VHS/Style6" {
	Properties {
		[HideInInspector]_MainTex ("Main", 2D) = "white" {}
		_OffsetIntensity ("Offset Intensity", Range(0, 0.1)) = 0.02
		_ColorOffset ("Color Offset", Range(0, 3)) = 1.5
		_NoiseIntensity ("Noise Intensity", Range(0, 0.02)) = 0.0088
	}
	SubShader {
		ZTest Always Cull Off ZWrite Off Fog { Mode Off }
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			half _OffsetIntensity, _ColorOffset, _NoiseIntensity;

			float mod (float x, float y) { return x - y * floor(x / y); }
			float random (float2 v) { return frac(sin(dot(v, float2(12.9898, 78.233))) * 43758.5453); }
			float verticalBar (float pos, float uvY, float offset)
			{
				const float range = 0.05;
				float edge0 = (pos - range);
				float edge1 = (pos + range);
				float f = smoothstep(edge0, pos, uvY) * offset;
				f -= smoothstep(pos, edge1, uvY) * offset;
				return f;
			}
			half4 frag (v2f_img input) : SV_Target
			{
				const float noiseQuality = 250.0;
				float2 uv = input.uv;

				for (float i = 0.0; i < 0.71; i += 0.1313)
				{
					float d = mod(_Time.y * i, 1.7);
					float o = sin(1.0 - tan(_Time.y * 0.24 * i));
					o *= _OffsetIntensity;
					uv.x += verticalBar(d, uv.y, o);
				}

				float uvY = uv.y;
				uvY *= noiseQuality;
				uvY = float(int(uvY)) * (1.0 / noiseQuality);
				float nis = random(float2(_Time.y * 0.00001, uvY));
				uv.x += nis * _NoiseIntensity;

				float2 offsetR = float2(0.006 * sin(_Time.y), 0.0) * _ColorOffset;
				float2 offsetG = float2(0.0073 * (cos(_Time.y * 0.97)), 0.0) * _ColorOffset;
				half r = tex2D(_MainTex, uv + offsetR).r;
				half g = tex2D(_MainTex, uv + offsetG).g;
				half b = tex2D(_MainTex, uv).b;
				return half4(r, g, b, 1.0);
			}
			ENDCG
		}
	}
	Fallback Off
}
