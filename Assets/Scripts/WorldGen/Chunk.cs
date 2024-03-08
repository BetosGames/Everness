using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Jobs;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.Tilemaps;
using static Chunk;
using static Planet;
using static PlayerSideTrigger;

public class Chunk
{
    public Planet planet;
    public Vector2Int startCoords;
    private bool generating;
    private bool generated;
    private bool loaded;
    private bool changedForeTiles = true;
    private bool changedMainTiles = true;
    private bool changedBackTiles = true;
    private bool changedPermabackTiles = true;

    private TileFore[] foreTileData;
    private TileMain[] mainTileData;
    private TileBack[] backTileData;
    private TilePermaback[] permabackTileData;
    public float[,] lightData;
    public Dictionary<string, GameObject> chunkPieces = new Dictionary<string, GameObject>();

    private TileBase[] foreTileBaseArray;
    private TileBase[] mainTileBaseArray;
    private TileBase[] backTileBaseArray;
    private TileBase[] permabackTileBaseArray;
    private static TileBase[] nullTileBaseArray;

    public ChunkRenderer activeChunkRenderer;

    private int chunkSize = Planet.chunkSize;
    private int lightRange = Planet.lightRange;

    private BoundsInt chunkBI = new BoundsInt(0, 0, 0, Planet.chunkSize, Planet.chunkSize, 1);

    public Chunk(Planet planet, Vector2Int startCoords)
    {
        this.startCoords = startCoords;
        this.planet = planet;

        foreTileBaseArray = new TileBase[chunkSize * chunkSize];
        mainTileBaseArray = new TileBase[chunkSize * chunkSize];
        backTileBaseArray = new TileBase[chunkSize * chunkSize];
        permabackTileBaseArray = new TileBase[chunkSize * chunkSize];

        if(nullTileBaseArray == null) nullTileBaseArray = Enumerable.Repeat<TileBase>(null, chunkSize * chunkSize).ToArray();
    }

    public void Load()
    {
        if (loaded || generating) return;

        if (!generated)
        {
            generating = true;
            planet.StartCoroutine(Generate());
            return;
        }

        activeChunkRenderer = ChunkRendererPool.INSTANCE.GetNewChunkRenderer(this, startCoords);

        if (changedForeTiles)
        {
            for (int i = 0; i < foreTileData.Length; i++)
            {
                if(foreTileData[i] != null)
                {
                    foreTileBaseArray[i] = foreTileData[i].isInvisible || foreTileData[i] is IAir ? null : Registry.GetTileBase(foreTileData[i].tileID);
                }
            }

            changedForeTiles = false;
        }

        if (changedMainTiles)
        {
            for (int i = 0; i < mainTileData.Length; i++)
            {
                mainTileBaseArray[i] = mainTileData[i].isInvisible || mainTileData[i] is IAir ? null : Registry.GetTileBase(mainTileData[i].tileID);
            }

            changedMainTiles = false;
        }


        if (changedBackTiles)
        {
            for (int i = 0; i < backTileData.Length; i++)
            {
                if (backTileData[i] != null)
                {
                    backTileBaseArray[i] = backTileData[i].isInvisible || backTileData[i] is IAir ? null : Registry.GetTileBase(backTileData[i].tileID);
                }
            }

            changedBackTiles = false;
        }

        if (changedPermabackTiles)
        {
            for (int i = 0; i < permabackTileData.Length; i++)
            {
                if (permabackTileData[i] != null)
                {
                    permabackTileBaseArray[i] = permabackTileData[i].isInvisible || permabackTileData[i] is IAir ? null : Registry.GetTileBase(permabackTileData[i].tileID);
                }
            }

            changedBackTiles = false;
        }

        activeChunkRenderer.foreTilemap.SetTilesBlock(chunkBI, foreTileBaseArray);
        activeChunkRenderer.mainTilemap.SetTilesBlock(chunkBI, mainTileBaseArray);
        activeChunkRenderer.backTilemap.SetTilesBlock(chunkBI, backTileBaseArray);
        activeChunkRenderer.permabackTilemap.SetTilesBlock(chunkBI, permabackTileBaseArray);

        loaded = true;
    }

