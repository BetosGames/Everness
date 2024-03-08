using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileWalnutLeaves : TileMain, IMinable
{
    public TileWalnutLeaves()
    {
        tileID = "walnut_leaves";
        displayName = "Walnut Leaves";
        isCollidable = false;
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
