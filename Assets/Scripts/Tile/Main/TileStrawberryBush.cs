using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileStrawberryBush : TileMain, IMinable
{
    public TileStrawberryBush()
    {
        tileID = "strawberry_bush";
        displayName = "Strawberry Bush";
        coreID = 47;
        isCollidable = false;
        opaqueness = 0;
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
