using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDirtKyaniteOre : TileMain, IMinable
{
    public TileDirtKyaniteOre()
    {
        tileID = "dirt_kyanite_ore";
        coreID = 58;
        displayName = "Dirt Kyanite Ore";
        generatesWithPermaback = new TileDirtCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
