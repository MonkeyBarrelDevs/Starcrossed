Shader "Retro Game Effects/CRT/FinalPostprocess" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
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
			float _VignetteStr, _VignetteSize;
			float _CrtBendX, _CrtBendY, _CrtOverscan;

			half4 alphaBlend (half4 top, half4 bottom)
			{
				half4 result;
				result.a   = top.a + bottom.a * (1 - top.a);
				result.rgb = (top.rgb * top.aaa + bottom.rgb * bottom.aaa * (1 - top.aaa)) / result.aaa;
				return result;
			}
			half3 vignette (float2 uv)
			{
				half outer = 1.0;
				half inner = _VignetteSize;
				half2 center = 0.5;
				float dist = distance(center, uv) * 1.414213; // multiplyed by 1.414213 to fit in the range of 0 to 1
				float vig = clamp((outer - dist) / (outer - inner), 0, 1);
				return vig.xxx;
			}
			float2 crt (float2 coord, float bendX, float bendY)
			{
				// to symmetrical coords
				coord = (coord - 0.5) * 2 / (_CrtOverscan + 1);

				// bend
				coord.x *= 1 + pow((abs(coord.y) / bendX), 3);
				coord.y *= 1 + pow((abs(coord.x) / bendY), 3);

				// transform back to 0~1
				coord = (coord / 2) + 0.5;
				return coord;
			}
			half4 frag (v2f_img i) : SV_Target
			{
				float2 crtUv = crt(i.uv, _CrtBendX, _CrtBendY);
				if (crtUv.x < 0 || crtUv.x > 1 || crtUv.y < 0 || crtUv.y > 1)
				{
					return half4(0, 0, 0, 1);
				}
				else
				{
					half4 final = tex2D(_MainTex, crtUv);
					half3 tmp = final.rgb * vignette(crtUv);
					final.rgb = alphaBlend(half4(tmp, _VignetteStr), final).rgb;
					return final;
				}
			}
			ENDCG
		}
	}
	Fallback Off
}