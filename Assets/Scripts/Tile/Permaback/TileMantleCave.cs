using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMantleCave : TilePermaback
{
    public TileMantleCave()
    {
        tileID = "mantle_cave";
        coreID = 0;
        displayName = "Mantle Cave";
        opaqueness = 0.2f;
    }

    public float mineTime => 1;
}
