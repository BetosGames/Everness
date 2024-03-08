using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using static FileManager.Save;
using static Universe;

public class Planet : MonoBehaviour
{
    public string planetID;
    public string planetDisplayName;

    private int seedHash;
    public System.Random seedRand;

    //Chunk size must be at LEAST lightRange big.
    public const int chunkSize = 16;
    public const int lightRange = 12;

    public List<Gen> generationSteps = new List<Gen>();

    private Dictionary<int, Dictionary<Vector2Int, int[]>> rootMaps = new Dictionary<int, Dictionary<Vector2Int, int[]>>();
    private Dictionary<int, HashSet<Vector2Int>> globalRootPositions = new Dictionary<int, HashSet<Vector2Int>>();
    private Dictionary<Vector2Int, Queue<LightPropagation>> lightPropagationsFromNeighboringChunks = new Dictionary<Vector2Int, Queue<LightPropagation>>();

    public HashSet<Chunk> allChunks = new HashSet<Chunk>();
    public HashSet<Chunk> loadedChunks = new HashSet<Chunk>();

    private HashSet<Entity> entities = new HashSet<Entity>();

    private Tilemap foreTilemap;
    private Tilemap backTilemap;

    public void Generate(PlanetData fromData)
    {
        SetPlanetParameters();
        ApplySeed();
        SetPlanetGenerationSteps();
        PopulateChunks(fromData);
    }

    private void ApplySeed()
    {
        seedHash = universeSeed.GetHashCode();
        seedRand = new System.Random(seedHash);
    }

    public virtual void SetPlanetParameters()
    {
        //length, height
    }

    public virtual void SetPlanetGenerationSteps()
    {
        //generationSteps.Add(...)
        //generationSteps.Add(...)
        //generationSteps.Add(...)
    }

    private void PopulateChunks(PlanetData fromData)
    {
        if(fromData == null)
        {

        }
        else
        {
            //TODO Fix chunk gen process for saved data

            foreach (ChunkData chunkData in fromData.chunkDatas)
            {
                Chunk newChunk = new Chunk(this, new Vector2Int((int)chunkData.position.x, (int)chunkData.position.y));
                //newChunk.SetPreGeneration(chunkData.tileData.Deserialize());
                allChunks.Add(newChunk);
            }
        }
        
    }

    public Chunk GetChunk(Vector2Int coordinates)
    {
        foreach(Chunk chunk in allChunks)
        {
            if (new Vector2Int(coordinates.x - (coordinates.x.mod(chunkSize)), coordinates.y - (coordinates.y.mod(chunkSize))) == chunk.startCoords) return chunk;
        }

        return null;
    }

    public void MineTile(Vector2Int coordinates, Player whoMined)
    {
        //Tile 
    }

    public void ChangeForeTile(Vector2Int coordinates, TileFore tileType)
    {
        Chunk targetChunk = GetChunk(coordinates);
        if (targetChunk == null) return;
        targetChunk.ChangeForeTile(new Vector2Int(Mathf.Abs(coordinates.x.mod(chunkSize)), Mathf.Abs(coordinates.y.mod(chunkSize))), tileType);
    }

    public void ChangeMainTile(Vector2Int coordinates, TileMain tileType, bool updateLighting = true)
    {
        Chunk targetChunk = GetChunk(coordinates);
        if (targetChunk == null) return;
        targetChunk.ChangeMainTile(new Vector2Int(Mathf.Abs(coordinates.x.mod(chunkSize)), Mathf.Abs(coordinates.y.mod(chunkSize))), tileType, updateLighting);
    }

    public void ChangeBackTile(Vector2Int coordinates, TileBack tileType, bool updateLighting = true)
    {
        Chunk targetChunk = GetChunk(coordinates);
        if (targetChunk == null) return;
        targetChunk.ChangeBackTile(new Vector2Int(Mathf.Abs(coordinates.x.mod(chunkSize)), Mathf.Abs(coordinates.y.mod(chunkSize))), tileType, updateLighting);
    }

