using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBlackGranite : TileMain, IMinable
{
    public TileBlackGranite()
    {
        tileID = "black_granite";
        coreID = 68;
        displayName = "Black Granite";
        generatesWithPermaback = new TileBlackGraniteCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