    public void Unload()
    {
        if (!loaded) return;

        loaded = false;

        activeChunkRenderer.foreTilemap.SetTilesBlock(chunkBI, nullTileBaseArray);
        activeChunkRenderer.mainTilemap.SetTilesBlock(chunkBI, nullTileBaseArray);
        activeChunkRenderer.backTilemap.SetTilesBlock(chunkBI, nullTileBaseArray);
        activeChunkRenderer.permabackTilemap.SetTilesBlock(chunkBI, nullTileBaseArray);

        ChunkRendererPool.INSTANCE.RetireChunkRenderer(activeChunkRenderer);
        activeChunkRenderer = null;
    }

    private IEnumerator Generate()
    {
        foreTileData = new TileFore[chunkSize * chunkSize];
        mainTileData = new TileMain[chunkSize * chunkSize];
        backTileData = new TileBack[chunkSize * chunkSize];
        permabackTileData = new TilePermaback[chunkSize * chunkSize];

        for (int g = 0; g < planet.generationSteps.Count; g++)
        {
            Gen chunkGen = planet.generationSteps[g].ChunkCopy(this, null);

            yield return new WaitUntil(() => chunkGen.map != null);

            for (int y = startCoords.y; y < startCoords.y + chunkSize; y++)
            {
                for (int x = startCoords.x; x < startCoords.x + chunkSize; x++)
                {
                    Tile tileToAdd = chunkGen.CalculatedMainTile (this, x, y);

                    int tilePos = new Vector2Int(x.mod(chunkSize), y.mod(chunkSize)).ToInt(chunkSize);

                    if (tileToAdd != null)
                    {
                        if (tileToAdd is TileFore)
                        {
                            TileFore tileFore = (TileFore)tileToAdd;

                            foreTileData[tilePos] = tileFore;
                            mainTileData[tilePos] = tileFore.generatesWithMain;
                            backTileData[tilePos] = tileFore.generatesWithBack;
                            permabackTileData[tilePos] = tileFore.generatesWithPermaback;
                        }
                        else if (tileToAdd is TileMain)
                        {
                            TileMain tileMain = (TileMain)tileToAdd;

                            foreTileData[tilePos] = tileMain.generatesWithFore;
                            mainTileData[tilePos] = tileMain;
                            backTileData[tilePos] = tileMain.generatesWithBack;
                            permabackTileData[tilePos] = tileMain.generatesWithPermaback;
                        }
                        else if (tileToAdd is TileBack)
                        {
                            TileBack tileBack = (TileBack)tileToAdd;

                            foreTileData[tilePos] = tileBack.generatesWithFore;
                            mainTileData[tilePos] = tileBack.generatesWithMain;
                            backTileData[tilePos] = tileBack;
                            permabackTileData[tilePos] = tileBack.generatesWithPermaback;
                        }
                        else if (tileToAdd is TileBack)
                        {
                            TilePermaback tilePermaback = (TilePermaback)tileToAdd;

                            foreTileData[tilePos] = tilePermaback.generatesWithFore;
                            mainTileData[tilePos] = tilePermaback.generatesWithMain;
                            backTileData[tilePos] = tilePermaback.generatesWithBack;
                            permabackTileData[tilePos] = tilePermaback;
                        }

                    }

                    if (g == planet.generationSteps.Count - 1)
                    {
                        if (foreTileData[tilePos] == null) foreTileData[tilePos] = new TileAirFore();
                        if (mainTileData[tilePos] == null) mainTileData[tilePos] = new TileAirMain();
                        if (backTileData[tilePos] == null) backTileData[tilePos] = new TileAirBack();
                        if (permabackTileData[tilePos] == null) permabackTileData[tilePos] = new TileAirPermaback();
                    }
                }
            }
        }

        thisChunkQueue = planet.GetNeighboringChunkLightPropagationQueue(startCoords);
        chunkQueues[0] = planet.GetNeighboringChunkLightPropagationQueue(startCoords + lightPropInitialPositions[0]);
        chunkQueues[1] = planet.GetNeighboringChunkLightPropagationQueue(startCoords + lightPropInitialPositions[1]);
        chunkQueues[2] = planet.GetNeighboringChunkLightPropagationQueue(startCoords + lightPropInitialPositions[2]);
        chunkQueues[3] = planet.GetNeighboringChunkLightPropagationQueue(startCoords + lightPropInitialPositions[3]);

        yield return new WaitUntil(() => thisChunkQueue != null && chunkQueues[0] != null && chunkQueues[1] != null && chunkQueues[2] != null && chunkQueues[3] != null);

        UpdateAllLighting();

        generating = false;
        generated = true;

        Load();
    }

