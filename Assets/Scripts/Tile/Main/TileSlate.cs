using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSlate : TileMain, IMinable
{
    public TileSlate()
    {
        tileID = "slate";
        coreID = 75;
        displayName = "Slate";
        generatesWithPermaback = new TileStoneCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
