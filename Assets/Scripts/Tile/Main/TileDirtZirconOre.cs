using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDirtZirconOre : TileMain, IMinable
{
    public TileDirtZirconOre()
    {
        tileID = "dirt_zircon_ore";
        coreID = 66;
        displayName = "Dirt Zircon Ore";
        generatesWithPermaback = new TileDirtCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
