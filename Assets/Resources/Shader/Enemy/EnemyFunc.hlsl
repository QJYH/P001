#ifndef EnemyFunc
#define EnemyFunc
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"

void CheckBlock_float(float originalAlpha, float2 selfScreenPos, float2 heroScreenPos, UnityTexture2D blockTexture2D, UnitySamplerState unitySamplerState, out float Out)
{
    int marchTimes = 50;
    float2 distance = heroScreenPos - selfScreenPos;
    
    for (int i = 1; i < marchTimes; i++)
    {
        float2 uv = selfScreenPos + ((float)i / (float)marchTimes) * distance;
        float r = SAMPLE_TEXTURE2D(blockTexture2D, unitySamplerState, uv).r;
        if (r < 0.01f)
        {
            Out = 0;
            return;
        }
    }
    
    Out = originalAlpha;
    return;
}

#endif