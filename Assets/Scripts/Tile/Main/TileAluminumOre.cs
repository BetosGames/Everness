using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAluminumOre : TileMain, IMinable
{
    public TileAluminumOre()
    {
        tileID = "aluminum_ore";
        coreID = 31;
        displayName = "Aluminum Ore";
        generatesWithPermaback = new TileHardpanCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
