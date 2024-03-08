using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gen
{
    //Amount of tiles of offset that the noisemap should be generated in both directions out to compensate for Random and Gradient falloffs.
    public const int falloffFeather = 5;

    public int genSeed = 0;

    public float[,] map = null;

    public int genUUID;

    public virtual Tile CalculatedMainTile(Chunk chunk, int x, int y)
    {
        return null;
    }

    public Gen ChunkCopy(Chunk chunk, Vector2Int? onlyCoords)
    {
        Gen newGen = (Gen) this.MemberwiseClone();
        newGen.MakeChunkMap(chunk, onlyCoords);
        return newGen;
    }

    //onlyCoords is to allow the Gen to create it's noisemap for only one tile. This keeps tile peaking more efficient because it doesn't have to generate an entire map every time.

    public virtual void MakeChunkMap(Chunk chunk, Vector2Int? onlyCoords)
    {

    }

    private static int latestGenUUID = -1;

    public static int NewGenUUID()
    {
        latestGenUUID += 1;
        return latestGenUUID;
    }
}
