using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileStickerBushBottom : TileMain, IMinable
{
    public TileStickerBushBottom()
    {
        tileID = "sticker_bush_bottom";
        displayName = "Sticker Bush";
        isCollidable = false;
        opaqueness = 0;
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
