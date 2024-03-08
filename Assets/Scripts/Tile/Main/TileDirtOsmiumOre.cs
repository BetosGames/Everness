using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDirtOsmiumOre : TileMain, IMinable
{
    public TileDirtOsmiumOre()
    {
        tileID = "dirt_osmium_ore";
        coreID = 59;
        displayName = "Dirt Osmium Ore";
        generatesWithPermaback = new TileDirtCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
