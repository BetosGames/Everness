using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSandCave : TilePermaback
{
    public TileSandCave()
    {
        tileID = "sand_cave";
        coreID = 39;
        displayName = "Sand Cave";
        opaqueness = 0.2f;
    }

    public float mineTime => 1;
}
