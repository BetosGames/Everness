using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileIcySnow : TileMain, IMinable
{
    public TileIcySnow()
    {
        tileID = "icy_snow";
        coreID = 10;
        displayName = "Icy Snow";
        opaqueness = 0.3f;
        generatesWithPermaback = new TileSnowCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
