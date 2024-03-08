using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UIElements;
using static Gen;

public class GenAreas : Gen
{
    public Tile tile;
    public List<string> generateIn = null;
    public float scale;
    public int octaves;
    public float persistance;
    public float lacunarity;
    public float threshold;
    public Falloff noiseFalloff = null;

    public GenAreas(Planet planet, Tile tile, List<string> generateIn, float scale, int octaves, float persistance, float lacunarity, float threshold, Falloff noiseFalloff, Gen followsSeed)
    {
        genUUID = NewGenUUID();
        this.tile = tile;
        this.generateIn = generateIn;
        this.scale = scale;
        this.octaves = octaves;
        this.persistance = persistance;
        this.lacunarity = lacunarity;
        this.threshold = threshold;
        this.noiseFalloff = noiseFalloff;
        this.genSeed = followsSeed == null ? planet.seedRand.Next() : followsSeed.genSeed;
    }

    public override Tile CalculatedMainTile(Chunk chunk, int x, int y)
    {
        int localX = x.mod(Planet.chunkSize);
        int localY = y.mod(Planet.chunkSize);

        float val = map[localX, localY] * noiseFalloff.EvaluateMultiplier(y);

        if (generateIn == null)
        {
            return val >= threshold ? tile.copy() : null;
        }
        else
        {
            if (chunk.GetMainTile(new Vector2Int(localX, localY)) != null)
            {
                return generateIn.Contains(chunk.GetMainTile(new Vector2Int(localX, localY)).tileID) && val >= threshold ? tile.copy() : null;
            }

            else
            {
                return null;
            }
        }
    }

    public override void MakeChunkMap(Chunk chunk, Vector2Int? onlyCoords)
    {
        map = Extra.GenerateNoiseMap(Planet.chunkSize, Planet.chunkSize, genSeed, scale, octaves, persistance, lacunarity, chunk.startCoords, onlyCoords);
    }
}
