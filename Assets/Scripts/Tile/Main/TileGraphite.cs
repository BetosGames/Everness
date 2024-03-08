using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGraphite : TileMain, IMinable
{
    public TileGraphite()
    {
        tileID = "graphite";
        coreID = 70;
        displayName = "Graphite";
        generatesWithPermaback = new TileBlackGraniteCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
