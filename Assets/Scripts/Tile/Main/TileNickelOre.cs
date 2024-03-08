using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileNickelOre : TileMain, IMinable
{
    public TileNickelOre()
    {
        tileID = "nickel_ore";
        coreID = 26;
        displayName = "Nickel Ore";
        generatesWithPermaback = new TileStoneCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
