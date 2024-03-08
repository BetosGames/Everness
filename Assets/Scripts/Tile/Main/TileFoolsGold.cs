using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFoolsGold : TileMain, IMinable
{
    public TileFoolsGold()
    {
        tileID = "fools_gold";
        coreID = 88;
        displayName = "Fool's Gold";
        generatesWithPermaback = new TileSandCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
