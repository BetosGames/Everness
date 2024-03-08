using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLimestone : TileMain, IMinable
{
    public TileLimestone()
    {
        tileID = "limestone";
        coreID = 52;
        displayName = "Limestone";
        generatesWithPermaback = new TileSandCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
