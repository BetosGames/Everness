using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFrostsandCave : TilePermaback
{
    public TileFrostsandCave()
    {
        tileID = "Frostsand_cave";
        coreID = 95;
        displayName = "Frostsand Cave";
    }

    public float mineTime => 1;
}
