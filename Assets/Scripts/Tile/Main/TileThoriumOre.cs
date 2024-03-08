using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileThoriumOre : TileMain, IMinable
{
    public TileThoriumOre()
    {
        tileID = "thorium_ore";
        coreID = 36;
        displayName = "Thorium Ore";
        generatesWithPermaback = new TileMantleCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
