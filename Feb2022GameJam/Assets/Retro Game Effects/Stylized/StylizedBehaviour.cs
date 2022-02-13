using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace RetroGameEffects.Stylized
{
	public class StylizedBehaviour : MonoBehaviour
	{
		public StylizedFeature m_Feature;
		public StylizedFeature.EDrawStyle m_DrawStyle;
		StylizedFeature.EBlendMode m_BlendMode;
		public Material m_Mat;
		public Material m_MatBlend;
//		[Header("Brightness Saturation Contrast")]
//		public float m_Saturation = 1f;
//		public float m_Brightness = 1f;
//		public float m_Contrast = 1f;
		[Header("Dot")]
		public Color m_Color = Color.white;
		public float m_Angle = 1.57f;
		public float m_Scale = 1f;
		[Header("Outline")]
		public float m_SensitivityDepth = 1f;
		public float m_SensitivityNormals = 1f;
		public float m_SampleDist = 1f;
		[Header("DrawNoise")]
		public float m_Step = 5f;
		[Header("Blend")]
		[Range(0f, 1f)] public float m_Intensity = 0f;
		public Texture2D m_PaperOverlay;

		void Start()
		{
			m_Feature.SteupMaterials(m_Mat, m_MatBlend);
		}
		void Update()
		{
			if (m_DrawStyle == StylizedFeature.EDrawStyle.One)
			{
				m_Mat.SetFloat("_Saturation", 1f);
				m_Mat.SetFloat("_Brightness", 1.5f);
				m_Mat.SetFloat("_Contrast", 1f);
				m_BlendMode = StylizedFeature.EBlendMode.AlphaBlend;
			}
			if (m_DrawStyle == StylizedFeature.EDrawStyle.Two)
			{
				m_Mat.SetFloat("_Saturation", 1f);
				m_Mat.SetFloat("_Brightness", 0.6f);
				m_Mat.SetFloat("_Contrast", 1f);
				m_BlendMode = StylizedFeature.EBlendMode.AlphaBlend;
			}
			if (m_DrawStyle == StylizedFeature.EDrawStyle.Three)
			{
				m_Mat.SetFloat("_Saturation", 1f);
				m_Mat.SetFloat("_Brightness", 0.6f);
				m_Mat.SetFloat("_Contrast", 1f);
				m_BlendMode = StylizedFeature.EBlendMode.Overlay;
			}
//			m_Mat.SetFloat("_Saturation", m_Saturation);
//			m_Mat.SetFloat("_Brightness", m_Brightness);
//			m_Mat.SetFloat("_Contrast", m_Contrast);
			
			m_Mat.SetColor("_Color", m_Color);
			m_Mat.SetFloat("_Angle", m_Angle);
			m_Mat.SetFloat("_Scale", m_Scale);
			
			m_Mat.SetFloat("_Step", m_Step);
			
			Vector2 sensitivity = new Vector2(m_SensitivityDepth, m_SensitivityNormals);
			m_Mat.SetColor("_Color", m_Color);
			m_Mat.SetVector("_Sensitivity", new Vector4(sensitivity.x, sensitivity.y, 1f, sensitivity.y));
			m_Mat.SetFloat("_SampleDistance", m_SampleDist);
			
			m_MatBlend.SetTexture("_PaperTex", m_PaperOverlay);
			m_MatBlend.SetFloat("_Intensity", m_Intensity);

			m_Feature.SetupParameters(m_DrawStyle, m_BlendMode);
		}
	}
}