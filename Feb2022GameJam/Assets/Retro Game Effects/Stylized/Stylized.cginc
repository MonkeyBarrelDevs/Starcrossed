#ifndef STYLIZED
#define STYLIZED

#include "UnityCG.cginc"
#include "Noise.cginc"

sampler2D _MainTex;
float4 _MainTex_TexelSize;
float _Saturation, _Brightness, _Contrast;

// brightness, contrast, saturation modify
float3 ContrastSaturationBrightness (float3 color, float brt, float sat, float con)
{
	float3 LuminaceCoeff = float3(0.2125, 0.7154, 0.0721);

	// brigntess calculations
	float3 brtColor = color * brt;
	float lumn = dot(brtColor, LuminaceCoeff);

	// saturation calculation
	float3 satColor = lerp(lumn.xxx, brtColor, sat);

	// contrast calculations
	float3 c = lerp(0.5, satColor, con);
	return c;
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

half4 _Color;
float _Angle, _Scale;

float pattern (float2 uv)
{
	float s = sin(_Angle);
	float c = cos(_Angle);
	half2 tex = uv * _ScreenParams.xy - 0.5;
	half2 pt = half2(c * tex.x - s * tex.y, s * tex.x + c * tex.y) * _Scale;
	return (sin(pt.x) * cos(pt.y)) * 2.0;
}
float4 fragDot (v2f_img input) : SV_Target
{
	half2 uv = input.uv;

	float4 col = tex2D(_MainTex, uv);
	float average = (col.r + col.g + col.b) / 3.0;

	float dst;
	if (average < 0.001)
		dst = 0.0;
	else if (average > 0.45)
		dst = 1.0;
	else
		dst = average * 10.0 - 2.0 + pattern(input.uv);

	float4 fragColor = _Color;
	if (dst < 1.0)
		dst = 0.2;

	dst = clamp(dst, 0.2, 1.0);

	fragColor.rgb = _Color * dst;
	fragColor.a = dst;
	return fragColor;
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

sampler2D _CameraDepthNormalsTexture;
half4 _Sensitivity;
half _SampleDistance;

inline half CheckSame (half2 centerNormal, float centerDepth, half4 theSample)
{
	half2 diff = abs(centerNormal - theSample.xy) * _Sensitivity.y;
	half isSameNormal = (diff.x + diff.y) * _Sensitivity.y < 0.1;

	float sampleDepth = DecodeFloatRG (theSample.zw);
	float zdiff = abs(centerDepth-sampleDepth);

	half isSameDepth = zdiff * _Sensitivity.x < 0.09 * centerDepth;
	return isSameNormal * isSameDepth;
}
half4 fragOutline (v2f_img input) : SV_Target
{
	half2 uv = input.uv;

	half sampleDist = _SampleDistance*2.4;
	half4 colSample1 = tex2D(_MainTex, uv + float2(0, -_MainTex_TexelSize.y) * sampleDist);
	half4 colSample2 = tex2D(_MainTex, uv + float2(0, +_MainTex_TexelSize.y) * sampleDist );
	half4 colSample3 = tex2D(_MainTex, uv + float2(-_MainTex_TexelSize.x, 0) * sampleDist );
	half4 colSample4 = tex2D(_MainTex, uv + float2(+_MainTex_TexelSize.x, 0) * sampleDist );
	
	half4 center = tex2D (_CameraDepthNormalsTexture, uv);//return center;
	half4 sample1 = tex2D(_CameraDepthNormalsTexture, uv + float2(0, -_MainTex_TexelSize.y) * sampleDist);
	half4 sample2 = tex2D(_CameraDepthNormalsTexture, uv + float2(0, +_MainTex_TexelSize.y) * sampleDist );
	half4 sample3 = tex2D(_CameraDepthNormalsTexture, uv + float2(-_MainTex_TexelSize.x, 0) * sampleDist );
	half4 sample4 = tex2D(_CameraDepthNormalsTexture, uv + float2(+_MainTex_TexelSize.x, 0) * sampleDist );

	half2 nrm = center.xy;   // center normal
	float dpt = DecodeFloatRG(center.zw);   // center depth

	half edge = 1.0;

	half4 lineColor = half4(0., 0., 0., 1.0);

	edge *= (CheckSame(nrm, dpt, sample1) + CheckSame(nrm, dpt, sample2) + CheckSame(nrm, dpt, sample3) + CheckSame(nrm, dpt, sample4)) / 4.0;

	float dst = 1.0 - edge;

	half v1 = (colSample1.r + colSample1.g + colSample1.b) / 3.0f;
	half v2 = (colSample2.r + colSample2.g + colSample2.b) / 3.0f;
	half v3 = (colSample3.r + colSample3.g + colSample3.b) / 3.0f;
	half v4 = (colSample4.r + colSample4.g + colSample4.b) / 3.0f;
	half avg = (v1 + v2 + v3 + v4)/4.0;


	half4 c = lineColor;
	c.a = dst;

	half2 pos = uv * _ScreenParams;
	float nz = snoise(half2(pos.x * pos.y, pos.x + pos.y));
	c.a *= 1.0 - dpt * 2.0;
	c.a *= abs(nz) * 2.0;
//return c.aaaa;
	return c;
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

half _Step;

float stepColor (float c) { return floor(saturate(c) * (_Step * 2) / 2) / (_Step * 2) *2; }
float4 fragNoiseDraw (v2f_img input) : SV_Target
{
	half2 uv = input.uv;
	half2 pos = uv * _ScreenParams;

	float4 c = tex2D(_MainTex, uv);
	float average = (c.r + c.g + c.b) / 3.0;
	float nz = snoise(half2(pos.x*pos.y + 1.0, pos.x+pos.y-1.0));

	half4 dnPix = tex2D(_CameraDepthNormalsTexture, input.uv);
	float depth = DecodeFloatRG (dnPix.zw);
	if(depth < 0.99)
		average *= depth*4.0 + 0.5;

	float newColor = stepColor( average + (nz*0.15) )+0.2;
	float4 fragColor = float4( newColor, newColor, newColor, c.a );
	return fragColor;
}

#endif