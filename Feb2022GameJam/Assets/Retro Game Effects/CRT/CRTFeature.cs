using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace RetroGameEffects.CRT
{
	public class CRTFeature : ScriptableRendererFeature
	{
		public class Pass : ScriptableRenderPass
		{
			Material m_MatFunc;
			Material m_MatPostPro;
			Material m_MatFinal;
			string m_ProfilerTag;
			RenderTargetIdentifier m_RtID1;
			RenderTargetIdentifier m_RtID2;
			RenderTargetIdentifier m_Source;
			int m_RtPropID1 = 0;
			int m_RtPropID2 = 0;
			bool m_TurnOff = false;

			public Pass(string tag)
			{
				this.renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
				m_ProfilerTag = tag;
			}
			public void Setup(RenderTargetIdentifier source, Material matBlur, Material matPostPro, Material matFinal, bool turnOff)
			{
				m_Source = source;
				m_MatFunc = matBlur;
				m_MatPostPro = matPostPro;
				m_MatFinal = matFinal;
				m_TurnOff = turnOff;
			}
			public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
			{
				int width = cameraTextureDescriptor.width;
				int height = cameraTextureDescriptor.height;

				m_RtPropID1 = Shader.PropertyToID("tmpRT1");
				m_RtPropID2 = Shader.PropertyToID("tmpRT2");
				cmd.GetTemporaryRT(m_RtPropID1, width, height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32);
				cmd.GetTemporaryRT(m_RtPropID2, width, height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32);
				m_RtID1 = new RenderTargetIdentifier(m_RtPropID1);
				m_RtID2 = new RenderTargetIdentifier(m_RtPropID2);
				ConfigureTarget(m_RtID1);
				ConfigureTarget(m_RtID2);
			}
			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				CommandBuffer cmd = CommandBufferPool.Get(m_ProfilerTag);
				cmd.Blit(m_Source, m_RtID1, m_MatFunc, 0);

				cmd.SetGlobalTexture("_BlurTex", m_RtID1);
				cmd.Blit(m_Source, m_RtID2, m_MatPostPro);
				if (m_TurnOff)
				{
					cmd.Blit(m_RtID2, m_RtID1, m_MatFunc, 1);
					cmd.Blit(m_RtID1, m_RtID2, m_MatFunc, 2);
					cmd.Blit(m_RtID2, m_Source, m_MatFinal);
				}
				else
				{
					cmd.Blit(m_RtID2, m_RtID1, m_MatFunc, 2);
					cmd.Blit(m_RtID1, m_Source, m_MatFinal);
				}
				context.ExecuteCommandBuffer(cmd);
				CommandBufferPool.Release(cmd);
			}
			public override void FrameCleanup(CommandBuffer cmd)
			{
				cmd.ReleaseTemporaryRT(m_RtPropID1);
				cmd.ReleaseTemporaryRT(m_RtPropID2);
			}
		}
		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		Material m_MatFunc;
		Material m_MatPostPro;
		Material m_MatFinal;
		Pass m_Pass;
		bool m_TurnOff;
		public override void Create()
		{
			m_Pass = new Pass(name);
		}
		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			RenderTargetIdentifier src = renderer.cameraColorTarget;
			m_Pass.Setup(src, m_MatFunc, m_MatPostPro, m_MatFinal, m_TurnOff);
			renderer.EnqueuePass(m_Pass);
		}
		public void SetupMaterial(Material matFunc, Material matPostPro, Material matFinal)
		{
			m_MatFunc = matFunc;
			m_MatPostPro = matPostPro;
			m_MatFinal = matFinal;
		}
		public void SetTurnOff(bool enable)
		{
			m_TurnOff = enable;
		}
	}
}