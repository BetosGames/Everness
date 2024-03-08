using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHardpanCave : TilePermaback
{
    public TileHardpanCave()
    {
        tileID = "hardpan_cave";
        coreID = 42;
        displayName = "Hardpan Cave";
        opaqueness = 0.2f;
    }

    public float mineTime => 1;
}
