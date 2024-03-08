using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePlutoniumOre : TileMain, IMinable
{
    public TilePlutoniumOre()
    {
        tileID = "plutonium_ore";
        coreID = 34;
        displayName = "Plutonium Ore";
        generatesWithPermaback = new TileMantleCave();
    }


    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
