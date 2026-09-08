#ifndef SwitchSceneAnimationFunc
#define SwitchSceneAnimationFunc

void GetAlpha_float(float distance, float radius, out float Out)
{
    float alpha = 255;
    if (distance < radius)
    {
        alpha = alpha - (radius - distance);
    }
   
    if (alpha < 0)
        alpha = 0;
    
    alpha = alpha / 255;
    Out = alpha;
    return;
    
}

#endif