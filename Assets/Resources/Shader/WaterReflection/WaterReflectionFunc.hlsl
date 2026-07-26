#ifndef WaterReflectionFunc
#define WaterReflectionFunc

void GetUV_float(float2 uv, float texelWidth, float texelHeight, float timeOffset, out float2 Out)
{
    float peak = 10;
    float cycle = 20;
    float horizontalOffset = peak * sin(3.14 * (uv.y / texelHeight / cycle + timeOffset));
    
    Out = float2(uv.x + horizontalOffset * texelWidth, uv.y);
    return;
}

#endif