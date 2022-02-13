using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace RetroGameEffects.Dither
{
	public class Dither : MonoBehaviour
	{
		[Header("Basic")]
		public RenderPipelineAsset m_Pipeline;
		public DitherFeature m_Feature;
		public Material m_Mat;
		[Range(1.1f, 6f)] public float m_Pixelate = 4f;
		public Texture2D m_PatternTex;
		[Range(0f, 1f)] public float m_Quantize = 0.125f;

		public enum PaletteMode { Off, Palette1, Palette2, Palette3 };
		[Header("Palette")]
		public PaletteMode m_Mode = PaletteMode.Palette1;
		public Texture2D m_PaletteTex1;   // 1 pixel height
		public Texture2D m_PaletteTex2;
		public Texture2D m_PaletteTex3;
		List<Vector4> m_Palette1 = new List<Vector4>();
		List<Vector4> m_Palette2 = new List<Vector4>();
		List<Vector4> m_Palette3 = new List<Vector4>();

		void BuildPalette(Texture2D t, List<Vector4> l)
		{
			for (int i = 0; i < t.width; i++)
			{
				Color c = t.GetPixel(i, 0);
				l.Add(new Vector4(c.r, c.g, c.b, c.a));
			}
		}
		void Start()
		{
			if (m_Pipeline != null)
				GraphicsSettings.renderPipelineAsset = m_Pipeline;
			m_Feature.m_Mat = m_Mat;
			BuildPalette(m_PaletteTex1, m_Palette1);
			BuildPalette(m_PaletteTex2, m_Palette2);
			BuildPalette(m_PaletteTex3, m_Palette3);
		}
		void Update()
		{
			if (m_Mode == PaletteMode.Off)
			{
				m_Mat.DisableKeyword("RGE_Palette");
			}
			else
			{
				m_Mat.EnableKeyword("RGE_Palette");

				List<Vector4> l = new List<Vector4>();
				if (m_Mode == PaletteMode.Palette1)
					l = m_Palette1;
				else if (m_Mode == PaletteMode.Palette2)
					l = m_Palette2;
				else if (m_Mode == PaletteMode.Palette3)
					l = m_Palette3;
				m_Mat.SetInt("_PaletteSize", l.Count);
				Shader.SetGlobalVectorArray("_Palette", l.ToArray());
			}
			m_Mat.SetTexture("_PatternTex", m_PatternTex);
			m_Mat.SetFloat("_Pixelate", m_Pixelate);
			m_Mat.SetFloat("_Quantize", m_Quantize);
		}
	}
}