using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileStone : TileMain, IMinable
{
    public TileStone()
    {
        tileID = "stone";
        coreID = 5;
        displayName = "Stone";
        generatesWithPermaback = new TileStoneCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
