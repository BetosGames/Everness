using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using static FileManager;
using static FileManager.Save;
using UnityEngine.Tilemaps;
using TMPro;
using static Planet;

public class Universe : MonoBehaviour
{
    public static Universe INSTANCE;
    public static bool isSetup = false;
    private WorldRenderer[] worldRenderers;
    [HideInInspector]
    public Planet activePlanet;
    [Header("Lighting")]
    public PlanetLightingRenderer planetLightingRenderer;
    public AnimationCurve lightPropagationCurve;
    public float lightingRenderInterval;

    public static Save generateFromSave;
    public static string universeGenPreset = "Default";
    public static string universeName = "Test";
    public static string universeSeed = "";
    public static bool saveIsOnline = false;
    public static string savePasscode = "";

    [Header("References")]
    public GameObject test;

    private void Awake()
    {
        universeSeed = GenerateRandomSeedString();
    }

    public string GenerateRandomSeedString()
    {
        string chars = "ABCDEFGHIJKLMNOPQRSTUWXYZabcdefghijklmnopqrstuwxyz0123456789";
        var stringChars = new char[15];
        var random = new System.Random();

        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }

        return new String(stringChars);
    }

    public void Start()
    {
        INSTANCE = this;

        if(generateFromSave == null)
        {
            GenerateFromPreset(universeGenPreset);
        }
        else
        {
            GenerateFromSave(generateFromSave);
        }

        worldRenderers = FindObjectsOfType<WorldRenderer>();
    }

    private void GenerateFromPreset(string preset)
    {
        switch (preset)
        {
            default:
                AddPlanetFromScratch("earth", "Earth", typeof(PlanetEarth), universeSeed);
                SetActivePlanet("earth");
                break;
        }

        StartCoroutine(PostInitialPlanetGen());
    }

    public void GenerateFromSave(Save save)
    {
        gameObject.DestroyAllChildrenOfType(typeof(Planet));

        AddPlanetsFromSave(save);
        SetActivePlanet(save.activePlanetID);
        StartCoroutine(PostInitialPlanetGen());
    }

    private IEnumerator PostInitialPlanetGen()
    {
        yield return new WaitUntil(() => activePlanet != null);
        isSetup = true;
    }

    public void Update()
    {
        if(isSetup) ActivePlanetUpdate();
    }

    private HashSet<Chunk> inRangeChunks = new HashSet<Chunk>();
    int tileX;
    int tileY;
    private Vector2Int rendererChunkCoords;
    private Vector2Int targetChunkCoords;
    private Chunk chunkAtCoords;
    private Chunk chunkToLoad;
    private bool loadingChunks;
    private float lightingRenderTimer;

    private Coroutine chunkLoadTimeout;

    public void ActivePlanetUpdate()
    {
        if (activePlanet == null) return;

        foreach(WorldRenderer worldRenderer in worldRenderers)
        {
            if (!loadingChunks && worldRenderer.renderTimer <= 0)
            {
                inRangeChunks = new HashSet<Chunk>();

                worldRenderer.renderTimer += worldRenderer.renderInterval;

                tileX = (int)Mathf.Round(worldRenderer.transform.position.x);
                tileY = (int)Mathf.Round(worldRenderer.transform.position.y);

                rendererChunkCoords.x = tileX - tileX.mod(Planet.chunkSize);
                rendererChunkCoords.y = tileY - tileY.mod(Planet.chunkSize);

                int chunkDistanceX = Mathf.CeilToInt(worldRenderer.renderDistance.x / Planet.chunkSize);
                int chunkDistanceY = Mathf.CeilToInt(worldRenderer.renderDistance.y / Planet.chunkSize);

                if (!loadingChunks)
                {
                    for (int x = -chunkDistanceX; x < chunkDistanceX; x++)
                    {
                        for (int y = -chunkDistanceY; y < chunkDistanceY; y++)
                        {
                            targetChunkCoords.x = rendererChunkCoords.x + x * Planet.chunkSize;
                            targetChunkCoords.y = rendererChunkCoords.y + y * Planet.chunkSize;

                            chunkAtCoords = activePlanet.GetChunk(targetChunkCoords);

                            if (chunkAtCoords == null)
                            {
                                chunkToLoad = new Chunk(activePlanet, targetChunkCoords);
                                activePlanet.allChunks.Add(chunkToLoad);
                            }
                            else
                            {
                                chunkToLoad = chunkAtCoords;
                            }

                            if (chunkToLoad != null)
                            {
                                inRangeChunks.Add(chunkToLoad);
                            }
                        }
                    }
                }

                StartCoroutine(LoadInRangeChunks());

                foreach (Chunk chunk in activePlanet.loadedChunks)
                {
                    if (!inRangeChunks.Contains(chunk) && !chunk.IsGenerating() && chunk.IsLoaded())
                    {
                        chunk.Unload();
                    }
                }

                activePlanet.loadedChunks.RemoveWhere(chunk => !chunk.IsGenerating() && !chunk.IsLoaded());
            }

            worldRenderer.renderTimer -= Time.deltaTime;
        }

        foreach (Chunk chunk in inRangeChunks)
        {
            if (chunk.IsGenerated())
            {
                chunk.UpdateNeighborsentLighting();
            }
        }

        if(lightingRenderTimer <= 0)
        {
            lightingRenderTimer += lightingRenderInterval;
            planetLightingRenderer.ApplyLighting(activePlanet);
        }

        lightingRenderTimer -= Time.deltaTime;
    }

    private IEnumerator LoadInRangeChunks()
    {
        loadingChunks = true;

        foreach(Chunk chunk in inRangeChunks)
        {
            if (!chunk.IsGenerating() && !chunk.IsLoaded())
            {
                chunk.Load();
                yield return new WaitUntil(() => chunk.IsLoaded());
                activePlanet.loadedChunks.Add(chunk);
            }
        }

        loadingChunks = false;
    }

    public void SaveUniverse()
    {
        Save newSave = new Save(universeName, universeSeed, GetComponentsInChildren<Planet>(), activePlanet.planetID, null);
        FileManager.DeleteSaveFile(newSave.saveName);
        FileManager.WriteSaveFile(newSave);
    }

    public void AddPlanetFromScratch(string planetID, string planetDisplayName, System.Type planetType, string seed)
    {
        GameObject newPlanet = new GameObject("Planet ({planetID})");
        newPlanet.transform.parent = transform;
        newPlanet.transform.localPosition = Vector3.zero;
        newPlanet.AddComponent(planetType);
        newPlanet.GetComponent<Planet>().planetID = planetID;
        newPlanet.GetComponent<Planet>().planetDisplayName = planetDisplayName;
        newPlanet.GetComponent<Planet>().Generate(null);
        newPlanet.SetActive(false);
    }

    public void AddPlanetFromSave(Save save, string planetID)
    {
        foreach(PlanetData planetData in save.planetDatas)
        {
            if (planetData.planetID != planetID) return;
            GameObject newPlanet = new GameObject($"Planet ({planetData.planetID})");
            newPlanet.transform.parent = transform;
            newPlanet.AddComponent(planetData.planetType);
            Planet newPlanetComp = GetComponent<Planet>();
            newPlanetComp.planetID = planetID;
            newPlanetComp.planetDisplayName = planetData.planetDisplayName;
            newPlanetComp.Generate(planetData);
            newPlanet.SetActive(false);
        }   
    }

    public void AddPlanetsFromSave(Save save)
    {
        foreach (PlanetData planetData in save.planetDatas)
        {
            GameObject newPlanet = new GameObject($"Planet ({planetData.planetID})");
            newPlanet.transform.parent = transform;
            newPlanet.AddComponent(planetData.planetType);
            newPlanet.GetComponent<Planet>().planetID = planetData.planetID;
            newPlanet.GetComponent<Planet>().planetDisplayName = planetData.planetDisplayName;
            newPlanet.GetComponent<Planet>().Generate(planetData);
            newPlanet.SetActive(false);
        }
    }

    public void SetActivePlanet(string planetID)
    {
        foreach(Planet p in GetComponentsInChildren<Planet>(true))
        {
            if (p.planetID != planetID)
            {
                p.gameObject.SetActive(false);
            }
            else
            {
                p.gameObject.SetActive(true);
                activePlanet = p.gameObject.GetComponent<Planet>();
            }
        }
    }

    public GameObject NewTestObject(Vector3 position, string text, Color color)
    {
        GameObject testObj = GameObject.Instantiate(Universe.INSTANCE.test, position, Quaternion.identity);
        testObj.GetComponent<SpriteRenderer>().color = color;
        testObj.GetComponentInChildren<TMP_Text>().text = text;
        return testObj;
    }

}