    //Lighting

    private PlanetLightingRenderer planetLightingRenderer;

    private Chunk rightChunk = null;
    private Chunk leftChunk = null;
    private Chunk topChunk = null;
    private Chunk bottomChunk = null;
    private Chunk topRightChunk = null;
    private Chunk topLeftChunk = null;
    private Chunk bottomRightChunk = null;
    private Chunk bottomLeftChunk = null;

    public void UpdateLightingForPosition(int x, int y)
    {
        if (planetLightingRenderer == null) planetLightingRenderer = Universe.INSTANCE.planetLightingRenderer;

        if ((permabackTileData[x + (y * chunkSize)] is IAir || mainTileData[x + (y * chunkSize)] is IGlow) && GetOpaqueness(x, y) == 0)
        {

            UpdateNeighboringPixelsJob job = new UpdateNeighboringPixelsJob(startCoords, x, y, lightRange, -1);
            JobHandle jobHandle = job.Schedule();
            jobHandle.Complete();

            planetLightingRenderer.ApplyLighting(planet, true);
        }
        else
        {
            //Lighting Propagates into right chunk: x + lightRange > chunkSize - 1
            //Lighting Propagates into left chunk: x - lightRange < 0
            //Lighting Propagates into top chunk: y + lightRange > chunkSize - 1
            //Lighting Propagates into bottom chunk: y - lightRange < 0

            if (x - lightRange < 0 || x + lightRange > chunkSize - 1 || y - lightRange < 0 || y + lightRange > chunkSize - 1)
            {
                if (x + lightRange > chunkSize - 1)
                {
                    rightChunk = planet.GetChunk(startCoords + (lightPropDirections[0] * chunkSize));

                    if (y + lightRange > chunkSize - 1)
                    {
                        topChunk = planet.GetChunk(startCoords + (lightPropDirections[2] * chunkSize));
                        topRightChunk = planet.GetChunk(startCoords + ((lightPropDirections[0] + lightPropDirections[2]) * chunkSize));
                    }
                    else if (y - lightRange < 0)
                    {
                        bottomChunk = planet.GetChunk(startCoords + (lightPropDirections[3] * chunkSize));
                        bottomRightChunk = planet.GetChunk(startCoords + ((lightPropDirections[0] + lightPropDirections[3]) * chunkSize));
                    }
                }

                if (x - lightRange < 0)
                {
                    leftChunk = planet.GetChunk(startCoords + (lightPropDirections[1] * chunkSize));

                    if (y + lightRange > chunkSize - 1)
                    {
                        topChunk = planet.GetChunk(startCoords + (lightPropDirections[2] * chunkSize));
                        topLeftChunk = planet.GetChunk(startCoords + ((lightPropDirections[1] + lightPropDirections[2]) * chunkSize));
                    }
                    else if (y - lightRange < 0)
                    {
                        bottomChunk = planet.GetChunk(startCoords + (lightPropDirections[3] * chunkSize));
                        bottomLeftChunk = planet.GetChunk(startCoords + ((lightPropDirections[1] + lightPropDirections[3]) * chunkSize));
                    }

                }

                if (topChunk != null)
                {
                    topChunk.RemovePreviousNeighborsentSides(PropSide.BOTTOM, (topLeftChunk == null ? PropSide.NONE : PropSide.LEFT), (topRightChunk == null ? PropSide.NONE : PropSide.RIGHT));
                    RemovePreviousNeighborsentSides(PropSide.TOP);
                }
                if (bottomChunk != null)
                {
                    bottomChunk.RemovePreviousNeighborsentSides(PropSide.TOP, (bottomLeftChunk == null ? PropSide.NONE : PropSide.LEFT), (bottomRightChunk == null ? PropSide.NONE : PropSide.RIGHT));
                    RemovePreviousNeighborsentSides(PropSide.BOTTOM);
                }
                if (leftChunk != null)
                {
                    leftChunk.RemovePreviousNeighborsentSides(PropSide.RIGHT, (topLeftChunk == null ? PropSide.NONE : PropSide.TOP), (bottomLeftChunk == null ? PropSide.NONE : PropSide.BOTTOM));
                    RemovePreviousNeighborsentSides(PropSide.LEFT);
                }

                if(rightChunk != null)
                {
                    rightChunk.RemovePreviousNeighborsentSides(PropSide.LEFT, (topRightChunk == null ? PropSide.NONE : PropSide.TOP), (bottomRightChunk == null ? PropSide.NONE : PropSide.BOTTOM));
                    RemovePreviousNeighborsentSides(PropSide.RIGHT);
                }

                if (topLeftChunk != null) topLeftChunk.RemovePreviousNeighborsentSides(PropSide.BOTTOM, PropSide.RIGHT);
                if (topRightChunk != null) topRightChunk.RemovePreviousNeighborsentSides(PropSide.BOTTOM, PropSide.LEFT);
                if (bottomLeftChunk != null) bottomLeftChunk.RemovePreviousNeighborsentSides(PropSide.TOP, PropSide.RIGHT);
                if (bottomRightChunk != null) bottomRightChunk.RemovePreviousNeighborsentSides(PropSide.TOP, PropSide.LEFT);

                if (topLeftChunk != null) topLeftChunk.UpdateAllLighting();
                if (topChunk != null) topChunk.UpdateAllLighting();
                if (topRightChunk != null) topRightChunk.UpdateAllLighting();
                if (leftChunk != null) leftChunk.UpdateAllLighting();
                UpdateAllLighting();
                if (rightChunk != null) rightChunk.UpdateAllLighting();
                if (bottomLeftChunk != null) bottomLeftChunk.UpdateAllLighting();
                if (bottomChunk != null) bottomChunk.UpdateAllLighting();
                if (bottomRightChunk != null) bottomRightChunk.UpdateAllLighting();

                if (topChunk != null) topChunk.UpdateNeighborsentLighting(PropSide.ALL);
                if (bottomChunk != null) bottomChunk.UpdateNeighborsentLighting(PropSide.ALL);
                if (leftChunk != null) leftChunk.UpdateNeighborsentLighting(PropSide.ALL);
                if (rightChunk != null) rightChunk.UpdateNeighborsentLighting(PropSide.ALL);
                UpdateNeighborsentLighting(PropSide.ALL);
                if (topLeftChunk != null) topLeftChunk.UpdateNeighborsentLighting(PropSide.ALL);
                if (topRightChunk != null) topRightChunk.UpdateNeighborsentLighting(PropSide.ALL);
                if (bottomLeftChunk != null) bottomLeftChunk.UpdateNeighborsentLighting(PropSide.ALL);
                if (bottomRightChunk != null) bottomRightChunk.UpdateNeighborsentLighting(PropSide.ALL);

            }
            else
            {
                UpdateAllLighting();
                UpdateNeighborsentLighting(PropSide.ALL);
            }

            planet.StartCoroutine(TilePlacementLightingUpdateBuffer());
        }
    }

