using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace RetroGameEffects.Stylized
{
	public class DepthNormalFeature : ScriptableRendererFeature
	{
		class Pass : ScriptableRenderPass
		{
			RenderTargetHandle depthAttachmentHandle { get; set; }
			internal RenderTextureDescriptor descriptor { get; private set; }

			Material m_Mat = null;
			FilteringSettings m_FilteringSettings;
			string m_ProfilerTag = "DepthNormals Prepass";
			ShaderTagId m_ShaderTagId = new ShaderTagId("DepthOnly");

			public Pass(RenderQueueRange renderQueueRange, LayerMask layerMask, Material material)
			{
				m_FilteringSettings = new FilteringSettings(renderQueueRange, layerMask);
				m_Mat = material;
			}
			public void Setup(RenderTextureDescriptor baseDescriptor, RenderTargetHandle depthAttachmentHandle)
			{
				this.depthAttachmentHandle = depthAttachmentHandle;
				baseDescriptor.colorFormat = RenderTextureFormat.ARGB32;
				baseDescriptor.depthBufferBits = 32;
				descriptor = baseDescriptor;
			}
			public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
			{
				cmd.GetTemporaryRT(depthAttachmentHandle.id, descriptor, FilterMode.Point);
				ConfigureTarget(depthAttachmentHandle.Identifier());
				ConfigureClear(ClearFlag.All, Color.black);
			}
			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				CommandBuffer cmd = CommandBufferPool.Get(m_ProfilerTag);
				{
					context.ExecuteCommandBuffer(cmd);
					cmd.Clear();

					var sortFlags = renderingData.cameraData.defaultOpaqueSortFlags;
					var drawSettings = CreateDrawingSettings(m_ShaderTagId, ref renderingData, sortFlags);
					drawSettings.perObjectData = PerObjectData.None;

					ref CameraData data = ref renderingData.cameraData;
					Camera camera = data.camera;
					if (data.isStereoEnabled)
						context.StartMultiEye(camera);

					drawSettings.overrideMaterial = m_Mat;
					context.DrawRenderers(renderingData.cullResults, ref drawSettings, ref m_FilteringSettings);

					cmd.SetGlobalTexture("_CameraDepthNormalsTexture", depthAttachmentHandle.id);
				}
				context.ExecuteCommandBuffer(cmd);
				CommandBufferPool.Release(cmd);
			}
			public override void FrameCleanup(CommandBuffer cmd)
			{
				if (depthAttachmentHandle != RenderTargetHandle.CameraTarget)
				{
					cmd.ReleaseTemporaryRT(depthAttachmentHandle.id);
					depthAttachmentHandle = RenderTargetHandle.CameraTarget;
				}
			}
		}

		Pass m_Pass;
		RenderTargetHandle m_RtDepthNormals;
		Material m_Mat;

		public override void Create()
		{
			m_Mat = CoreUtils.CreateEngineMaterial("Hidden/Internal-DepthNormalsTexture");
			m_Pass = new Pass(RenderQueueRange.opaque, -1, m_Mat);
			m_Pass.renderPassEvent = RenderPassEvent.AfterRenderingPrePasses;
			m_RtDepthNormals.Init("_CameraDepthNormalsTexture");
		}
		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			m_Pass.Setup(renderingData.cameraData.cameraTargetDescriptor, m_RtDepthNormals);
			renderer.EnqueuePass(m_Pass);
		}
	}
}