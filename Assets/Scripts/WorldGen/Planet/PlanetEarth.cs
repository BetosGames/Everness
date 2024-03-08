using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetEarth : Planet
{
    public override void SetPlanetParameters()
    {

    }

    public override void SetPlanetGenerationSteps()
    {
        generationSteps.Add(new GenPrimary(this));
        generationSteps.Add(new GenStructures((GenPrimary) generationSteps[0], new StructureTree(), 46, 4, new TileShortgrass()));
        generationSteps.Add(new GenStructures((GenPrimary) generationSteps[0], new StructureStickerBush(), 98, null, new TileShortgrass()));
    }
}