    public IEnumerator TilePlacementLightingUpdateBuffer()
    {
        yield return new WaitForSeconds(0.06f);
        planetLightingRenderer.ApplyLighting(planet, true);
    }

    public enum PropSide : byte { NONE, TOP, BOTTOM, LEFT, RIGHT, ALL}

    private HashSet<LightPropagation> previousNeighborsentPropagations = new HashSet<LightPropagation>();

    public void RemovePreviousNeighborsentSides(params PropSide[] sidesOfPreviousNeighborsents)
    {
        if (sidesOfPreviousNeighborsents.Length != 0)
        {
            foreach (PropSide propSide in sidesOfPreviousNeighborsents)
            {
                if (propSide == PropSide.NONE) continue;

                switch (propSide)
                {
                    case PropSide.TOP:
                        previousNeighborsentPropagations.RemoveWhere((prop) => prop.direction == 3);
                        break;
                    case PropSide.BOTTOM:
                        previousNeighborsentPropagations.RemoveWhere((prop) => prop.direction == 2);
                        break;
                    case PropSide.LEFT:
                        previousNeighborsentPropagations.RemoveWhere((prop) => prop.direction == 0);
                        break;
                    case PropSide.RIGHT:
                        previousNeighborsentPropagations.RemoveWhere((prop) => prop.direction == 1);
                        break;
                }
            }
        }
    }

