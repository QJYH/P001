#ifndef TreasureFunc
#define TreasureFunc
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"

void GetEdgeColor_float(float2 uv, float timeOffset, out float3 Out)
{
    int rotateAnglePerScecond = 90;
    uv -= float2(0.5, 0.5);
    float angle = atan2(uv.y, uv.x);
    angle = ((angle / 3.14159 * 180) + 180);
    angle = fmod((angle + timeOffset * rotateAnglePerScecond), 360);
    
    Out = float3(angle / 360, 0.6, 1);
    return;

}

void IsEdge_float(UnityTexture2D unityTexture2D, UnitySamplerState unitySamplerState, float2 uvCenter, float texelWidth, float texelHeight, out bool Out)
{
    int edgeLength = 2;
    
    float alphaCenter = SAMPLE_TEXTURE2D(unityTexture2D, unitySamplerState, uvCenter).a;
    if (alphaCenter > 0.01)
    {
        Out = false;
        return;
    }
    
    for (int x = -edgeLength; x <= edgeLength; x++)
    {
        for (int y = -edgeLength; y <= edgeLength; y++)
        {
            float2 uv = uvCenter + float2(x * texelWidth, y * texelHeight);
            float alpha = SAMPLE_TEXTURE2D(unityTexture2D, unitySamplerState, uv).a;
            if (alpha > 0.01)
            {
                Out = true;
                return;
            }
        }

    }
    
    Out = false;
    return;
}

#endif