using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileJasperOre : TileMain, IMinable
{
    public TileJasperOre()
    {
        tileID = "jasper_ore";
        coreID = 72;
        displayName = "Jasper Ore";
        generatesWithPermaback = new TileBlackGraniteCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
