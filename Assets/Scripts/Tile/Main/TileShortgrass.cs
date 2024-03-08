using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileShortgrass : TileMain, IMinable
{
    public TileShortgrass()
    {
        tileID = "shortgrass";
        displayName = "Shortgrass";
        coreID = 97;
        isCollidable = false;
        opaqueness = 0;
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
