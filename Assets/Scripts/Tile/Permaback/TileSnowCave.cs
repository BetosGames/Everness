using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSnowCave : TilePermaback
{
    public TileSnowCave()
    {
        tileID = "snow_cave";
        coreID = 50;
        displayName = "Snow Cave";
        opaqueness = 0.2f;
    }

    public float mineTime => 1;
}
