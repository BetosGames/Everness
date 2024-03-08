using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileIce : TileMain, IMinable
{
    public TileIce()
    {
        tileID = "ice";
        displayName = "Ice";
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
