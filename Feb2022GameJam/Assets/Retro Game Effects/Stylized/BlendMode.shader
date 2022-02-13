Shader "Retro Game Effects/Stylized/BlendMode" {
	Properties {
		[HideInInspector] _MainTex ("Main", 2D) = "white" {}
	}
	CGINCLUDE
	#include "UnityCG.cginc"

	sampler2D _MainTex, _OverlayTex, _PaperTex;
	half _Intensity;

	half4 fragOverlay (v2f_img input) : SV_Target
	{
		half2 uv = input.uv;
		half4 o = tex2D(_OverlayTex, uv);
		half4 c = tex2D(_MainTex, uv);

		float3 check = step(0.5, c.rgb);

		float3 result =  check * (1.0 - ((1.0 - 2.0 * (c.rgb - 0.5)) * (1.0 - o.rgb)));
		result += (1.0 - check) * (2.0 * c.rgb) * o.rgb;
		return half4(lerp(c.rgb, result.rgb, _Intensity), c.a);
	}
	half4 fragAlphaBlend (v2f_img input) : SV_Target
	{
		half2 uv = input.uv;
		half4 c1 = tex2D(_OverlayTex, uv);
		half4 c2 = tex2D(_MainTex, uv);
		half4 c3 = tex2D(_PaperTex, uv);
		return lerp(c2, c1, c1.a) * c3;
	}
	ENDCG
	Subshader {
		ZTest Always Cull Off ZWrite Off Fog { Mode Off }
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment fragOverlay
			ENDCG
		}
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment fragAlphaBlend
			ENDCG
		}
	}
	Fallback Off
}