using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGravel : TileMain, IMinable
{
    public TileGravel()
    {
        tileID = "gravel";
        coreID = 15;
        displayName = "Gravel";
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
