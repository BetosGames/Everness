using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Structure
{
    public const char notIncludedChar = '-';
    public int lengthOfBiggestDimensionEver;
    private int rootChunkCheckRange;
    private bool calculatedRootChunkCheckRange;
    public string structureID;
    public Dictionary<char, Tile> legend;
    public Vector2Int origin;

    public virtual string[] Pattern(System.Random random)
    {
        return null;
    }

    public HashSet<VectorTileMapping> GenerateNewLocalMappings(int seed)
    {
        HashSet<VectorTileMapping> newLocalMappings = new HashSet<VectorTileMapping>();

        System.Random newRandom = new System.Random(seed);

        string[] structurePattern = Pattern(newRandom);

        for (int y = 0; y < structurePattern.Length; y++)
        {
            for (int x = 0; x < structurePattern[0].Length; x++)
            {
                if (structurePattern[y][x] != Structure.notIncludedChar) newLocalMappings.Add(new Structure.VectorTileMapping(new Vector2Int(x - origin.x, (structurePattern.Length - 1 - y) - origin.y), legend[structurePattern[y][x]]));
            }
        }

        return newLocalMappings;
    }

    public int GetRootChunkCheckRange()
    {
        if (!calculatedRootChunkCheckRange)
        {
            rootChunkCheckRange = Mathf.CeilToInt(lengthOfBiggestDimensionEver / Planet.chunkSize) + 1;
            calculatedRootChunkCheckRange = true;
        }

        return rootChunkCheckRange;
    }

    public class VectorTileMapping
    {
        public Vector2Int vector;
        public Tile tile;

        public VectorTileMapping(Vector2Int vector, Tile tile)
        {
            this.vector = vector;
            this.tile = tile;
        }
    }

}
