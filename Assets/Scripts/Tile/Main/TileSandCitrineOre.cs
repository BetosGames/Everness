using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSandCitrineOre : TileMain, IMinable
{
    public TileSandCitrineOre()
    {
        tileID = "sand_citrine_ore";
        coreID = 38;
        displayName = "Sand Citrine Ore";
        generatesWithPermaback = new TileSandCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