    public TileFore GetForeTile(Vector2Int coordinates)
    {
        Chunk targetChunk = GetChunk(coordinates);
        return targetChunk.GetForeTile(new Vector2Int(coordinates.x.mod(chunkSize), coordinates.y.mod(chunkSize)));
    }

    public TileMain GetMainTile(Vector2Int coordinates)
    {
        Chunk targetChunk = GetChunk(coordinates);
        return targetChunk.GetMainTile(new Vector2Int(coordinates.x.mod(chunkSize), coordinates.y.mod(chunkSize)));
    }

    public TileBack GetBackTile(Vector2Int coordinates)
    {
        Chunk targetChunk = GetChunk(coordinates);
        return targetChunk.GetBackTile(new Vector2Int(coordinates.x.mod(chunkSize), coordinates.y.mod(chunkSize)));
    }

    public Entity SpawnEntity(string entityID, Vector2 spawnAt)
    {
        GameObject entityPrefab = Registry.INSTANCE.GetPrefabFromEntityID(entityID);

        if(entityPrefab == null)
        {
            return null;
        }

        Entity newEntity = Instantiate(entityPrefab, spawnAt, Quaternion.identity, transform).GetComponent<Entity>();
        newEntity.planet = this;
        entities.Add(newEntity);
        return newEntity;

    }

    public int[] GetRootMap(int genUUID, Vector2Int chunkStartCoords)
    {
        if (!rootMaps.ContainsKey(genUUID)) return null;
        if (!rootMaps[genUUID].ContainsKey(chunkStartCoords)) return null;

        return rootMaps[genUUID][chunkStartCoords];
    }

    public HashSet<Vector2Int> GetGlobalRootPostions(int genUUID)
    {
        if (globalRootPositions.ContainsKey(genUUID)) return globalRootPositions[genUUID];
        return null;
    }

    public void AddToGlobalRootPositions(int genUUID, Vector2Int posiiton)
    {
        if (!globalRootPositions.ContainsKey(genUUID)) globalRootPositions.Add(genUUID, new HashSet<Vector2Int>());
        globalRootPositions[genUUID].Add(posiiton);
    }

    public void SetRootMap(int genUUID, Vector2Int chunkStartCoords, int[] rootMap)
    {
        if (!rootMaps.ContainsKey(genUUID)) rootMaps.Add(genUUID, new Dictionary<Vector2Int, int[]>());
        if (rootMaps[genUUID].ContainsKey(chunkStartCoords))
        {
            rootMaps[genUUID][chunkStartCoords] = rootMap;
        }
        else
        {
            rootMaps[genUUID].Add(chunkStartCoords, rootMap);
        }
    }

    public Queue<LightPropagation> GetLightPropagationsFromNeighboringChunks(Vector2Int startCoords)
    {
        if (lightPropagationsFromNeighboringChunks.ContainsKey(startCoords))
        {
            return lightPropagationsFromNeighboringChunks[startCoords];
        }
        else
        {
            return null;
        }
    }

    public Queue<LightPropagation> GetNeighboringChunkLightPropagationQueue(Vector2Int startCoords)
    {
        if (!lightPropagationsFromNeighboringChunks.ContainsKey(startCoords))
        {
            lightPropagationsFromNeighboringChunks.Add(startCoords, new Queue<LightPropagation>());
        }
        else
        {
            if (lightPropagationsFromNeighboringChunks[startCoords] == null)
            {
                lightPropagationsFromNeighboringChunks[startCoords] = new Queue<LightPropagation>();
            }
        }

        return lightPropagationsFromNeighboringChunks[startCoords];
    }

    public struct LightPropagation
    {
        public LightPropagation(Vector2Int position, int direction, float value)
        {
            this.position = position;
            this.direction = direction;
            this.value = value;
        }

        public Vector2Int position;
        public int direction;
        public float value;
    }
}
