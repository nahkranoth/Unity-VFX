using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RayMarchRenderFeature : ScriptableRendererFeature
{
    class CustomRenderPass : ScriptableRenderPass
    {
        private RayMarchSettings settings;
        private RenderTargetIdentifier cameraTex;
        private CameraData cameraData;
        private RenderTextureDescriptor camTextureDescript;
        private RenderTargetIdentifier tempOne;
        private RenderTargetHandle handle;

        public CustomRenderPass(RayMarchSettings settings)
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
            camTextureDescript = renderingData.cameraData.cameraTargetDescriptor;
            cameraTex = renderingData.cameraData.renderer.cameraColorTarget;
            cameraData = renderingData.cameraData;
            camTextureDescript.enableRandomWrite = true;
            cmd.GetTemporaryRT(handle.id, camTextureDescript, FilterMode.Bilinear);
        }

        // Here you can implement the rendering logic.
        // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
        // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
        // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
        
        private Matrix4x4 GetFrustumCorners(Camera cam)
        {
            //TODO: Make sure this code does what you think it should do
            float camFov = cam.fieldOfView;
            float camAspect = cam.aspect;

            Matrix4x4 frustumCorners = Matrix4x4.identity;

            float fovWHalf = camFov * 0.5f;

            float tan_fov = Mathf.Tan(fovWHalf * Mathf.Deg2Rad);

            Vector3 toRight = Vector3.right * tan_fov * camAspect;
            Vector3 toTop = Vector3.up * tan_fov;

            Vector3 topLeft = (-Vector3.forward - toRight + toTop);
            Vector3 topRight = (-Vector3.forward + toRight + toTop);
            Vector3 bottomLeft = (-Vector3.forward - toRight - toTop);
            Vector3 bottomRight = (-Vector3.forward + toRight - toTop);

            frustumCorners.SetRow(0, topLeft);
            frustumCorners.SetRow(1, topRight);
            frustumCorners.SetRow(2, bottomLeft);
            frustumCorners.SetRow(3, bottomRight);

            return frustumCorners;
        }
        
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("Custom Raymarch pass");
            cmd.Clear();
            
            cmd.SetComputeTextureParam(settings.cShader, 0, "Source", cameraTex);
            cmd.SetComputeTextureParam(settings.cShader, 0, "Result", handle.Identifier());
            cmd.SetComputeIntParam(settings.cShader, "width", camTextureDescript.width);
            cmd.SetComputeIntParam(settings.cShader, "height", camTextureDescript.height);
            cmd.SetComputeVectorParam(settings.cShader, "CameraWorldPos", cameraData.camera.transform.position);
            cmd.SetComputeMatrixParam(settings.cShader, "CameraViewMatrix", cameraData.camera.cameraToWorldMatrix);
            cmd.SetComputeMatrixParam(settings.cShader, "FrustumCornersES", GetFrustumCorners(cameraData.camera));
            
            //Do your thing
            
            cmd.DispatchCompute(settings.cShader, 0, camTextureDescript.width/8, camTextureDescript.height/8, 1);
            cmd.Blit(handle.Identifier(), cameraTex);
            
            //execute command buffer
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
    public struct RayMarchSettings
    {
        public RenderPassEvent rPEvent;
        public ComputeShader cShader;
    }

    public RayMarchSettings settings = new RayMarchSettings();
    
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


