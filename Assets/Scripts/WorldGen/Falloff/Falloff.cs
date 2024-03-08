using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falloff
{
    //0 to maxHeight
    public int? minHeight;
    //minHeight to Planet.height
    public int? maxHeight;
    //0 to 1;
    public float falloffSlope;

    public Falloff(int? minHeight, int? maxHeight)
    {
        this.minHeight = minHeight;
        this.maxHeight = maxHeight;
    }

    public virtual float EvaluateMultiplier(float yValue)
    {
        return (minHeight.HasValue ? yValue >= minHeight.Value : true) && (maxHeight.HasValue ? yValue <= maxHeight.Value : true) ? 1 : 0 ;
    }
}
