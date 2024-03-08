using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class GenStructures : Gen
{
    public GenPrimary genPrimary;
    public Structure structure;
    public int coreID;
    public int? minimumDistance;
    public Tile[] structureMap;
    public Tile replacementIfInvalidDistance;

    public GenStructures(GenPrimary genPrimary, Structure structure, int coreID, int? minimumDistance, Tile replacementIfInvalidDistance)
    {
        genUUID = NewGenUUID();
        this.genPrimary = genPrimary;
        this.structure = structure;
        this.coreID = coreID;
        this.minimumDistance = minimumDistance;
        this.replacementIfInvalidDistance = replacementIfInvalidDistance;
        this.genSeed = genPrimary.planet.seedRand.Next();
    }

    public override Tile CalculatedMainTile(Chunk chunk, int x, int y)
    {
        x = x.mod(Planet.chunkSize);
        y = y.mod(Planet.chunkSize);

        Tile structureMapTile = structureMap[new Vector2Int(x, y).ToInt(Planet.chunkSize)];

        return structureMapTile;
    }

    public override void MakeChunkMap(Chunk chunk, Vector2Int? onlyCoords)
    {
        genPrimary.planet.StartCoroutine(GenerateStructureMap(chunk, (map) =>
        {
            structureMap = map;
            this.map = new float[0, 0];
        }));
    }

    //Makes a Dictionary that contains coordinates and tile values that are valid for the structure generation.

    private IEnumerator GenerateStructureMap(Chunk chunk, System.Action<Tile[]> toDoWithMap)
    {
        Tile[] newStructureMap = new Tile[Planet.chunkSize * Planet.chunkSize];

        System.Random chunkRandom = new System.Random(genSeed + chunk.startCoords.GetHashCode());

        int[,] combinedRootMaps = new int[(structure.GetRootChunkCheckRange() * 2 + 1) * Planet.chunkSize, (structure.GetRootChunkCheckRange() * 2 + 1) * Planet.chunkSize];

        for (int rootX = -structure.GetRootChunkCheckRange(); rootX <= structure.GetRootChunkCheckRange(); rootX++)
        {
            for (int rootY = -structure.GetRootChunkCheckRange(); rootY <= structure.GetRootChunkCheckRange(); rootY++)
            {
                Vector2Int targetChunkStartCoords = chunk.startCoords + new Vector2Int(rootX * Planet.chunkSize, rootY * Planet.chunkSize);

                int[] targetChunkRootMap = chunk.planet.GetRootMap(genUUID, targetChunkStartCoords);

                if (targetChunkRootMap == null)
                {
                    int[] newChunkRootMap = new int[Planet.chunkSize * Planet.chunkSize];

                    int[,] valueMap = null;

                    genPrimary.core.CreateMapBasedOnEditorSettings(genPrimary.genSeed, Planet.chunkSize, Planet.chunkSize, targetChunkStartCoords, this.coreID, (map) =>
                    {
                        valueMap = map;
                    });

                    yield return new WaitUntil(() => valueMap != null);

                    for (int x = 0; x < valueMap.GetLength(0); x++)
                    {
                        for (int y = 0; y < valueMap.GetLength(1); y++)
                        {
                            if (valueMap[x, y] == this.coreID)
                            {
                                
                                Vector2Int currentPoint = new Vector2Int(x, y);
                                bool validRoot = true;

                                if(minimumDistance != null)
                                {
                                    HashSet<Vector2Int> allRootPositionsForThisGen = chunk.planet.GetGlobalRootPostions(genUUID);

                                    if (allRootPositionsForThisGen != null && allRootPositionsForThisGen.Count > 0)
                                    {
                                        foreach (Vector2Int existingRoot in allRootPositionsForThisGen)
                                        {
                                            if (Vector2.Distance(existingRoot, currentPoint + targetChunkStartCoords) <= minimumDistance)
                                            {
                                                validRoot = false;
                                                break;
                                            }
                                        }
                                    }
                                }

                                if (validRoot)
                                {
                                    newChunkRootMap[new Vector2Int(x, y).ToInt(Planet.chunkSize)] = chunkRandom.Next() + 1;
                                    if(minimumDistance != null) chunk.planet.AddToGlobalRootPositions(genUUID, currentPoint + targetChunkStartCoords);
                                }
                                else if (replacementIfInvalidDistance != null)
                                {
                                    newChunkRootMap[new Vector2Int(x, y).ToInt(Planet.chunkSize)] = -10;
                                }
                            }
                        }
                    }

                    chunk.planet.SetRootMap(genUUID, targetChunkStartCoords, newChunkRootMap);
                    targetChunkRootMap = newChunkRootMap;
                }



                for (int j = 0; j < Planet.chunkSize; j++)
                {
                    for (int k = 0; k < Planet.chunkSize; k++)
                    {
                        int root = targetChunkRootMap[new Vector2Int(j, k).ToInt(Planet.chunkSize)];

                        if (root != 0)
                        {
                            combinedRootMaps[((rootX + structure.GetRootChunkCheckRange()) * Planet.chunkSize) + j, ((rootY + structure.GetRootChunkCheckRange()) * Planet.chunkSize) + k] = root;
                        }
                    }
                }
            }
        }

        //chunk.planet.ClearGlobalRootPositions(genUUID);

        int cropPointMin = structure.GetRootChunkCheckRange() * Planet.chunkSize;
        int cropPointMax = (structure.GetRootChunkCheckRange() * Planet.chunkSize) + Planet.chunkSize;

        int size = combinedRootMaps.GetLength(0);

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if (combinedRootMaps[x, y] != 0)
                {
                    if(combinedRootMaps[x, y] == -10)
                    {
                        if (x < cropPointMin || x >= cropPointMax || y < cropPointMin || y >= cropPointMax) continue;
                        newStructureMap[(new Vector2Int(x - (structure.GetRootChunkCheckRange() * Planet.chunkSize), y - (structure.GetRootChunkCheckRange() * Planet.chunkSize))).ToInt(Planet.chunkSize)] = replacementIfInvalidDistance.copy();
                    }
                    else
                    {
                        HashSet<Structure.VectorTileMapping> newLocalMappings = structure.GenerateNewLocalMappings(combinedRootMaps[x, y]);

                        foreach (Structure.VectorTileMapping localMapping in newLocalMappings)
                        {
                            int adjustedX = x + localMapping.vector.x;
                            int adjustedY = y + localMapping.vector.y;

                            if (adjustedX < cropPointMin || adjustedX >= cropPointMax || adjustedY < cropPointMin || adjustedY >= cropPointMax) continue;

                            newStructureMap[(new Vector2Int(adjustedX - (structure.GetRootChunkCheckRange() * Planet.chunkSize), adjustedY - (structure.GetRootChunkCheckRange() * Planet.chunkSize))).ToInt(Planet.chunkSize)] = localMapping.tile;
                        }
                    }
                    
                }
            }
        }

        toDoWithMap(newStructureMap);
    }
}
