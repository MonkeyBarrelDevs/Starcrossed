Shader "Retro Game Effects/VHS/Style4" {
	Properties {
		[HideInInspector]_MainTex ("Main", 2D) = "white" {}
		_Distortion ("Distortion", Range(1, 10)) = 1
		_Speed ("Speed", Range(0, 1)) = 0.1
	}
	SubShader {
		ZTest Always Cull Off ZWrite Off Fog { Mode Off }
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			half _Distortion, _Speed;

			half4 frag (v2f_img input) : SV_Target
			{
				float2 uv = 0.5 + (input.uv - 0.5) * (0.9 + 0.1 * sin(_Speed * _Time.y));
				half3 c = 0.0;
				c.r = tex2D(_MainTex, float2(uv.x + 0.003 * _Distortion, uv.y)).r;
				c.g = tex2D(_MainTex, float2(uv.x + 0.000 * _Distortion, uv.y)).g;
				c.b = tex2D(_MainTex, float2(uv.x - 0.003 * _Distortion, uv.y)).b;
				c = saturate(c * 0.5 + 0.5 * c * c * 1.2);
				c *= 0.5 + 8.0 * uv.x * uv.y * (1.0 - uv.x) * (1.0 - uv.y);
				c *= half3(0.95, 1.05, 0.95);
				c *= 0.9 + 0.1 * sin(10.0 * _Time.y + uv.y * 1000.0);
				c *= 0.99 + 0.01 * sin(110.0 * _Time.y);
				return half4(c, 1.0);
			}
			ENDCG
		}
	}
	Fallback Off
}
