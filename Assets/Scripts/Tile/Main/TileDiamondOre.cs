using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDiamondOre : TileMain, IMinable
{
    public TileDiamondOre()
    {
        tileID = "diamond_ore";
        coreID = 74;
        displayName = "Diamond Ore";
        generatesWithPermaback = new TileRuckCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
