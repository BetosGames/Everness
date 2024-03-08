using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAmethystOre : TileMain, IMinable
{
    public TileAmethystOre()
    {
        tileID = "amethyst_ore";
        coreID = 73;
        displayName = "Amethyst Ore";
        generatesWithPermaback = new TileBlackGraniteCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
