using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDeadGrassyDirt : TileMain, IMinable
{
    public TileDeadGrassyDirt()
    {
        tileID = "dead_grassy_dirt";
        coreID = 61;
        displayName = "Dead Grassy Dirt";
        generatesWithPermaback = new TileDirtCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
