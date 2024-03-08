using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrassyDirt : TileMain, IMinable
{
    public TileGrassyDirt()
    {
        tileID = "grassy_dirt";
        coreID = 2;
        displayName = "Grassy Dirt";
        generatesWithPermaback = new TileDirtCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
