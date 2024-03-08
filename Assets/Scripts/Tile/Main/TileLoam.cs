using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLoam : TileMain, IMinable
{
    public TileLoam()
    {
        tileID = "loam";
        coreID = 9;
        displayName = "Loam";
        generatesWithPermaback = new TileDirtCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
