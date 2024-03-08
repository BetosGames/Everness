using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDryIce : TileMain, IMinable
{
    public TileDryIce()
    {
        tileID = "dry_ice";
        coreID = 91;
        displayName = "Dry Ice";
        generatesWithPermaback = new TileFrostsandCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
