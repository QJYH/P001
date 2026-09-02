#ifndef EnemyFunc
#define EnemyFunc
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"

void HideInShadow_float(float originalAlpha, float2 selfScreenPos, UnityTexture2D blockTexture2D, UnitySamplerState unitySamplerState, out float Out)
{    
    float r = SAMPLE_TEXTURE2D(blockTexture2D, unitySamplerState, selfScreenPos).r;
    Out = (r < 0.8) ? 0 : originalAlpha;
    return;
}

#endif