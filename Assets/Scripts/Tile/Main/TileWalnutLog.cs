using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileWalnutLog : TileMain, IMinable
{
    public TileWalnutLog()
    {
        tileID = "walnut_log";
        displayName = "Walnut Log";
        isCollidable = false;
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