    private bool updatingNeighborSent = false;

    public void UpdateNeighborsentLighting(params PropSide[] sidesOfPreviousNeighborsents)
    {
        if(sidesOfPreviousNeighborsents.Length != 0)
        {
            int sideDirection = -1;

            foreach (PropSide propSide in sidesOfPreviousNeighborsents)
            {
                switch (propSide)
                {
                    case PropSide.TOP:
                        sideDirection = 3;
                        break;
                    case PropSide.BOTTOM:
                        sideDirection = 2;
                        break;
                    case PropSide.LEFT:
                        sideDirection = 0;
                        break;
                    case PropSide.RIGHT:
                        sideDirection = 1;
                        break;
                }

                if (sideDirection != -1)
                {
                    Debug.Log("prop goes out of bounds");
                }

                int test = 0;

                foreach (LightPropagation previousNLP in previousNeighborsentPropagations)
                {
                    if (propSide == PropSide.ALL || previousNLP.direction == sideDirection)
                    {
                        UpdateNeighboringPixelsJob job = new UpdateNeighboringPixelsJob(startCoords, previousNLP.position.x, previousNLP.position.y, previousNLP.value, previousNLP.direction);
                        JobHandle jobHandle = job.Schedule();
                        jobHandle.Complete();

                        test++;
                    }
                }
            }
        }
        else
        {
            if (updatingNeighborSent) return;

            while (thisChunkQueue.Count > 0)
            {
                updatingNeighborSent = true;

                LightPropagation neighborsentPropagation = thisChunkQueue.Dequeue();
                previousNeighborsentPropagations.RemoveWhere((prop) => prop.position == neighborsentPropagation.position);
                previousNeighborsentPropagations.Add(neighborsentPropagation);

                UpdateNeighboringPixelsJob job = new UpdateNeighboringPixelsJob(startCoords, neighborsentPropagation.position.x, neighborsentPropagation.position.y, neighborsentPropagation.value, neighborsentPropagation.direction);
                JobHandle jobHandle = job.Schedule();
                jobHandle.Complete();

            }

            updatingNeighborSent = false;

            
        }
    }

