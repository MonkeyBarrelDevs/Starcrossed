using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace RetroGameEffects.VHS
{
	public class VHSFeature : ScriptableRendererFeature
	{
		public class HeavyPass : ScriptableRenderPass
		{
			Material m_Mat;
			Material m_MatTV;
			string m_ProfilerTag;
			RenderTargetIdentifier m_RtID1;
			RenderTargetIdentifier m_RtID2;
			RenderTargetIdentifier m_Source;
			int m_RtPropID1 = 0;
			int m_RtPropID2 = 0;

			public HeavyPass(string tag)
			{
				this.renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
				m_ProfilerTag = tag;
			}
			public void Setup(RenderTargetIdentifier source, Material mat, Material matTV)
			{
				m_Source = source;
				m_Mat = mat;
				m_MatTV = matTV;
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
				cmd.Blit(m_Source, m_RtID1, m_Mat, 2);
				cmd.Blit(m_RtID1, m_RtID2, m_Mat, 1);
				cmd.Blit(m_RtID2, m_RtID1, m_Mat, 0);
				cmd.Blit(m_RtID1, m_Source, m_MatTV);
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
		public class SinglePass : ScriptableRenderPass
		{
			Material m_Mat;
			RenderTargetIdentifier m_Source;
			RenderTargetHandle m_TempColorTexture;
			string m_ProfilerTag;

			public SinglePass(string tag)
			{
				this.renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
				m_ProfilerTag = tag;
				m_TempColorTexture.Init("_TemporaryColorTexture");
			}
			public void Setup(RenderTargetIdentifier source, Material mat)
			{
				m_Source = source;
				m_Mat = mat;
			}
			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
				opaqueDesc.depthBufferBits = 0;

				CommandBuffer cmd = CommandBufferPool.Get(m_ProfilerTag);
				cmd.GetTemporaryRT(m_TempColorTexture.id, opaqueDesc, FilterMode.Bilinear);

				Blit(cmd, m_Source, m_TempColorTexture.Identifier(), m_Mat, 0);
				Blit(cmd, m_TempColorTexture.Identifier(), m_Source);

				context.ExecuteCommandBuffer(cmd);
				CommandBufferPool.Release(cmd);
			}
			public override void FrameCleanup(CommandBuffer cmd)
			{
				cmd.ReleaseTemporaryRT(m_TempColorTexture.id);
			}
		}
		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		[System.Serializable] public class VHSHeavyPassConfig
		{
			public Material m_Mat;
			public Material m_MatTV;
			public HeavyPass m_Pass;
		}
		[System.Serializable] public class VHSSinglePassConfig
		{
			public Material m_Mat;
		}
		public enum EDrawStyle { Style1 = 0, Style2, Style3, Style4, Style5, Style6 }
		public EDrawStyle m_Style = EDrawStyle.Style1;
		public VHSHeavyPassConfig m_Style1;
		public VHSSinglePassConfig m_Style2, m_Style3, m_Style4, m_Style5, m_Style6;
		SinglePass m_SinglePass;

		public override void Create()
		{
			m_Style1.m_Pass = new HeavyPass(name);
			m_SinglePass = new SinglePass(name);
		}
		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			RenderTargetIdentifier src = renderer.cameraColorTarget;
			if (m_Style == EDrawStyle.Style1)
			{
				if (m_Style1.m_Mat == null || m_Style1.m_MatTV == null)
				{
					Debug.LogWarningFormat("Missing material. {0} pass will not execute. Check for missing reference in the assigned renderer.", GetType().Name);
					return;
				}
				m_Style1.m_Pass.Setup(src, m_Style1.m_Mat, m_Style1.m_MatTV);
				renderer.EnqueuePass(m_Style1.m_Pass);
			}
			else if (m_Style == EDrawStyle.Style2)
			{
				if (m_Style2.m_Mat == null)
				{
					Debug.LogWarningFormat("Missing material. {0} pass will not execute. Check for missing reference in the assigned renderer.", GetType().Name);
					return;
				}
				m_SinglePass.Setup(src, m_Style2.m_Mat);
				renderer.EnqueuePass(m_SinglePass);
			}
			else if (m_Style == EDrawStyle.Style3)
			{
				if (m_Style3.m_Mat == null)
				{
					Debug.LogWarningFormat("Missing material. {0} pass will not execute. Check for missing reference in the assigned renderer.", GetType().Name);
					return;
				}
				m_SinglePass.Setup(src, m_Style3.m_Mat);
				renderer.EnqueuePass(m_SinglePass);
			}
			else if (m_Style == EDrawStyle.Style4)
			{
				if (m_Style4.m_Mat == null)
				{
					Debug.LogWarningFormat("Missing material. {0} pass will not execute. Check for missing reference in the assigned renderer.", GetType().Name);
					return;
				}
				m_SinglePass.Setup(src, m_Style4.m_Mat);
				renderer.EnqueuePass(m_SinglePass);
			}
			else if (m_Style == EDrawStyle.Style5)
			{
				if (m_Style5.m_Mat == null)
				{
					Debug.LogWarningFormat("Missing material. {0} pass will not execute. Check for missing reference in the assigned renderer.", GetType().Name);
					return;
				}
				m_SinglePass.Setup(src, m_Style5.m_Mat);
				renderer.EnqueuePass(m_SinglePass);
			}
			else if (m_Style == EDrawStyle.Style6)
			{
				if (m_Style6.m_Mat == null)
				{
					Debug.LogWarningFormat("Missing material. {0} pass will not execute. Check for missing reference in the assigned renderer.", GetType().Name);
					return;
				}
				m_SinglePass.Setup(src, m_Style6.m_Mat);
				renderer.EnqueuePass(m_SinglePass);
			}
		}
	}
}