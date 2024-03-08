using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRuck : TileMain, IMinable
{
    public TileRuck()
    {
        tileID = "ruck";
        coreID = 17;
        displayName = "Ruck";
        generatesWithPermaback = new TileRuckCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
