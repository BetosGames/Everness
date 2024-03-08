using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UIElements;
using static Gen;

public class GenPrimary : Gen
{
    public Planet planet;
    public GenPrimaryCore core;
    public int[,] coreMap;

    public GenPrimary(Planet planet)
    {
        genUUID = NewGenUUID();
        this.planet = planet;
        this.genSeed = planet.seedRand.Next();

        GenPrimaryCore[] allCores = GameObject.FindObjectsOfType<GenPrimaryCore>();

        for(int i = 0; i < allCores.Length; i++)
        {
            if (allCores[i].forPlanetID == planet.planetID)
            {
                core = allCores[i];
                break;
            }
        }

        if (core == null) Debug.Log("Core does not exist for GenPrimary!");
    }

    public override Tile CalculatedMainTile(Chunk chunk, int x, int y)
    {
        x = x.mod(Planet.chunkSize);
        y = y.mod(Planet.chunkSize);

        int sampledCoreID = coreMap[x, y];

        return Registry.NewTileFromCoreID(sampledCoreID);
    }

    public override void MakeChunkMap(Chunk chunk, Vector2Int? onlyCoords)
    {
        core.CreateMapBasedOnEditorSettings(genSeed, Planet.chunkSize, Planet.chunkSize, chunk.startCoords, null, (map) =>
        {
            coreMap = map;
            this.map = new float[0, 0];
        });
    }
}
