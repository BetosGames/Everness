using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSand : TileMain, IMinable
{
    public TileSand()
    {
        tileID = "sand";
        coreID = 4;
        displayName = "Sand";
        generatesWithPermaback = new TileSandCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
