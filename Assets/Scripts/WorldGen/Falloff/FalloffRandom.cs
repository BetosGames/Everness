using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FalloffRandom : Falloff
{
    private Planet planet;

    public FalloffRandom(Planet planet, int? minHeight, int? maxHeight, float falloffSlope) : base(minHeight, maxHeight)
    {
        this.planet = planet;
        this.minHeight = minHeight;
        this.maxHeight = maxHeight;
        this.falloffSlope = falloffSlope;
    }

    public override float EvaluateMultiplier(float yValue)
    {
        float evaluation = 1;

        if (minHeight.HasValue && yValue <= minHeight.Value)
        {
            float randomRoll = (float)planet.seedRand.Next(1, 101) / 100;
            float randomChance = Mathf.Clamp(1 - falloffSlope * (minHeight.Value - yValue), 0, 1);

            evaluation = randomRoll <= randomChance ? 1 : 0;

        }
        else if (maxHeight.HasValue && yValue >= maxHeight.Value)
        {
            float randomRoll = (float)planet.seedRand.Next(1, 101) / 100;
            float randomChance = Mathf.Clamp(1 - falloffSlope * (yValue - maxHeight.Value), 0, 1);

            evaluation = randomRoll <= randomChance ? 1 : 0;
        }

        return evaluation;
    }
}
