using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTitaniumOre : TileMain, IMinable
{
    public TileTitaniumOre()
    {
        tileID = "titanium_ore";
        coreID = 32;
        displayName = "Titanium Ore";
        generatesWithPermaback = new TileRuckCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
