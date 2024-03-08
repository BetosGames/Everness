using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFrostsand : TileMain, IMinable
{
    public TileFrostsand()
    {
        tileID = "frostsand";
        coreID = 90;
        displayName = "Frostsand";
        generatesWithPermaback = new TileFrostsandCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
