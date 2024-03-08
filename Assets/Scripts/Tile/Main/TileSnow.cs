using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSnow : TileMain, IMinable
{
    public TileSnow()
    {
        tileID = "snow";
        coreID = 6;
        displayName = "Snow";
        opaqueness = 0.3f;
        generatesWithPermaback = new TileSnowCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
