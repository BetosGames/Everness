using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTorch : TileMain, IGlow
{
    public TileTorch()
    {
        tileID = "torch";
        displayName = "Torch";
        opaqueness = 0;
        isCollidable = false;
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
