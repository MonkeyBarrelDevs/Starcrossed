Shader "Retro Game Effects/Stylized/Process" {
	Properties {
		[HideInInspector] _MainTex ("Main", 2D) = "white" {}
	}
	SubShader {
		ZTest Always Cull Off ZWrite Off Fog { Mode Off }
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "Stylized.cginc"

			float4 frag (v2f_img input) : SV_Target
			{
				float4 c = tex2D(_MainTex, input.uv);
				c.rgb = ContrastSaturationBrightness(c.rgb, _Brightness, _Saturation, _Contrast);
				return c;
			}
			ENDCG
		}
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment fragDot
			#include "Stylized.cginc"
			ENDCG
		}
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment fragOutline
			#include "Stylized.cginc"
			ENDCG
		}
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment fragNoiseDraw
			#include "Stylized.cginc"
			ENDCG
		}
	}
	Fallback Off
}