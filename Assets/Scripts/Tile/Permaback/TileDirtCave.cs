using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDirtCave : TilePermaback
{
    public TileDirtCave()
    {
        tileID = "dirt_cave";
        coreID = 8;
        displayName = "Dirt Cave";
        opaqueness = 0.1f;
    }

    public float mineTime => 1;
}
