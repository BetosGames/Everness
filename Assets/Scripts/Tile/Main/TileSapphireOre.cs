using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSapphireOre : TileMain, IMinable
{
    public TileSapphireOre()
    {
        tileID = "sapphire_ore";
        coreID = 76;
        displayName = "Sapphire Ore";
        generatesWithPermaback = new TileStoneCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
