using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAncientBrick : TileMain, IMinable
{
    public TileAncientBrick()
    {
        tileID = "ancient_brick";
        displayName = "Ancient Brick";
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
