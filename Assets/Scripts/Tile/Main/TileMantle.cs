using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMantle : TileMain, IMinable
{
    public TileMantle()
    {
        tileID = "mantle";
        coreID = 19;
        displayName = "Mantle";
        generatesWithPermaback = new TileMantleCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
