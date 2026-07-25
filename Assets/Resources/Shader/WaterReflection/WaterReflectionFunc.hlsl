#ifndef WaterReflectionFunc
#define WaterReflectionFunc

void GetUV_float(float2 uv, float texelWidth, float texelHeight, float timeOffset, out float2 Out)
{
    float horizontalOffset = 5 * sin(3.14 * (uv.y + timeOffset) / texelHeight / 10);
    
    Out = (uv.x + horizontalOffset * texelWidth, uv.y);
    return;

}

#endif