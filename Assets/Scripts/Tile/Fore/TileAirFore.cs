using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAirFore : TileFore, IAir
{
    public TileAirFore()
    {
        tileID = "air_fore";
        displayName = "Fore Air";
        isInvisible = true;
    }
}
