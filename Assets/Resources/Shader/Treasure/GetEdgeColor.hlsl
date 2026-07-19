#ifndef GetEdgeColor
#define GetEdgeColor
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"

void GetEdgeColor_float(float2 uv, float timeOffset, out float3 Out)
{
    int rotateAnglePerScecond = 90;
    uv -= float2(0.5, 0.5);
    float angle = atan2(uv.y, uv.x);
    angle = ((angle / 3.14159 * 180) + 180);
    angle = fmod((angle + timeOffset * rotateAnglePerScecond), 360);
    
    Out = float3(angle / 360, 1, 1);
    return;

}

#endif