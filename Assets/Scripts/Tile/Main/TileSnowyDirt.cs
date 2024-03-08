using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSnowyDirt : TileMain, IMinable
{
    public TileSnowyDirt()
    {
        tileID = "snowy_dirt";
        coreID = 55;
        displayName = "Snowy Dirt";
        generatesWithPermaback = new TileDirtCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
