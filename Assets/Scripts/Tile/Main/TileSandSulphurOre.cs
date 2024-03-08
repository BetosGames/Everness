using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSandSulphurOre : TileMain, IMinable
{
    public TileSandSulphurOre()
    {
        tileID = "sand_sulphur_ore";
        coreID = 43;
        displayName = "Sand Sulphur Ore";
        generatesWithPermaback = new TileSandCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
