using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSandZirconOre : TileMain, IMinable
{
    public TileSandZirconOre()
    {
        tileID = "sand_zircon_ore";
        coreID = 44;
        displayName = "Sand Zircon Ore";
        generatesWithPermaback = new TileSandCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
