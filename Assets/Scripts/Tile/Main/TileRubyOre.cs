using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRubyOre : TileMain, IMinable
{
    public TileRubyOre()
    {
        tileID = "ruby_ore";
        coreID = 78;
        displayName = "Ruby Ore";
        generatesWithPermaback = new TileStoneCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
