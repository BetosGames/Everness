using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAirMain : TileMain, IAir
{
    public TileAirMain()
    {
        tileID = "air_main";
        displayName = "Main Air";
        isInvisible = true;
        //isCollidable = true;
    }
}