    public void UpdateAllLighting()
    {
        lightData = new float[chunkSize, chunkSize];

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                if ((permabackTileData[x + (y * chunkSize)] is IAir || mainTileData[x + (y * chunkSize)] is IGlow) && GetOpaqueness(x, y) == 0)
                {
                    UpdateNeighboringPixelsJob job = new UpdateNeighboringPixelsJob(startCoords, x, y, lightRange, -1);
                    JobHandle jobHandle = job.Schedule();
                    jobHandle.Complete();
                }
            }
        }
    }

    private static Vector2Int[] lightPropDirections = new Vector2Int[] { new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) };
    private static Vector2Int[] lightPropInitialPositions = new Vector2Int[] { new Vector2Int(Planet.chunkSize, 0), new Vector2Int(-Planet.chunkSize, 0), new Vector2Int(0, Planet.chunkSize), new Vector2Int(0, -Planet.chunkSize) };
    public struct UpdateNeighboringPixelsJob : IJob
    {
        public Vector2Int chunkStartCoords;
        public int x;
        public int y;
        public float initialValue;
        public int specificDirection;

        public UpdateNeighboringPixelsJob(Vector2Int chunkStartCoords, int x, int y, float initialValue, int specificDirection)
        {
            this.chunkStartCoords = chunkStartCoords;
            this.x = x;
            this.y = y;
            this.initialValue = initialValue;
            this.specificDirection = specificDirection;
        }


        public void Execute()
        {
            Universe.INSTANCE.activePlanet.GetChunk(chunkStartCoords).UpdateNeighboringPixels(x, y, initialValue, specificDirection, -1);
        }
    }


    Queue<LightPropagation> thisChunkQueue;
    Queue<LightPropagation>[] chunkQueues = new Queue<LightPropagation>[4];

    private int[] newBlacklistedDirections = new int[] { 1, 0, 3, 2 };


    public void UpdateNeighboringPixels(int x, int y, float initialValue, int specificDirection, int blacklistDirection)
    {
        initialValue = Mathf.Clamp(initialValue, 0, lightRange);

        if (x >= 0 && x < chunkSize && y >= 0 && y < chunkSize) lightData[x, y] = initialValue;

        if (initialValue > 0)
        {
            if (specificDirection == -1)
            {
                Vector2Int neighborLightPropOrigin = Vector2Int.zero;
                bool boundsCheck = false;

                for (int i = 0; i < 4; i++)
                {
                    if (blacklistDirection != i)
                    {
                        switch (i)
                        {
                            case 0:
                                neighborLightPropOrigin = new Vector2Int(-1, y);
                                boundsCheck = x + 1 >= lightData.GetLength(0);
                                break;
                            case 1:
                                neighborLightPropOrigin = new Vector2Int(chunkSize, y);
                                boundsCheck = x - 1 < 0;
                                break;
                            case 2:
                                neighborLightPropOrigin = new Vector2Int(x, -1);
                                boundsCheck = y + 1 >= lightData.GetLength(1);
                                break;
                            default:
                                neighborLightPropOrigin = new Vector2Int(x, chunkSize);
                                boundsCheck = y - 1 < 0;
                                break;
                        }

                        if (boundsCheck)
                        {
                            chunkQueues[i].Enqueue(new Planet.LightPropagation(neighborLightPropOrigin, i, initialValue));
                        }
                        else if (GetOpaqueness(x + lightPropDirections[i].x, y + lightPropDirections[i].y) != 0 && lightData[x + lightPropDirections[i].x, y + lightPropDirections[i].y] < lightData[x, y])
                        {
                            UpdateNeighboringPixels(x + lightPropDirections[i].x, y + lightPropDirections[i].y, initialValue - 1 - GetOpaqueness(x + lightPropDirections[i].x, y + lightPropDirections[i].y), -1, newBlacklistedDirections[i]);
                        }

                    }
                }
            }
            else
            {
                Vector2Int direction = lightPropDirections[specificDirection];

                if (lightData[x + direction.x, y + direction.y] < initialValue)
                {

                    float opaqueness = GetOpaqueness(x + direction.x, y + direction.y);

                    switch (specificDirection)
                    {
                        case 0:
                            UpdateNeighboringPixels(x + direction.x, y + direction.y, initialValue - 1 - opaqueness, -1, 1);
                            break;
                        case 1:
                            UpdateNeighboringPixels(x + direction.x, y + direction.y, initialValue - 1 - opaqueness, -1, 0);
                            break;
                        case 2:
                            UpdateNeighboringPixels(x + direction.x, y + direction.y, initialValue - 1 - opaqueness, -1, 3);
                            break;
                        default:
                            UpdateNeighboringPixels(x + direction.x, y + direction.y, initialValue - 1 - opaqueness, -1, 2);
                            break;
                    }
                }
            }
        }
    }

    public float GetOpaqueness(int x, int y)
    {
        float finalOpaqueness = 0;

        TileFore foreTile = foreTileData[x + (y * chunkSize)];
        TileMain mainTile = mainTileData[x + (y * chunkSize)];
        TileBack backTile = backTileData[x + (y * chunkSize)];
        TilePermaback permabackTile = permabackTileData[x + (y * chunkSize)];

        if(mainTile is not IGlow)
        {
            if (foreTile is not IAir && finalOpaqueness < foreTile.opaqueness) finalOpaqueness = foreTile.opaqueness;
            if (mainTile is not IAir && finalOpaqueness < mainTile.opaqueness) finalOpaqueness = mainTile.opaqueness;
            if (backTile is not IAir && finalOpaqueness < backTile.opaqueness) finalOpaqueness = backTile.opaqueness;
            if (permabackTile is not IAir && finalOpaqueness < permabackTile.opaqueness) finalOpaqueness = permabackTile.opaqueness;
        }

        return finalOpaqueness;
    }

    public bool IsGenerated()
    {
        return generated;
    }

    public bool IsGenerating()
    {
        return generating;
    }

    public bool IsLoaded()
    {
        return loaded;
    }

    public Vector2 GetCenterCoords()
    {
        return new Vector2((float)startCoords.x + (float)chunkSize / 2, (float)startCoords.y + (float)chunkSize / 2);
    }

    public void ChangeForeTile(Vector2Int coordinates, TileFore changeToTile)
    {
        Tile oldTile = foreTileData[coordinates.ToInt(chunkSize)];

        if (oldTile == null)
        {
            GUIChat.INSTANCE.WriteLine("<red>You cannot build here!");
            return;
        }

        if (changeToTile.tileID != oldTile.tileID)
        {
            if (loaded) activeChunkRenderer.foreTilemap.SetTile((Vector3Int)coordinates, changeToTile.isInvisible ? null : Registry.GetTileBase(changeToTile.tileID));
            changedForeTiles = true;
        }

        foreTileData[coordinates.ToInt(chunkSize)] = (TileFore) changeToTile.copy();
    }

    public void ChangeMainTile(Vector2Int coordinates, TileMain changeToTile, bool updateLighting = true)
    {
        Tile oldTile = mainTileData[coordinates.ToInt(chunkSize)];

        if(oldTile == null)
        {
            GUIChat.INSTANCE.WriteLine("<red>You cannot build here!");
            return;
        }

        mainTileData[coordinates.ToInt(chunkSize)] = (TileMain)changeToTile.copy();

        if (changeToTile.tileID != oldTile.tileID)
        {
            if(loaded) activeChunkRenderer.mainTilemap.SetTile((Vector3Int)coordinates, changeToTile.isInvisible ? null : Registry.GetTileBase(changeToTile.tileID));
            changedMainTiles = true;
            if(updateLighting) UpdateLightingForPosition(coordinates.x, coordinates.y);
        }
    }

    public void ChangeBackTile(Vector2Int coordinates, TileBack changeToTile, bool updateLighting = true)
    {
        TileBack oldBacking = backTileData[coordinates.ToInt(chunkSize)];

        if (oldBacking == null)
        {
            GUIChat.INSTANCE.WriteLine("<red>You cannot build here!");
            return;
        }

        if (changeToTile.tileID != oldBacking.tileID)
        {
            if (loaded) activeChunkRenderer.backTilemap.SetTile((Vector3Int)coordinates, changeToTile.isInvisible ? null : Registry.GetTileBase(changeToTile.tileID));
            changedBackTiles = true;
            if (updateLighting) UpdateLightingForPosition(coordinates.x, coordinates.y);
        }

        backTileData[coordinates.ToInt(chunkSize)] = (TileBack) changeToTile.copy();

    }

    public TileFore GetForeTile(Vector2Int coordinates)
    {
        if (foreTileData == null) return null;
        return foreTileData[coordinates.ToInt(chunkSize)];
    }

    public TileMain GetMainTile(Vector2Int coordinates)
    {
        if (mainTileData == null) return null;
        return mainTileData[coordinates.ToInt(chunkSize)];
    }

    public TileBack GetBackTile(Vector2Int coordinates)
    {
        if (backTileData == null) return null;
        return backTileData[coordinates.ToInt(chunkSize)];
    }

    public Tile[] GetForeTileData()
    {
        return mainTileData;
    }

    public float GetLightLevel(Vector2Int coordinates)
    {
        return lightData[coordinates.x, coordinates.y];
    }
}
