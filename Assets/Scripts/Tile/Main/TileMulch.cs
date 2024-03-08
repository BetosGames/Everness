using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMulch : TileMain, IMinable
{
    public TileMulch()
    {
        tileID = "mulch";
        coreID = 64;
        displayName = "Mulch";
        generatesWithPermaback = new TileDirtCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
