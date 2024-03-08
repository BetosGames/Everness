using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAirPermaback : TilePermaback, IAir
{
    public TileAirPermaback()
    {
        tileID = "air_permaback";
        displayName = "Permaback Air";
        isInvisible = true;
    }
}
