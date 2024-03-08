using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FalloffGradient : Falloff
{
    public FalloffGradient(int? minHeight, int? maxHeight, float falloffSlope) : base(minHeight, maxHeight)
    {
        this.minHeight = minHeight;
        this.maxHeight = maxHeight;
        this.falloffSlope = falloffSlope;
    }

    public override float EvaluateMultiplier(float yValue)
    {
        float evaluation = 1;

        if (minHeight.HasValue && yValue <= minHeight.Value)
        {
            evaluation = Mathf.Clamp(1 - (falloffSlope.Remap(0, 1, 0.111f, 0.7f) * (minHeight.Value - yValue)), 0, 1);
        }
        else if (maxHeight.HasValue && yValue >= maxHeight.Value)
        {
            evaluation = Mathf.Clamp(1 - (falloffSlope.Remap(0, 1, 0.111f, 0.7f) * (yValue - maxHeight.Value)), 0, 1);
        }

        return evaluation;
    }
}
