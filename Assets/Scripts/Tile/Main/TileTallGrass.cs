using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTallgrass : TileMain, IMinable
{
    public TileTallgrass()
    {
        tileID = "tallgrass";
        displayName = "Tallgrass";
        coreID = 57;
        isCollidable = false;
        opaqueness = 0;
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
