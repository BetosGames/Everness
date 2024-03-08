using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTalc : TileMain, IMinable
{
    public TileTalc()
    {
        tileID = "talc";
        coreID = 37;
        displayName = "Talc";
        generatesWithPermaback = new TileSnowCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
