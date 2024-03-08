using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDirt : TileMain, IMinable
{
    public TileDirt()
    {
        tileID = "dirt";
        coreID = 1;
        displayName = "Dirt";
        generatesWithPermaback = new TileDirtCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
