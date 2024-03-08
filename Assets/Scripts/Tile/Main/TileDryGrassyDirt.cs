using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDryGrassyDirt : TileMain, IMinable
{
    public TileDryGrassyDirt()
    {
        tileID = "dry_grassy_dirt";
        coreID = 12;
        displayName = "Dry Grassy Dirt";
        generatesWithPermaback = new TileDirtCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
