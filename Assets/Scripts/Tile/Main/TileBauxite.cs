using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBauxite : TileMain, IMinable
{
    public TileBauxite()
    {
        tileID = "bauxite";
        coreID = 79;
        displayName = "Bauxite";
        generatesWithPermaback = new TileAdobeCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
