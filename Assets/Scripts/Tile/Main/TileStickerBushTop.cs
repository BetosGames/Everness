using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileStickerBushTop : TileMain, IMinable
{
    public TileStickerBushTop()
    {
        tileID = "sticker_bush_top";
        displayName = "Sticker Bush";
        isCollidable = false;
        opaqueness = 0;
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
