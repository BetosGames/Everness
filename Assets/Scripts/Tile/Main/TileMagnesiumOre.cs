using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMagnesiumOre : TileMain, IMinable
{
    public TileMagnesiumOre()
    {
        tileID = "magnesium_ore";
        coreID = 81;
        displayName = "Magnesium Ore";
        generatesWithPermaback = new TileSandCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
