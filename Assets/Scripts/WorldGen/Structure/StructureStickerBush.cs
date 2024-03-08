using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructureStickerBush : Structure
{
    public StructureStickerBush()
    {
        structureID = "sticker_bush";
        legend = new Dictionary<char, Tile>() { { 'B', new TileStickerBushBottom() }, { 'T', new TileStickerBushTop() }};
        origin = new Vector2Int(0, 0);
        lengthOfBiggestDimensionEver = 2;
    }

    public override string[] Pattern(System.Random random)
    {
        return new string[]
        { "T",
          "B"};
    }
}
