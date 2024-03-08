using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSiliconOre : TileMain, IMinable
{
    public TileSiliconOre()
    {
        tileID = "silicon_ore";
        coreID = 28;
        displayName = "Silicon Ore";
        generatesWithPermaback = new TileRuckCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
