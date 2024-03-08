using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileIronOre : TileMain, IMinable
{
    public TileIronOre()
    {
        tileID = "iron_ore";
        coreID = 25;
        displayName = "Iron Ore";
        generatesWithPermaback = new TileHardpanCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
