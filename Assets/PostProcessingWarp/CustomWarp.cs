using System;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CustomWarp : ScriptableRendererFeature
{

    class CustomRenderPass : ScriptableRenderPass
    {
        private WarpSettings settings;
        private RenderTargetIdentifier cameraTex;
        private RenderTextureDescriptor camSettings;
        private RenderTargetIdentifier tempOne;
        private RenderTargetHandle handle;
        public CustomRenderPass(WarpSettings settings)
        {
            this.settings = settings;
            handle.Init("Handle");
        }

        // This method is called before executing the render pass.
        // It can be used to configure render targets and their clear state. Also to create temporary render target textures.
        // When empty this render pass will render to the active camera render target.
        // You should never call CommandBuffer.SetRenderTarget. Instead call <c>ConfigureTarget</c> and <c>ConfigureClear</c>.
        // The render pipeline will ensure target setup and clearing happens in a performant manner.
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            camSettings = renderingData.cameraData.cameraTargetDescriptor;
            cameraTex = renderingData.cameraData.renderer.cameraColorTarget;
            camSettings.enableRandomWrite = true;
            cmd.GetTemporaryRT(handle.id, camSettings, FilterMode.Bilinear);
        }

        // Here you can implement the rendering logic.
        // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
        // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
        // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("Custom Warp pass");
            cmd.Clear();
            cmd.SetComputeTextureParam(settings.cShader, 0, "Source", cameraTex);
            cmd.SetComputeTextureParam(settings.cShader, 0, "Warp", settings.warp);
            cmd.SetComputeTextureParam(settings.cShader, 0, "Result", handle.Identifier());
            cmd.SetComputeIntParam(settings.cShader, "width", camSettings.width);
            cmd.SetComputeIntParam(settings.cShader, "height", camSettings.height);
            cmd.SetComputeFloatParam(settings.cShader, "time", Time.time);
            
            cmd.DispatchCompute(settings.cShader, 0, camSettings.width/8, camSettings.height/8, 1);
            
            cmd.Blit(handle.Identifier(), cameraTex);
            // don't forget to tell ScriptableRenderContext to actually execute the commands
            context.ExecuteCommandBuffer(cmd);
            // tidy up after ourselves
            cmd.Clear();
            CommandBufferPool.Release(cmd);
        }

        // Cleanup any allocated resources that were created during the execution of this render pass.
        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(handle.id);
        }
    }

    [Serializable]
    public struct WarpSettings
    {
        public RenderPassEvent rPEvent;
        public ComputeShader cShader;
        public Texture2D warp;
    }

    public WarpSettings settings = new WarpSettings();
    CustomRenderPass m_ScriptablePass;

    /// <inheritdoc/>
    public override void Create()
    {
        m_ScriptablePass = new CustomRenderPass(settings);
        // Configures where the render pass should be injected.
        m_ScriptablePass.renderPassEvent = settings.rPEvent;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType == CameraType.Game)
        {
            if (settings.cShader == null) Debug.LogWarning("No Compute Shader provided");
            renderer.EnqueuePass(m_ScriptablePass);
        }
    }
}


