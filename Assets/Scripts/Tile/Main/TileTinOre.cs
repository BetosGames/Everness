using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTinOre : TileMain, IMinable
{
    public TileTinOre()
    {
        tileID = "tin_ore";
        coreID = 21;
        displayName = "Tin Ore";
        generatesWithPermaback = new TileDirtCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
