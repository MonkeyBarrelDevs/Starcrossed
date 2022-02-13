using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace RetroGameEffects.Stylized
{
	public class StylizedFeature : ScriptableRendererFeature
	{
		public enum EDrawStyle {
			One = 0,
			Two = 1,
			Three = 2,
		}
		public enum EBlendMode {
			Overlay = 0,
			AlphaBlend = 1,
		}
		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public class Pass : ScriptableRenderPass
		{
			Material m_MatBCS;
			Material m_MatBlendMode;
			EBlendMode m_BlendMode;
			string m_ProfilerTag;
			RenderTargetIdentifier m_RtID1;
			RenderTargetIdentifier m_RtID2;
			RenderTargetIdentifier m_RtID3;
			RenderTargetIdentifier m_Source;
			int m_RtPropID1 = 0;
			int m_RtPropID2 = 0;
			int m_RtPropID3 = 0;
			EDrawStyle m_Style;

			public Pass(string tag)
			{
				this.renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
				m_ProfilerTag = tag;
			}
			public void Setup(RenderTargetIdentifier source, Material matBCS, Material matBlendMode, EBlendMode mode, EDrawStyle style)
			{
				m_Source = source;
				m_MatBCS = matBCS;
				m_MatBlendMode = matBlendMode;
				m_BlendMode = mode;
				m_Style = style;
			}
			public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
			{
				int w = cameraTextureDescriptor.width;
				int h = cameraTextureDescriptor.height;

				m_RtPropID1 = Shader.PropertyToID("tmpRT1");
				m_RtPropID2 = Shader.PropertyToID("tmpRT2");
				m_RtPropID3 = Shader.PropertyToID("tmpRT3");
				cmd.GetTemporaryRT(m_RtPropID1, w, h, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32);
				cmd.GetTemporaryRT(m_RtPropID2, w, h, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32);
				cmd.GetTemporaryRT(m_RtPropID3, w, h, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32);
				m_RtID1 = new RenderTargetIdentifier(m_RtPropID1);
				m_RtID2 = new RenderTargetIdentifier(m_RtPropID2);
				m_RtID3 = new RenderTargetIdentifier(m_RtPropID3);
				ConfigureTarget(m_RtID1);
				ConfigureTarget(m_RtID2);
				ConfigureTarget(m_RtID3);
			}
			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				CommandBuffer cmd = CommandBufferPool.Get(m_ProfilerTag);

				cmd.Blit(m_Source, m_RtID1, m_MatBCS, 0);
				if (m_Style == EDrawStyle.One)
					cmd.Blit(m_RtID1, m_RtID2, m_MatBCS, 3);
				if (m_Style == EDrawStyle.Two || m_Style == EDrawStyle.Three)
					cmd.Blit(m_RtID1, m_RtID2, m_MatBCS, 1);
				cmd.Blit(m_RtID1, m_RtID3, m_MatBCS, 2);
				cmd.SetGlobalTexture("_OverlayTex", m_RtID3);

//cmd.Blit(m_RtID1, m_Source);
//cmd.Blit(m_RtID2, m_Source);
//cmd.Blit(m_RtID3, m_Source);
//cmd.Blit(m_RtID1, m_Source, m_MatBCS, 3);
//context.ExecuteCommandBuffer(cmd);
//CommandBufferPool.Release(cmd);
//return;

				cmd.Blit(m_RtID2, m_Source, m_MatBlendMode, (int)m_BlendMode);
				context.ExecuteCommandBuffer(cmd);
				CommandBufferPool.Release(cmd);
			}
			public override void FrameCleanup(CommandBuffer cmd)
			{
				cmd.ReleaseTemporaryRT(m_RtPropID1);
				cmd.ReleaseTemporaryRT(m_RtPropID2);
				cmd.ReleaseTemporaryRT(m_RtPropID3);
			}
		}
		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		Material m_MatBCS;
		Material m_MatBlend;
		EDrawStyle m_Style = EDrawStyle.One;
		EBlendMode m_BlendMode = EBlendMode.AlphaBlend;
		Pass m_Pass;

		public override void Create()
		{
			m_Pass = new Pass(name);
		}
		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			if (m_MatBCS == null || m_MatBlend == null)
			{
				Debug.LogWarningFormat("Missing material. {0} pass will not execute. Check for missing reference in the assigned renderer.", GetType().Name);
				return;
			}
			RenderTargetIdentifier src = renderer.cameraColorTarget;
			m_Pass.Setup(src, m_MatBCS, m_MatBlend, m_BlendMode, m_Style);
			renderer.EnqueuePass(m_Pass);
		}
		public void SteupMaterials(Material matBCS, Material matBlend)
		{
			m_MatBCS = matBCS;
			m_MatBlend = matBlend;
		}
		public void SetupParameters(EDrawStyle style, EBlendMode blend)
		{
			m_Style = style;
			m_BlendMode = blend;
		}
	}
}