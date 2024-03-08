using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileForestLeaves : TileMain, IMinable
{
    public TileForestLeaves()
    {
        tileID = "forest_leaves";
        displayName = "Forest Leaves";
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
