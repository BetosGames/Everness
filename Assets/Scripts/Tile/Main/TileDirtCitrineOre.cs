using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDirtCitrineOre : TileMain, IMinable
{
    public TileDirtCitrineOre()
    {
        tileID = "dirt_citrine_ore";
        coreID = 65;
        displayName = "Dirt Citrine Ore";
        generatesWithPermaback = new TileDirtCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
