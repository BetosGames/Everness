using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSnowKyaniteOre : TileMain, IMinable
{
    public TileSnowKyaniteOre()
    {
        tileID = "snow_kyanite_ore";
        coreID = 51;
        displayName = "Snow Kyanite Ore";
        opaqueness = 0.3f;
        generatesWithPermaback = new TileSnowCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
