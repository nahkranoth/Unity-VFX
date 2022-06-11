Research into writing Custom Post Processing through a ScriptableRenderPass and a compute shader.

OnCameraSetup creates a renderTexture from the camera input.
The render texture is passed to the compute shader and modified.
The result is Blitted into the camera.

In many examples I saw all kinds of ways to get the camera input. 
Through some personal poking I found that actually the OnCameraSetup passes it allong.
Also note the 

`renderingData.cameraData.cameraType == CameraType.Game`

This checks for the camera type being the game view.
A related project is the RayMarcher.


**Instructions**

To make this work switch to the local rendering pipeline