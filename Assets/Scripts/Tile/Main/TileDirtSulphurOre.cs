using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDirtSulphurOre : TileMain, IMinable
{
    public TileDirtSulphurOre()
    {
        tileID = "dirt_sulphur_ore";
        coreID = 67;
        displayName = "Dirt Sulphur Ore";
        generatesWithPermaback = new TileDirtCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
