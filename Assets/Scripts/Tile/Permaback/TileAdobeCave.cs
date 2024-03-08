using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAdobeCave : TilePermaback
{
    public TileAdobeCave()
    {
        tileID = "adobe_cave";
        coreID = 83;
        displayName = "Adobe Cave";
        opaqueness = 0.2f;
    }

    public float mineTime => 1;
}
