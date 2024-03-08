using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileUraniumOre : TileMain, IMinable
{
    public TileUraniumOre()
    {
        tileID = "uranium_ore";
        coreID = 35;
        displayName = "Uranium Ore";
        generatesWithPermaback = new TileMantleCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
