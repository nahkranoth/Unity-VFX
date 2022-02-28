using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BlitRenderFeature : ScriptableRendererFeature
{
    class BlitRenderPass : ScriptableRenderPass
    {
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get();
            
            
        }
    }
    
    BlitRenderPass m_ScriptablePass;

    public override void Create()
    {
        m_ScriptablePass = new BlitRenderPass();
 
        // Configures where the render pass should be injected.
        m_ScriptablePass.renderPassEvent = RenderPassEvent.AfterRendering;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_ScriptablePass);
    }
}