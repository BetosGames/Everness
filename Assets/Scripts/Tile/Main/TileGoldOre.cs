using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGoldOre : TileMain, IMinable
{
    public TileGoldOre()
    {
        tileID = "gold_ore";
        coreID = 33;
        displayName = "Gold Ore";
        generatesWithPermaback = new TileStoneCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
