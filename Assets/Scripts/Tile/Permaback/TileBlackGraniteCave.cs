using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBlackGraniteCave : TilePermaback
{
    public TileBlackGraniteCave()
    {
        tileID = "black_granite_cave";
        coreID = 69;
        displayName = "Black Granite Cave";
    }

    public float mineTime => 1;
}
