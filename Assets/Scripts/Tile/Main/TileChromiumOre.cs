using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileChromiumOre : TileMain, IMinable
{
    public TileChromiumOre()
    {
        tileID = "chromium_ore";
        coreID = 27;
        displayName = "Chromium Ore";
        generatesWithPermaback = new TileRuckCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
