using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileClay : TileMain, IMinable
{
    public TileClay()
    {
        tileID = "clay";
        coreID = 3;
        displayName = "Clay";
        generatesWithPermaback = new TileDirtCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
