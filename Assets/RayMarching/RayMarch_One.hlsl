

void rX(inout float3 p, float a) {
	float3 q = p;
	float c = cos(a);
	float s = sin(a);
	p.y = c * q.y - s * q.z;
	p.z = s * q.y + c * q.z;
}

void rY(inout float3 p, float a) {
	float3 q = p;
	float c = cos(a);
	float s = sin(a);
	p.x = c * q.x + s * q.z;
	p.z = -s * q.x + c * q.z;
}

void rZ(inout float3 p, float a) {
	float3 q = p;
	float c = cos(a);
	float s = sin(a);
	p.x = c * q.x - s * q.y;
	p.y = s * q.x + c * q.y;
}


float2x2 Rot(float a)
{
	float s = sin(a);
	float c = cos(a);
	return float2x2(c, -s, s, c);
}

float sdSphere(float3 p, float r) {
	return length(p) - r;
}

float sdTorus(float3 p, float2 t)
{
	float2 q = float2(length(p.xz) - t.x, p.y);
	return length(q) - t.y;
}

float3 raymarchStep(float2 rayPosition, float3 offset, float3 rayDirection, float time)
{
	const int MAXSTEPS = 99;
	const float DISTANCE_THRESHOLD = 0.1;
	const float FAR_CLIP = 100.;

	float dist = 0.;
	
	float3 clr = float3(0,0,0);
	
	for(int i=0;i<MAXSTEPS;i++)
	{
		float3 ray = offset + float3(rayPosition, 1.) * rayDirection * dist;

		//rX(ray,time);
		//rZ(ray,time);
		
		float ns = sdTorus(ray, float2(1, 0.2));
		dist += ns;
		
		if (ns < DISTANCE_THRESHOLD) {
			clr = float3(1,1,1);
			break; 
		}
		if (dist > FAR_CLIP) {
			//miss as we've gone past rear clip
			break;
		}
		
	}
	return clr;
}

#define PITwo 6.2831853076

void RayMarch_float(float2 UV, float3 CameraWorldPos, float4x4 CameraViewMatrix, float CamNearClip, float CamAspect, float CamFOV, float2 ScreenSize, float Time, out float4 clr)
{
	float2 rayCoordinate = UV * 2. - 1;

	float frustumHeight = CamNearClip * tan(CamFOV * 0.5f * (PITwo * 2) / 360.);
	float frustumWidth = frustumHeight * CamAspect;

	float3 topLeft = float3(-frustumWidth, frustumHeight, CamNearClip);
	float3 topRight = float3(frustumWidth, frustumHeight, CamNearClip);
	float3 bottomLeft = float3(-frustumWidth, -frustumHeight, CamNearClip);

	float3 origin = CameraWorldPos + topLeft;

	float nearFrustWidthNormalized = (topRight.x / ScreenSize.x) * 2;
	float nearFrustHeightNormalized = (bottomLeft.y / ScreenSize.y) * 2;

	float3 nearPos = origin + float3(UV.x * nearFrustWidthNormalized, UV.y * nearFrustHeightNormalized, 0);
	float3 rayDirection = nearPos - CameraWorldPos;

	CameraWorldPos = mul(CameraViewMatrix, CameraWorldPos);
	float3 result = raymarchStep(rayCoordinate, CameraWorldPos, rayDirection, Time);
	
	clr = float4(result.x, result.y, result.z, .1f);

}