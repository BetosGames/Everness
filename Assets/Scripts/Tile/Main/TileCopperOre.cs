using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCopperOre : TileMain, IMinable
{
    public TileCopperOre()
    {
        tileID = "copper_ore";
        coreID = 30;
        displayName = "Copper Ore";
        generatesWithPermaback = new TileDirtCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
