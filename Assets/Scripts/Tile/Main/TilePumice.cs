using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePumice : TileMain, IMinable
{
    public TilePumice()
    {
        tileID = "pumice";
        coreID = 86;
        displayName = "Pumice";
        generatesWithPermaback = new TileSandCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
