#ifndef FogFunc
#define FogFunc

void TryDisappear_float(float3 originalColor, float originalAlpha, float disappearThreshold, float disappearValue, out float3 OutColor, out float OutAlpha)
{
    if (abs(disappearValue - disappearThreshold) < 0.01)
    {
        OutColor = float3(0.6, 0, 0);
        OutAlpha = min(originalAlpha, 1);

    }
    else if (disappearValue > disappearThreshold)
    {
        OutColor = originalColor;
        OutAlpha = originalAlpha;
    }
    else if (disappearValue < disappearThreshold)
    {
        OutColor = originalColor;
        OutAlpha = 0;
    }
    return;
}

#endif