using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEmeraldOre : TileMain, IMinable
{
    public TileEmeraldOre()
    {
        tileID = "emerald_ore";
        coreID = 77;
        displayName = "Emerald Ore";
        generatesWithPermaback = new TileStoneCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
