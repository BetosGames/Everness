using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDirtCadmiumOre : TileMain, IMinable
{
    public TileDirtCadmiumOre()
    {
        tileID = "dirt_cadmium_ore";
        coreID = 60;
        displayName = "Dirt Cadmium Ore";
        generatesWithPermaback = new TileSnowCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
