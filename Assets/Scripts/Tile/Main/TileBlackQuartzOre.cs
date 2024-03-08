using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBlackQuartzOre : TileMain, IMinable
{
    public TileBlackQuartzOre()
    {
        tileID = "black_quartz_ore";
        coreID = 71;
        displayName = "Black Quartz Ore";
        generatesWithPermaback = new TileBlackGraniteCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
