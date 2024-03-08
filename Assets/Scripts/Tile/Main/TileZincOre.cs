using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileZincOre : TileMain, IMinable
{
    public TileZincOre()
    {
        tileID = "zinc_ore";
        coreID = 20;
        displayName = "Zinc Ore";
        generatesWithPermaback = new TileDirtCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
