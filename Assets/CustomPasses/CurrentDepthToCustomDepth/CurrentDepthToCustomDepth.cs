using UnityEditor.Rendering.HighDefinition;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;

#if UNITY_EDITOR
[CustomPassDrawer(typeof(CurrentDepthToCustomDepth))]
class CurrentDepthToCustomDepthDrawer : CustomPassDrawer
{
    protected override PassUIFlag commonPassUIFlags => PassUIFlag.Name;
}
#endif

class CurrentDepthToCustomDepth : CustomPass
{
    Material depthToCustomDepth;
    
    [SerializeField, HideInInspector]
    Shader depthToCustomDepthShader;

    readonly int currentCameraDepth = Shader.PropertyToID("_CurrentCameraDepth");

    protected override void Setup(ScriptableRenderContext renderContext, CommandBuffer cmd)
    {
        if (depthToCustomDepthShader == null)
            depthToCustomDepthShader = Shader.Find("Hidden/FullScreen/CurrentDepthToCustomDepth");
        depthToCustomDepth = CoreUtils.CreateEngineMaterial(depthToCustomDepthShader);
    }

    protected override void Execute(CustomPassContext ctx)
    {
        depthToCustomDepth.SetTexture(currentCameraDepth, ctx.cameraDepthBuffer);
        CoreUtils.SetRenderTarget(ctx.cmd, ctx.customDepthBuffer.Value);
        CoreUtils.DrawFullScreen(ctx.cmd, depthToCustomDepth, shaderPassId: 0);
    }

    protected override void Cleanup()
    {
        CoreUtils.Destroy(depthToCustomDepth);
    }
}