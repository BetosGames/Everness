using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSnowOsmiumOre : TileMain, IMinable
{
    public TileSnowOsmiumOre()
    {
        tileID = "snow_osmium_ore";
        coreID = 53;
        displayName = "Snow Osmium Ore";
        opaqueness = 0.3f;
        generatesWithPermaback = new TileSnowCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
