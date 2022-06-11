#ifndef MYHLSLINCLUDE_INCLUDED
#define MYHLSLINCLUDE_INCLUDED

void KernelEdgeDetect_float(UnityTexture2D tex, UnitySamplerState State, float3 coord, float SizeT, float Multi, out float3 Color)
{
    float3 clr = float3(1., 1., 1.);
    SizeT *= 0.001;
    float2 texelSize = float2(SizeT, SizeT);
    float2 x = float2(0,0);
    float2 y = float2(0,0);

    x += SAMPLE_TEXTURE2D(tex, State, coord + float2(-texelSize.x, -texelSize.y)) * -1.0;
    x += SAMPLE_TEXTURE2D(tex, State, coord + float2(-texelSize.x,            0)) * -2.0;
    x += SAMPLE_TEXTURE2D(tex, State, coord + float2(-texelSize.x,  texelSize.y)) * -1.0;

    x += SAMPLE_TEXTURE2D(tex, State, coord + float2( texelSize.x, -texelSize.y)) *  1.0;
    x += SAMPLE_TEXTURE2D(tex, State, coord + float2( texelSize.x,            0)) *  2.0;
    x += SAMPLE_TEXTURE2D(tex, State, coord + float2( texelSize.x,  texelSize.y)) *  1.0;

    y += SAMPLE_TEXTURE2D(tex, State, coord + float2(-texelSize.x, -texelSize.y)) * -1.0;
    y += SAMPLE_TEXTURE2D(tex, State, coord + float2(           0, -texelSize.y)) * -2.0;
    y += SAMPLE_TEXTURE2D(tex, State, coord + float2( texelSize.x, -texelSize.y)) * -1.0;

    y += SAMPLE_TEXTURE2D(tex, State, coord + float2(-texelSize.x,  texelSize.y)) *  1.0;
    y += SAMPLE_TEXTURE2D(tex, State, coord + float2(           0,  texelSize.y)) *  2.0;
    y += SAMPLE_TEXTURE2D(tex, State, coord + float2( texelSize.x,  texelSize.y)) *  1.0;

    float res = sqrt(x * x + y * y);
    float3 lines = float3(-1*res, -1*res, -1*res);
    lines *= Multi;

    clr = SAMPLE_TEXTURE2D(tex, State, coord);
    clr += lines;

    Color = clr;
}
#endif //MYHLSLINCLUDE_INCLUDED


bool isfinite(UnityTexture2D g)
{
    return true;
}