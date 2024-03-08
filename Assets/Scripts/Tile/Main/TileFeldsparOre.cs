using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFeldsparOre : TileMain, IMinable
{
    public TileFeldsparOre()
    {
        tileID = "feldspar_ore";
        coreID = 22;
        displayName = "Feldspar Ore";
        generatesWithPermaback = new TileHardpanCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
