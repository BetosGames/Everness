using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLeadOre : TileMain, IMinable
{
    public TileLeadOre()
    {
        tileID = "lead_ore";
        coreID = 96;
        displayName = "Lead Ore";
        generatesWithPermaback = new TileStoneCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
