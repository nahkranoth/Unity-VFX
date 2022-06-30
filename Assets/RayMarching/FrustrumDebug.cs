using System;
using UnityEngine;


[ExecuteInEditMode]
public class FrustrumDebug : MonoBehaviour
{
    public Camera cam;

    // Update is called once per frame
    // void Update()
    // {
    //     float camAspect = cam.aspect;
    //     
    //     float camNear = cam.nearClipPlane;
    //     float camFieldView = cam.fieldOfView;
    //     
    //     var frustumHeight = camNear * Mathf.Tan(camFieldView * 0.5f * Mathf.Deg2Rad);
    //     var frustumWidth = frustumHeight * camAspect;
    //
    //     Vector3 topLeft = new Vector3(-frustumWidth, frustumHeight, camNear);
    //     Vector3 topRight = new Vector3(frustumWidth, frustumHeight, camNear);
    //     Vector3 bottomLeft = new Vector3(-frustumWidth, -frustumHeight, camNear);
    //
    //     Vector3 origin = transform.position + topLeft;
    //
    //     float nearFrustWidthNormalized = (topRight.x / 6) * 2;
    //     float nearFrustHeightNormalized = (bottomLeft.y / 6) * 2;
    //     
    //     for (var i = 0; i <= 6; i++)
    //     {
    //         for (var j = 0; j <= 6; j++)
    //         {
    //             var nearPos = origin + new Vector3(i * nearFrustWidthNormalized, j * nearFrustHeightNormalized, 0);
    //             var dir = nearPos - transform.position;
    //             Debug.DrawRay(nearPos, dir * 1000f, Color.red);
    //         }
    //     }
    //     
    // }
    
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

    private void Update()
    {
        Matrix4x4 mat = GetFrustumCorners(Camera.main);
        var TL = mat.GetRow(0);
        var TR = mat.GetRow(1);
        var BL = mat.GetRow(2);
        var BR = mat.GetRow(3);
        
        var iX = TL - (TL - TR) * 0.5f;
        var iY = TL - (TL - BL) * 0.5f;
        var iR = new Vector4(iX.x, iY.y, iX.z, 0f);
        
        var mTL = Camera.main.cameraToWorldMatrix * iR;
        Debug.DrawRay(Camera.main.transform.position, mTL * 1000f, Color.red);
    }
}
