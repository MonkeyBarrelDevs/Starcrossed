#ifndef PSX_INCLUDE
#define PSX_INCLUDE

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/BSDF.hlsl"

struct Attributes
{
	float4 positionOS   : POSITION;
	float3 normalOS     : NORMAL;
	float4 tangentOS    : TANGENT;
	float2 uv           : TEXCOORD0;
#if LIGHTMAP_ON
	float2 uvLightmap   : TEXCOORD1;
#endif
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
	float2 uv           : TEXCOORD0;
#if LIGHTMAP_ON
	float2 uvLightmap   : TEXCOORD1;
#endif
	float3 positionWS   : TEXCOORD2;
	half3  normalWS     : TEXCOORD3;
#ifdef _NORMALMAP
	half4 tangentWS     : TEXCOORD4;
#endif
	float4 positionCS   : SV_POSITION;
};

struct CustomSurfaceData
{
	half3 diffuse;           // diffuse color. should be black for metals.
	half3 normalWS;          // normal in world space
	half3 emission;          // emissive color
	half  alpha;             // 0 for transparent materials, 1.0 for opaque.
	float3 positionWS;       // world position
};

struct LightingData
{
	Light light;
	half3 environmentLighting;
	half3 normalWS;
	half NdotL;
};

void SurfaceFunction (Varyings IN, out CustomSurfaceData surfaceData);
half4 LightingFunction (CustomSurfaceData surfaceData, LightingData lightingData);

half _VertexSnappingX, _VertexSnappingY;

Varyings SurfaceVertex (Attributes IN)
{
	VertexPositionInputs vertexInput = GetVertexPositionInputs(IN.positionOS.xyz);
	VertexNormalInputs vertexNormalInput = GetVertexNormalInputs(IN.normalOS, IN.tangentOS);

	// vertex snapping
	float4 clipPos = vertexInput.positionCS;
	float4 vertex = clipPos;
	vertex.xyz = clipPos.xyz / clipPos.w;
	vertex.x = floor(_VertexSnappingX * vertex.x) / _VertexSnappingX;
	vertex.y = floor(_VertexSnappingY * vertex.y) / _VertexSnappingY;
	vertex.xyz *= clipPos.w;
	vertexInput.positionCS = vertex;

	float2 uv = IN.uv;
#ifdef AFFINE_MAPPING   // affine texture mapping
	float distance = length(vertexInput.positionVS);
	uv *= distance + (vertex.w * 8.0) / distance / 2.0;
#endif

	Varyings OUT;
	OUT.uv = uv;
#if LIGHTMAP_ON
	OUT.uvLightmap = IN.uvLightmap.xy * unity_LightmapST.xy + unity_LightmapST.zw;
#endif
	OUT.positionWS = vertexInput.positionWS;
	OUT.normalWS = vertexNormalInput.normalWS;
#ifdef _NORMALMAP
	OUT.tangentWS = float4(vertexNormalInput.tangentWS, IN.tangentOS.w * GetOddNegativeScale());
#endif
	OUT.positionCS = vertexInput.positionCS;
	return OUT;
}

half4 SurfaceFragment (Varyings IN) : SV_Target
{
	CustomSurfaceData surfaceData;
	SurfaceFunction(IN, surfaceData);

	// shadowCoord is position in shadow light space
	float4 shadowCoord = TransformWorldToShadowCoord(IN.positionWS);
	Light light = GetMainLight(shadowCoord);

	LightingData lightingData;
	lightingData.light = light;
	lightingData.environmentLighting = SAMPLE_GI(IN.uvLightmap, SampleSH(surfaceData.normalWS), surfaceData.normalWS);
	lightingData.normalWS = surfaceData.normalWS;
	lightingData.NdotL = saturate(dot(surfaceData.normalWS, lightingData.light.direction));
//	lightingData.NdotL = lightingData.NdotL * 0.5 + 0.5;
	return LightingFunction(surfaceData, lightingData);
}

#endif