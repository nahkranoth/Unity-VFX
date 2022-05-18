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

float3 raymarchStep(float2 rayPosition, float3 offset, float3 rotation, float time)
{
	const int MAXSTEPS = 99;
	const float DISTANCE_THRESHOLD = 0.1;
	const float FAR_CLIP = 100.;

	float dist = 0.;
	
	float3 clr = float3(0,0,0);
	
	for(int i=0;i<MAXSTEPS;i++)
	{
		float3 ray = offset + float3(rayPosition, 1.) * dist;

		rX(ray,time);
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

void RayMarch_float(float2 UV, float3 CameraWorldPos, float3 CameraDirection, float4x4 CameraFrustrum, float Time, out float3 clr)
{
	float2 rayCoordinate = UV * 2. - 1;
	clr = raymarchStep(rayCoordinate, CameraWorldPos, CameraDirection, Time);
}