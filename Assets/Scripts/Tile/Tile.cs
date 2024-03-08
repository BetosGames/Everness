using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]

public class Tile
{
    //This is the name of the tile without spaces or caps. Used for commands and other code references.
    public string tileID;
    //This is the code number of the tile that connects a GenPrimaryCore value to this tile.
    public int coreID = -1;
    //This is the official name of the tile that will be seen in-game.
    public string displayName;
    //Determines how much light this tile lets through.
    public float opaqueness = 0.65f;
    
    public bool isInvisible = false;

    public Tile()
    {
        
    }

    public Tile copy()
    {
        return (Tile) this.MemberwiseClone();
    }
}
