using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileStoneCave : TilePermaback
{
    public TileStoneCave()
    {
        tileID = "stone_cave";
        coreID = 41;
        displayName = "Stone Cave";
        opaqueness = 0.2f;
    }

    public float mineTime => 1;
}
