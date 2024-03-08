using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePermafrost : TileMain, IMinable
{
    public TilePermafrost()
    {
        tileID = "permafrost";
        coreID = 49;
        displayName = "Permafrost";
        generatesWithPermaback = new TileDirtCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
