using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace RetroGameEffects.CRT
{
	[RequireComponent(typeof(Camera))]
	public class CRTScreen : MonoBehaviour
	{
		public static void SetupMiniCRTPreset(CRTScreen effect) {
			effect.m_BlurSize					= 0.8f;
			effect.m_BlurStrength				= 0.8f;
			effect.m_BleedingSize 				= 0.5f;
			effect.m_BleedingStrength 			= 1f;
			effect.m_ChromaticAberrationOffset 	= 2.5f;
			effect.m_RGBMaskIntensivity			= 0.8f;
			effect.m_RGBMaskStrength			= 0.8f;
			effect.m_RGBMaskBleeding			= 0.3f;
			effect.m_ColorNoiseMode				= NoiseMode.Add;
			effect.m_ColorNoiseStrength			= 0.25f;
			effect.m_WhiteNoiseMode				= NoiseMode.Lighten;
			effect.m_WhiteNoiseStrength			= 0.25f;
			effect.m_DarkestLevel				= Color.black;
			effect.m_BrightestLevel				= Color.Lerp(Color.black, Color.white, 225f / 255f);
			effect.m_DarkestColor				= Color.Lerp(Color.black, Color.white, 35f / 255f);
			effect.m_BrightestColor 			= Color.white;
			effect.m_Brightness					= 0.3f;
			effect.m_Contrast					= 0.3f;
			effect.m_Saturation					= -0.1f;
			effect.m_InterferenceWidth			= 25f;
			effect.m_InterferenceSpeed			= 3f;
			effect.m_InterferenceStrength		= 0f;
			effect.m_InterferenceSplit			= 0.25f;
			effect.m_CurvatureX					= 0.7f;
			effect.m_CurvatureY					= 0.7f;
			effect.m_Overscan					= 0f;
			effect.m_VignetteSize				= 0.5f;
			effect.m_VignetteStrength			= 0.425f;
		}
		public static void SetupColorTVPreset(CRTScreen effect) {
			effect.m_BlurSize					= 0.9f;
			effect.m_BlurStrength				= 0.6f;
			effect.m_BleedingSize 				= 0.85f;
			effect.m_BleedingStrength 			= 0.75f;
			effect.m_ChromaticAberrationOffset 	= 1.75f;
			effect.m_RGBMaskIntensivity			= 0.4f;
			effect.m_RGBMaskStrength			= 0.4f;
			effect.m_RGBMaskBleeding			= 0.1f;
			effect.m_ColorNoiseMode				= NoiseMode.Add;
			effect.m_ColorNoiseStrength			= 0.3f;
			effect.m_WhiteNoiseMode				= NoiseMode.Lighten;
			effect.m_WhiteNoiseStrength			= 0.2f;
			effect.m_DarkestLevel				= Color.black;
			effect.m_BrightestLevel				= Color.Lerp(Color.black, Color.white, 235f / 255f);
			effect.m_DarkestColor				= Color.Lerp(Color.black, Color.white, 35f / 255f);
			effect.m_BrightestColor				= new Color(245f / 255f, 1f, 1f);
			effect.m_Brightness					= 0.0f;
			effect.m_Contrast					= 0.2f;
			effect.m_Saturation					= 0.1f;
			effect.m_InterferenceWidth			= 25f;
			effect.m_InterferenceSpeed			= 3f;
			effect.m_InterferenceStrength		= 0f;
			effect.m_InterferenceSplit			= 0.25f;
			effect.m_CurvatureX					= 0.5f;
			effect.m_CurvatureY					= 0.5f;
			effect.m_Overscan					= 0.1f;
			effect.m_VignetteSize				= 0.4f;
			effect.m_VignetteStrength			= 0.5f;
		}
		public static void SetupOldTVPreset(CRTScreen effect) {
			effect.m_BlurSize					= 0.9f;
			effect.m_BlurStrength				= 0.8f;
			effect.m_BleedingSize 				= 0.95f;
			effect.m_BleedingStrength 			= 0.95f;
			effect.m_ChromaticAberrationOffset 	= 1.9f;
			effect.m_RGBMaskIntensivity			= 0.7f;
			effect.m_RGBMaskStrength			= 0.7f;
			effect.m_RGBMaskBleeding			= 0.3f;
			effect.m_ColorNoiseMode				= NoiseMode.Add;
			effect.m_ColorNoiseStrength			= 0.5f;
			effect.m_WhiteNoiseMode				= NoiseMode.Darken;
			effect.m_WhiteNoiseStrength			= 0.55f;
			effect.m_DarkestLevel				= Color.black;
			effect.m_BrightestLevel				= Color.Lerp(Color.black, Color.white, 235f / 255f);
			effect.m_DarkestColor				= Color.Lerp(Color.black, Color.white, 35f / 255f);
			effect.m_BrightestColor				= new Color(245f / 255f, 1f, 1f);
			effect.m_Brightness					= 0f;
			effect.m_Contrast					= -0.1f;
			effect.m_Saturation					= -0.05f;
			effect.m_InterferenceWidth			= 35f;
			effect.m_InterferenceSpeed			= 2f;
			effect.m_InterferenceStrength		= 0.075f;
			effect.m_InterferenceSplit			= 0.25f;
			effect.m_CurvatureX					= 0.625f;
			effect.m_CurvatureY					= 0.625f;
			effect.m_Overscan					= 0.1f;
			effect.m_VignetteSize				= 0.4f;
			effect.m_VignetteStrength			= 0.5f;
		}
		public static void SetupArcadeDisplayPreset(CRTScreen effect) {
			effect.m_BlurSize					= 0.5f;
			effect.m_BlurStrength				= 0.7f;
			effect.m_BleedingSize 				= 0.65f;
			effect.m_BleedingStrength 			= 0.8f;
			effect.m_ChromaticAberrationOffset 	= 0.9f;
			effect.m_RGBMaskIntensivity			= 0.4f;
			effect.m_RGBMaskStrength			= 0.4f;
			effect.m_RGBMaskBleeding			= 0.2f;
			effect.m_ColorNoiseMode				= NoiseMode.Lighten;
			effect.m_ColorNoiseStrength			= 0.15f;
			effect.m_WhiteNoiseMode				= NoiseMode.Lighten;
			effect.m_WhiteNoiseStrength			= 0.1f;
			effect.m_DarkestLevel				= Color.black;
			effect.m_BrightestLevel				= Color.white;
			effect.m_DarkestColor				= Color.black;
			effect.m_BrightestColor				= Color.white;
			effect.m_Brightness					= 0.1f;
			effect.m_Contrast					= 0.1f;
			effect.m_Saturation					= 0.1f;
			effect.m_InterferenceWidth			= 25f;
			effect.m_InterferenceSpeed			= 3f;
			effect.m_InterferenceStrength		= 0f;
			effect.m_InterferenceSplit			= 0.25f;
			effect.m_CurvatureX					= 0.0f;
			effect.m_CurvatureY					= 0.0f;
			effect.m_Overscan					= 0.0f;
			effect.m_VignetteSize				= 0.3f;
			effect.m_VignetteStrength			= 0.2f;
		}
		public static void SetupBrokenBlackWhitePreset(CRTScreen effect) {
			effect.m_BlurSize					= 0.9f;
			effect.m_BlurStrength				= 1f;
			effect.m_BleedingSize 				= 0.75f;
			effect.m_BleedingStrength 			= 0.9f;
			effect.m_ChromaticAberrationOffset 	= 2.5f;
			effect.m_RGBMaskIntensivity			= 0.6f;
			effect.m_RGBMaskStrength			= 0.6f;
			effect.m_RGBMaskBleeding			= 0.1f;
			effect.m_ColorNoiseMode				= NoiseMode.Add;
			effect.m_ColorNoiseStrength			= 0.75f;
			effect.m_WhiteNoiseMode				= NoiseMode.Lighten;
			effect.m_WhiteNoiseStrength			= 0.5f;
			effect.m_DarkestLevel				= Color.Lerp(Color.black, Color.white, 15f / 255f);
			effect.m_BrightestLevel				= Color.Lerp(Color.black, Color.white, 225f / 255f);
			effect.m_DarkestColor				= Color.Lerp(Color.black, Color.white, 60f / 255f);
			effect.m_BrightestColor 			= Color.white;
			effect.m_Brightness					= 0f;
			effect.m_Contrast					= -0.2f;
			effect.m_Saturation					= -1.0f;
			effect.m_InterferenceWidth			= 85f;
			effect.m_InterferenceSpeed			= 2.5f;
			effect.m_InterferenceStrength		= 0.05f;
			effect.m_InterferenceSplit			= 0f;
			effect.m_CurvatureX					= 0.6f;
			effect.m_CurvatureY					= 0.6f;
			effect.m_Overscan					= 0.4f;
			effect.m_VignetteSize				= 0.75f;
			effect.m_VignetteStrength			= 0.5f;
		}
		public enum Preset { Custom, MiniCRT, ColorTV, OldTV, ArcadeDisplay, BrokenBlackWhite };
		public enum NoiseMode { Add, Lighten, Darken };
		public enum MaskMode { Thin, Dense, Denser, ThinScanline, Scanline, DenseScanline };

		[Header("Basic")]
		public RenderPipelineAsset m_Pipeline;
		public CRT.CRTFeature m_Feature;
		public Material m_MatFunc;
		public Material m_MatPostPro;
		public Material m_MatFinal;
		public Preset m_CurrPreset = Preset.OldTV;
		Preset m_Preset = Preset.Custom;
		[Header("Blur")]
		[Range(0f, 1f)] public float m_BlurSize = 0.7f;
		[Range(0f, 1f)] public float m_BlurStrength = 0.6f;
		[Header("Luminosity Bleeding")]
		[Range(0f, 2f)] public float m_BleedingSize = 0.75f;
		[Range(0f, 1f)] public float m_BleedingStrength = 0.5f;
		[Header("Chromatic Aberration")]
		[Range(-2.5f, 2.5f)] public float m_ChromaticAberrationOffset = 1.25f;
		[Header("RGB Mask")]
		[Range(0f, 1f)] public float m_RGBMaskIntensivity = 0.6f;
		[Range(0f, 1f)] public float m_RGBMaskStrength = 0.6f;
		[Range(0f, 1f)] public float m_RGBMaskBleeding = 0.1f;
		[Header("Noise")]
		public NoiseMode m_ColorNoiseMode = NoiseMode.Add;
		[Range(0f, 1f)] public float m_ColorNoiseStrength = 0.15f;
		public NoiseMode m_WhiteNoiseMode = NoiseMode.Lighten;
		[Range(0f, 1f)] public float m_WhiteNoiseStrength = 0.25f;
		[Header("Color Adjustments")]
		public Color m_DarkestLevel = Color.black;
		public Color m_BrightestLevel = Color.Lerp(Color.black, Color.white, 235.0f / 255.0f);
		[Space(4)]
		public Color m_DarkestColor	= Color.Lerp(Color.black, Color.white, 40.0f / 255.0f);
		public Color m_BrightestColor = Color.white;
		[Space(4)]
		[Range(-1f, 1f)] public float m_Brightness = 0.2f;
		[Range(-1f, 1f)] public float m_Contrast =  0.1f;
		[Range(-1f, 1f)] public float m_Saturation = -0.05f;
		[Header("Horizontal Interference")]
		[Range(3, 80)] public float m_InterferenceWidth = 25f;
		[Range(-25.0f, 25.0f)] public float m_InterferenceSpeed = 3f;
		[Range(0f, 1f)] public float m_InterferenceStrength = 0f;
		[Range(0f, 1f)] public float m_InterferenceSplit = 0.25f;
		[Header("Curve And Vignette")]
		[Range(0f, 1f)] public float m_CurvatureX = 0.6f;
		[Range(0f, 1f)] public float m_CurvatureY = 0.6f;
		[Range(0f, 1f)] public float m_Overscan = 0f;
		[Space(4)]
		[Range(0f, 1f)] public float m_VignetteSize = 0.35f;
		[Range(0f, 2f)] public float m_VignetteStrength = 0.1f;
		[Header("Turn Off")]
		public bool m_TurnOff = false;
		public Color m_BgColor = new Color(0.0859f, 0.0937f, 0.1328f, 1f);
		[Header("Moire")]
		[Range(0f, 1f)] public float m_LineAmount = 0.25f;
		[Range(0f, 0.02f)] public float m_LineSpeed = 0.01f;
		//public float m_LineNumber = 150;

		float m_BlurSigma = float.NaN;
		float[] blurKernel = new float[2];
		float blurZ = float.NaN;

		float m_CurrentBrightness = float.NaN;
		Matrix4x4 m_BrightnessMat = new Matrix4x4();
		float m_CurrentContrast = float.NaN;
		Matrix4x4 m_ContrastMat = new Matrix4x4();
		float m_CurrentSaturation = float.NaN;
		Matrix4x4 m_SaturationMat = new Matrix4x4();
		Matrix4x4 m_ColorMat = new Matrix4x4();

		void Start()
		{
			if (m_Pipeline != null)
				GraphicsSettings.renderPipelineAsset = m_Pipeline;
			m_Feature.SetupMaterial(m_MatFunc, m_MatPostPro, m_MatFinal);
		}
		void Update()
		{
			if (m_CurrPreset != m_Preset)
			{
				m_Preset = m_CurrPreset;
				switch (m_Preset) {
					case Preset.Custom:
						break;
					case Preset.MiniCRT:
						SetupMiniCRTPreset(this);
						break;
					case Preset.ColorTV:
						SetupColorTVPreset(this);
						break;
					case Preset.OldTV:
						SetupOldTVPreset(this);
						break;
					case Preset.ArcadeDisplay:
						SetupArcadeDisplayPreset(this);
						break;
					case Preset.BrokenBlackWhite:
						SetupBrokenBlackWhitePreset(this);
						break;
				}
			}
			UpdateMaterialParameters();
		}
		void OnDisable() {}
		void UpdateMaterialParameters()
		{
			m_Feature.SetTurnOff(m_TurnOff);

			float sz = Mathf.Lerp(0.00001f, 1.99999f, m_BlurSize);
			UpdateBlurKernel(sz);

			float brightness = Mathf.Lerp(0.8f, 1.2f, (m_Brightness + 1f) / 2f);
			float contrast = Mathf.Lerp(0.5f, 1.5f, (m_Contrast + 1f) / 2f);
			float saturation = Mathf.Lerp(0.0f, 2.0f, (m_Saturation + 1f) / 2f);
			UpdateColorMatrices(brightness - 1.5f, contrast, saturation);

			m_MatFunc.SetVector("_BlurKernel", new Vector4(blurKernel[0], blurKernel[1]));
			m_MatFunc.SetFloat("_BlurZ", blurZ);
			m_MatFunc.SetColor("_BgColor", m_BgColor);
			m_MatFunc.SetFloat("_LineAmount", m_LineAmount);
			m_MatFunc.SetFloat("_Speed", m_LineSpeed);
			//m_MatFunc.SetFloat("_Number", m_LineNumber);

			//m_MatPostPro.SetTexture("_BlurTex", rtBlur);
			m_MatPostPro.SetFloat("_BlurStr", 1f - m_BlurStrength);
			m_MatPostPro.SetFloat("_BleedDist", m_BleedingSize);
			m_MatPostPro.SetFloat("_BleedStr", m_BleedingStrength);
			m_MatPostPro.SetFloat("_RgbMaskStr", Mathf.Lerp(0f, 0.3f, m_RGBMaskStrength));
			m_MatPostPro.SetFloat("_RgbMaskSub", m_RGBMaskIntensivity);
			m_MatPostPro.SetFloat("_RgbMaskSep", 1f - m_RGBMaskBleeding);
			m_MatPostPro.SetFloat("_ColorNoiseStr", Mathf.Lerp(0f, 0.4f, m_ColorNoiseStrength));
			m_MatPostPro.SetInt("_ColorNoiseMode", (int) m_ColorNoiseMode);
			m_MatPostPro.SetFloat("_MonoNoiseStr", Mathf.Lerp(0f, 0.4f, m_WhiteNoiseStrength));
			m_MatPostPro.SetInt("_MonoNoiseMode", (int) m_WhiteNoiseMode);
			m_MatPostPro.SetMatrix("_ColorMat", m_ColorMat);
			m_MatPostPro.SetColor("_MinLevels", m_DarkestLevel);
			m_MatPostPro.SetColor("_MaxLevels", m_BrightestLevel);
			m_MatPostPro.SetColor("_BlackPoint", m_DarkestColor);
			m_MatPostPro.SetColor("_WhitePoint", m_BrightestColor);
			m_MatPostPro.SetFloat("_InterWidth", m_InterferenceWidth);
			m_MatPostPro.SetFloat("_InterSpeed", m_InterferenceSpeed);
			m_MatPostPro.SetFloat("_InterStr", m_InterferenceStrength);
			m_MatPostPro.SetFloat("_InterSplit", m_InterferenceSplit);
			m_MatPostPro.SetFloat("_AberStr", -m_ChromaticAberrationOffset);

			float realCurvatureX = Mathf.Lerp(0.25f, 0.45f, m_CurvatureX);
			float realCurvatureY = Mathf.Lerp(0.25f, 0.45f, m_CurvatureY);

			m_MatFinal.SetFloat("_VignetteStr", m_VignetteStrength);
			m_MatFinal.SetFloat("_VignetteSize", 1f - m_VignetteSize);
			m_MatFinal.SetFloat("_CrtBendX", Mathf.Lerp(1f, 100f, (1f - realCurvatureX) / Mathf.Exp(10f * realCurvatureX)));
			m_MatFinal.SetFloat("_CrtBendY", Mathf.Lerp(1f, 100f, (1f - realCurvatureY) / Mathf.Exp(10f * realCurvatureY)));
			m_MatFinal.SetFloat("_CrtOverscan", Mathf.Lerp(0f, 0.25f, m_Overscan));
		}
		float CalculateBlurWeight(float x, float sigma) { return 0.39894f * Mathf.Exp(-0.5f * x * x / (sigma * sigma)) / sigma; }
		void UpdateBlurKernel(float sigma)
		{
			if (sigma == m_BlurSigma)
				return;

			m_BlurSigma = sigma;
			const int kSize = 1;

			blurZ = 0.0f;
			for(int j = 0; j <= kSize; ++j) {
				float normal = CalculateBlurWeight(j, sigma);
				blurKernel[kSize - j] = normal;

				if(j > 0)
					blurZ += 2 * normal;
				else
					blurZ += normal;
			}
			blurZ *= blurZ;
		}
		void UpdateColorMatrices(float b, float c, float s)
		{
			bool rebuildColorMat = false;
			if (b != m_CurrentBrightness) {
				rebuildColorMat = true;
				m_CurrentBrightness = b;

				m_BrightnessMat.SetColumn(0, new Vector4(1f, 0f, 0f, 0f));
				m_BrightnessMat.SetColumn(1, new Vector4(0f, 1f, 0f, 0f));
				m_BrightnessMat.SetColumn(2, new Vector4(0f, 0f, 1f, 0f));
				m_BrightnessMat.SetColumn(3, new Vector4(b, b, b, 1f));
			}
			if (c != m_CurrentContrast) {
				rebuildColorMat = true;
				m_CurrentContrast = c;

				float t = (1f - m_Contrast) / 2f;

				m_ContrastMat.SetColumn(0, new Vector4(c, 0f, 0f, 0f));
				m_ContrastMat.SetColumn(1, new Vector4(0f, c, 0f, 0f));
				m_ContrastMat.SetColumn(2, new Vector4(0f, 0f, c, 0f));
				m_ContrastMat.SetColumn(3, new Vector4(t, t, t, 1f));
			}
			if (s != m_CurrentSaturation) {
				rebuildColorMat = true;
				m_CurrentSaturation = s;

				Vector3 luminance = new Vector3(0.3086f, 0.6094f, 0.0820f);
				float t = 1f - s;

				Vector4 red = new Vector4(luminance.x * t + s, luminance.x * t, luminance.x * t, 0f);
				Vector4 green = new Vector4(luminance.y * t, luminance.y * t + s, luminance.y * t, 0f);
				Vector4 blue = new Vector4(luminance.z * t, luminance.z * t, luminance.z * t + s, 0f);

				m_SaturationMat.SetColumn(0, red);
				m_SaturationMat.SetColumn(1, green);
				m_SaturationMat.SetColumn(2, blue);
				m_SaturationMat.SetColumn(3, new Vector4(0f, 0f, 0f, 1f));
			}
			if (rebuildColorMat)
				m_ColorMat = m_BrightnessMat * m_ContrastMat * m_SaturationMat;
		}
	}
}