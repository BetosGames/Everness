using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHardpan : TileMain, IMinable
{
    public TileHardpan()
    {
        tileID = "hardpan";
        coreID = 16;
        displayName = "Hardpan";
        generatesWithPermaback = new TileHardpanCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
