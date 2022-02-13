Shader "Retro Game Effects/VHS/Style2" {
	Properties {
		[HideInInspector]_MainTex ("Main", 2D) = "white" {}
		_LensDistortion        ("Lens distortion", Float) = 1.2
		_ChromaticAberration   ("Chromatic aberration", Float) = 0
		_ColorBleedIterations  ("Color bleed iterations", Int) = 10
		_ColorBleedAmount      ("Color bleed amount", Float) = 0
		_LineAmount            ("Line amount", Float) = 1
		_LinesDisplacement     ("Lines displacement", Float) = 0
		_LinesSpeed            ("Lines speed", Float) = 0
		_Contrast              ("Contrast", Range(0, 1)) = 1
		_SineLinesAmount       ("Sine lines amount", Float) = 1
		_SineLinesSpeed        ("Sine lines speed", Float) = 0
		_SineLinesThreshold    ("Sine lines threshold", Range(0, 1)) = 0
		_SineLinesDisplacement ("Sine lines displacement", Float) = 0
		_NoiseTexture          ("Noise", 2D) = "white" {}
	}
	SubShader {
		ZTest Always Cull Off ZWrite Off Fog { Mode Off }
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex, _NoiseTexture;
			float4 _MainTex_TexelSize, _NoiseTexture_ST;
			float _ChromaticAberration;
			float _ColorBleedAmount, _ColorBleedIterations;
			float _LineAmount, _LinesDisplacement;
			float _Contrast, _Vignette, _LensDistortion;
			float _LinesSpeed, _SineLinesAmount, _SineLinesDisplacement, _SineLinesThreshold, _SineLinesSpeed;

			float2 screenDistort (float2 uv)
			{
				uv -= 0.5;
				uv = uv * _LensDistortion * (1.0 / 1.2 + 2.0 * uv.x * uv.x * uv.y * uv.y);
				uv += 0.5;
				return uv;
			}
			float rand (in float2 v) { return frac(sin(dot(v, float2(12.9898, 78.233))) * 43758.5453123); }

			half4 frag (v2f_img input) : SV_Target
			{
				float2 uv = screenDistort(input.uv);
				half colR = 0, colG = 0, colB = 0;
				float offset = 0;

				// solid lines
				float lines = step(0.5, frac(uv.y * _LineAmount + _Time.y * _LinesSpeed)) * 2.0 - 1.0;
				float linesDispl = lines * _LinesDisplacement;

				// offsetting and wrapping the whole screen overtime
				uv.y = frac(uv.y + lerp(0.0, 0.4, frac(_Time.z * 2.0) * step(0.97, rand(floor(_Time.z * 2.0)))));

				// constantly changing random noise values
				float random = rand(uv + _Time.x);
				// sampling the noise texture while also making it move constantly
				float noise = tex2D(_NoiseTexture, uv * _NoiseTexture_ST.xy + rand(_Time.x)).x;

				// getting random values from -1 to 1 every few frames to randomly change the speed and direction of the sine lines
				float sineLinesTime = _Time.y * _SineLinesSpeed * (rand(floor(_Time.y)) * 2.0 - 1.0);
				float sineLines = sin(uv.y * _SineLinesAmount * UNITY_PI * 2.0 + sineLinesTime) * 0.5 + 0.5;
				// lines with a random 0-1 value, to be used as mask
				float randLines = rand(round(uv.y * _SineLinesAmount + sineLinesTime));
				float sineLinesMask = step(randLines, _SineLinesThreshold);
				float sineLinesDispl = sineLines * sineLinesMask * _SineLinesDisplacement;

				// multiple sampling for color bleeding
				for (int k = 0; k < _ColorBleedIterations; k++)
				{
					offset += lerp(0.8, _ColorBleedAmount, sin(_Time.y) * 0.5 + 0.5);
					colR += tex2D(_MainTex, uv + float2(offset + _ChromaticAberration + linesDispl + sineLinesDispl, 0) * _MainTex_TexelSize.xy).r;
					colG += tex2D(_MainTex, uv + float2(offset + _ChromaticAberration - linesDispl + sineLinesDispl, 0) * _MainTex_TexelSize.xy).g;
					colB += tex2D(_MainTex, uv + float2(offset + linesDispl + sineLinesDispl, 0) * _MainTex_TexelSize.xy).b;
				}
				colR /= _ColorBleedIterations;
				colG /= _ColorBleedIterations;
				colB /= _ColorBleedIterations;

				half4 col = half4(colR, colG, colB, 1.0);
				col = lerp(0.5, col, _Contrast);  // reducing contrast
				col *= max(0.7, random);   // grain noise
				col += smoothstep(abs(uv.y * 2.0 - 1.0) - 0.4, abs(uv.y * 2.0 - 1.0) - 0.99, noise);   // top and bottom noise
				col += step(0.99, 1.0 - randLines) * step(sineLines, noise) * 0.2;   // passing lines noise
				return col;
			}
			ENDCG
		}
	}
	Fallback Off
}
