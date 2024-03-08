using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileNeonOre : TileMain, IMinable
{
    public TileNeonOre()
    {
        tileID = "neon_ore";
        coreID = 82;
        displayName = "Neon Ore";
        generatesWithPermaback = new TileSandCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
