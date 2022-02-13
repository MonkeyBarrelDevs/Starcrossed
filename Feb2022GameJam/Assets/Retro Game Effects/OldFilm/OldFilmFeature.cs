using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace RetroGameEffects.OldFilm
{
	public class OldFilmFeature : ScriptableRendererFeature
	{
		public enum EDrawStyle { PassOne = 0, PassTwo = 1, PassThree = 2, }
		public class Pass : ScriptableRenderPass
		{
			Material m_Mat;
			string m_ProfilerTag;
			RenderTargetIdentifier m_RtID;
			RenderTargetIdentifier m_Source;
			int m_RtPropID = 0;
			EDrawStyle m_DrawStyle = EDrawStyle.PassOne;

			public Pass(string tag)
			{
				this.renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
				m_ProfilerTag = tag;
			}
			public void Setup(RenderTargetIdentifier source, Material mat, EDrawStyle style)
			{
				m_Source = source;
				m_Mat = mat;
				m_DrawStyle = style;
			}
			public override void Configure(CommandBuffer cmd, RenderTextureDescriptor rtDesc)
			{
				m_RtPropID = Shader.PropertyToID("tmpRT");
				cmd.GetTemporaryRT(m_RtPropID, rtDesc.width, rtDesc.height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32);
				m_RtID = new RenderTargetIdentifier(m_RtPropID);
				ConfigureTarget(m_RtID);
			}
			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				CommandBuffer cmd = CommandBufferPool.Get(m_ProfilerTag);

				if (m_DrawStyle == EDrawStyle.PassOne)
					cmd.Blit(m_Source, m_RtID, m_Mat, 0);
				else if (m_DrawStyle == EDrawStyle.PassTwo)
					cmd.Blit(m_Source, m_RtID, m_Mat, 1);
				else if (m_DrawStyle == EDrawStyle.PassThree)
					cmd.Blit(m_Source, m_RtID, m_Mat, 2);

				cmd.Blit(m_RtID, m_Source);
				context.ExecuteCommandBuffer(cmd);
				CommandBufferPool.Release(cmd);
			}
			public override void FrameCleanup(CommandBuffer cmd)
			{
				cmd.ReleaseTemporaryRT(m_RtPropID);
			}
		}
		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public EDrawStyle m_DrawStyle = EDrawStyle.PassOne;
		public Material m_Mat;
		Pass m_Pass;

		public override void Create()
		{
			m_Pass = new Pass(name);
		}
		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			RenderTargetIdentifier src = renderer.cameraColorTarget;
			m_Pass.Setup(src, m_Mat, m_DrawStyle);
			renderer.EnqueuePass(m_Pass);
		}
	}
}