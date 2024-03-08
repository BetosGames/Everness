using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UIElements;
using static Gen;

public class GenLayer : Gen
{
    public Tile tile;
    public List<string> generateIn = null;
    public float scale;
    public int octaves;
    public float persistance;
    public float lacunarity;
    public int height;
    public float amplitude;
    public bool onlyTop;

    public GenLayer(Planet planet, Tile tile, List<string> generateIn, float scale, int octaves, float persistance, float lacunarity, int height, float amplitude, bool onlyTop, Gen followsSeed)
    {
        genUUID = NewGenUUID();
        this.tile = tile;
        this.generateIn = generateIn;
        this.scale = scale;
        this.octaves = octaves;
        this.persistance = persistance;
        this.lacunarity = lacunarity;
        this.height = height;
        this.amplitude = amplitude;
        this.onlyTop = onlyTop;
        this.genSeed = followsSeed == null ? planet.seedRand.Next() : followsSeed.genSeed;
    }

    public override Tile CalculatedMainTile(Chunk chunk, int x, int y)
    {
        //if (onlyTop && y <= height - (octaves * amplitude * 3f)) return null;

        float val = Mathf.Min(Mathf.FloorToInt(height - (map[x.mod(Planet.chunkSize), 0] * amplitude) + ((octaves - 1) * amplitude * 0.1f) + 1), height);

        if (generateIn == null)
        {
            if (onlyTop)
            {
                return y == val ? tile.copy() : null;
            }
            else
            {
                return y <= val ? tile.copy() : null;
            }
        }
        else
        {
            int localX = x.mod(Planet.chunkSize);
            int localY = y.mod(Planet.chunkSize);

            if (chunk.GetMainTile(new Vector2Int(localX, localY)) != null)
            {
                if (onlyTop)
                {
                    return generateIn.Contains(chunk.GetMainTile(new Vector2Int(localX, localY)).tileID) && y == val ? tile.copy() : null;
                }
                else
                {
                    return generateIn.Contains(chunk.GetMainTile(new Vector2Int(localX, localY)).tileID) && y <= val ? tile.copy() : null;
                }
            }

            else
            {
                return null;
            }
        }
    }

    public override void MakeChunkMap(Chunk chunk, Vector2Int? onlyCoords)
    {
        map = Extra.GenerateNoiseMap(Planet.chunkSize, 1, genSeed, scale, octaves, persistance, lacunarity, new Vector2Int(chunk.startCoords.x, 0), onlyCoords);
    }
}
