using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePotashOre : TileMain, IMinable
{
    public TilePotashOre()
    {
        tileID = "potash_ore";
        coreID = 80;
        displayName = "Potash Ore";
        generatesWithPermaback = new TileAdobeCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
