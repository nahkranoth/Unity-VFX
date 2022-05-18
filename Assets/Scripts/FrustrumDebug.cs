using UnityEngine;


[ExecuteInEditMode]
public class FrustrumDebug : MonoBehaviour
{
    public Camera cam;

    // Update is called once per frame
    void Update()
    {
        float camFov = cam.fieldOfView;
        float camAspect = cam.aspect;
        float camNear = cam.nearClipPlane;
        
        var frustumHeight = cam.nearClipPlane * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        var frustumWidth = frustumHeight * camAspect;

        Vector3 topLeft = new Vector3(-frustumWidth, frustumHeight, camNear);
        Vector3 topRight = new Vector3(frustumWidth, frustumHeight, camNear);
        Vector3 bottomLeft = new Vector3(-frustumWidth, -frustumHeight, camNear);
        Vector3 bottomRight = new Vector3(frustumWidth, -frustumHeight, camNear);

        Vector3 origin = transform.position + topLeft;

        float nearFrustWidthNormalized = (topRight.x / 6) * 2;
        float nearFrustHeightNormalized = (bottomLeft.y / 6) * 2;
        
        for (var i = 0; i <= 6; i++)
        {
            for (var j = 0; j <= 6; j++)
            {
                var nearPos = origin + new Vector3(i * nearFrustWidthNormalized, j * nearFrustHeightNormalized, 0);
                var dir = nearPos - transform.position;
                Debug.DrawRay(nearPos, dir, Color.red);
            }
        }
        
    }
}
