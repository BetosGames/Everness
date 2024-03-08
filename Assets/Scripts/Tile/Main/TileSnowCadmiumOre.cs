using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSnowCadmiumOre : TileMain, IMinable
{
    public TileSnowCadmiumOre()
    {
        tileID = "snow_cadmium_ore";
        coreID = 54;
        displayName = "Snow Cadmium Ore";
        opaqueness = 0.3f;
        generatesWithPermaback = new TileSnowCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
