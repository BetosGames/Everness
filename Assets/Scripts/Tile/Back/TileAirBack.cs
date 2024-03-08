using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAirBack : TileBack, IAir
{
    public TileAirBack()
    {
        tileID = "air_back";
        displayName = "Back Air";
        isInvisible = true;
    }
}
