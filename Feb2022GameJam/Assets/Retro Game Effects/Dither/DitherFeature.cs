using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace RetroGameEffects.Dither
{
	public class DitherFeature : ScriptableRendererFeature
	{
		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public class Pass : ScriptableRenderPass
		{
			Material m_Mat;
			RenderTargetIdentifier source { get; set; }
			RenderTargetHandle m_TempColorTexture;
			string m_ProfilerTag;

			public Pass(string tag)
			{
				this.renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
				m_ProfilerTag = tag;
				m_TempColorTexture.Init("_TemporaryColorTexture");
			}
			public void Setup(RenderTargetIdentifier source, Material mat)
			{
				this.source = source;
				m_Mat = mat;
			}
			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
				opaqueDesc.depthBufferBits = 0;

				CommandBuffer cmd = CommandBufferPool.Get(m_ProfilerTag);
				cmd.GetTemporaryRT(m_TempColorTexture.id, opaqueDesc, FilterMode.Bilinear);

				Blit(cmd, source, m_TempColorTexture.Identifier(), m_Mat, 0);
				Blit(cmd, m_TempColorTexture.Identifier(), source);

				context.ExecuteCommandBuffer(cmd);
				CommandBufferPool.Release(cmd);
			}
			public override void FrameCleanup(CommandBuffer cmd)
			{
				cmd.ReleaseTemporaryRT(m_TempColorTexture.id);
			}
		}
		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public Material m_Mat;
		Pass m_Pass;

		public override void Create()
		{
			m_Pass = new Pass(name);
		}
		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			RenderTargetIdentifier src = renderer.cameraColorTarget;
			if (m_Mat == null)
			{
				Debug.LogWarningFormat("Missing material. {0} pass will not execute. Check for missing reference in the assigned renderer.", GetType().Name);
				return;
			}
			m_Pass.Setup(src, m_Mat);
			renderer.EnqueuePass(m_Pass);
		}
	}
}