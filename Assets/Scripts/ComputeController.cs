using UnityEngine;

public class ComputeController : MonoBehaviour
{
    [SerializeField] private RenderTexture text;
    [SerializeField] private ComputeShader compute;
    [SerializeField] private Material mat;
    // Start is calleds before the first frame update
    void Start()
    {
        if(text == null){
            text = new RenderTexture(256, 256, 24);
            text.enableRandomWrite = true;
            text.Create();
            mat.mainTexture = text;
        }
        compute.SetTexture(0, "Result", text);
        compute.SetFloat("Resolution", text.width);
        compute.Dispatch(0, text.width/8, text.height/8, 1);
    }

}
