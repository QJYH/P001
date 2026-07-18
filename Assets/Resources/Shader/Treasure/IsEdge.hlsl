#ifndef IsEdge
#define IsEdge
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"

void IsEdge_float(UnityTexture2D mainTexture, UnitySamplerState samplerState, float2 uv, float texelWidth, float texelHeight, out
bool Out)
{
    float2 uvUp = uv + float2(0, 1) / texelHeight;
    float2 uvDown = uv + float2(0, -1) / texelHeight;
    float2 uvLeft = uv + float2(-1, 0) / texelWidth;
    float2 uvRight = uv + float2(1, 0) / texelWidth;

    float alphaUp = SAMPLE_TEXTURE2D(mainTexture, samplerState, uvUp).a;
    float alphaDown = SAMPLE_TEXTURE2D(mainTexture, samplerState, uvDown).a;
    float alphaLeft = SAMPLE_TEXTURE2D(mainTexture, samplerState, uvLeft).a;
    float alphaRight = SAMPLE_TEXTURE2D(mainTexture, samplerState, uvRight).a;
    
    Out = (alphaUp * alphaDown * alphaLeft * alphaRight) == 0;
}

#endif