using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRuckCave : TilePermaback
{
    public TileRuckCave()
    {
        tileID = "ruck_cave";
        coreID = 40;
        displayName = "Ruck Cave";
        opaqueness = 0.2f;
    }

    public float mineTime => 1;
}
