using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;
using static Chunk;

public class PlanetLightingRenderer : MonoBehaviour
{
    public SpriteRenderer lightingRenderer;

    private Texture2D currentLightingTexture;

    private int chunkSize = Planet.chunkSize;
    private int lightRange = Planet.lightRange;
    private AnimationCurve propCurve;
    private static Vector2 rendererOffset = new Vector2(-0.5f, -0.5f);
    private Vector2Int lastKnownBottomLeftChunkStartCoords;
    private int previousLoadedChunkCount = -1;

    Chunk[] loadedChunks;

    public void ApplyLighting(Planet planet, bool forceUpdate = false)
    {

        int loadedChunksCount = planet.loadedChunks.Count;

        if (loadedChunksCount == 0) return;

        loadedChunks = new Chunk[loadedChunksCount];
        planet.loadedChunks.CopyTo(loadedChunks);

        Chunk bottomLeftChunk = loadedChunks[0];
        Chunk topRightChunk = loadedChunks[0];
        Chunk targetChunk = null;

        for (int i = 0; i < loadedChunks.Length; i++)
        {
            targetChunk = loadedChunks[i];
            if (targetChunk.startCoords.x <= bottomLeftChunk.startCoords.x && targetChunk.startCoords.y <= bottomLeftChunk.startCoords.y) bottomLeftChunk = targetChunk;
            if (targetChunk.startCoords.x >= topRightChunk.startCoords.x && targetChunk.startCoords.y >= topRightChunk.startCoords.y) topRightChunk = targetChunk;
        }

        lastKnownBottomLeftChunkStartCoords = bottomLeftChunk.startCoords;
        if (propCurve == null) propCurve = Universe.INSTANCE.lightPropagationCurve;

        Texture2D newLightingTexture = new Texture2D(topRightChunk.startCoords.x - bottomLeftChunk.startCoords.x + chunkSize, topRightChunk.startCoords.y - bottomLeftChunk.startCoords.y + chunkSize, TextureFormat.ARGB32, true);
        newLightingTexture.filterMode = FilterMode.Trilinear;
        newLightingTexture.wrapMode = TextureWrapMode.Clamp;

        //Debug.Log($"{lightingTexture.width},{lightingTexture.height}");
        //Debug.Log($"{topRightChunk.startCoords.x},{topRightChunk.startCoords.y}");

        for (int i = 0; i < loadedChunks.Length; i++)
        {
            targetChunk = loadedChunks[i];

            NativeArray<Color32> result = new NativeArray<Color32>(chunkSize * chunkSize, Allocator.TempJob);
            LoadChunkLightingJob job = new LoadChunkLightingJob(targetChunk.startCoords, bottomLeftChunk.startCoords, result);
            JobHandle jobHandle = job.Schedule();
            jobHandle.Complete();

            try
            {
                newLightingTexture.SetPixels32((targetChunk.startCoords.x - bottomLeftChunk.startCoords.x), (targetChunk.startCoords.y - bottomLeftChunk.startCoords.y), chunkSize, chunkSize, job.result.ToArray());

            }
            catch (ArgumentException) { }

            result.Dispose();

        }

        transform.position = ((Vector2)bottomLeftChunk.startCoords) + rendererOffset;
        newLightingTexture.Apply();
        lightingRenderer.sprite = Sprite.Create(newLightingTexture, new Rect(0, 0, newLightingTexture.width, newLightingTexture.height), new Vector2(0, 0), 1);
    }

    public struct LoadChunkLightingJob : IJob
    {
        int chunkSize;
        Vector2Int startCoords;
        Vector2Int bottomLeftChunkCoords;
        public NativeArray<Color32> result;

        public LoadChunkLightingJob(Vector2Int startCoords, Vector2Int bottomLeftChunkCoords, NativeArray<Color32> result)
        {
            this.chunkSize = Planet.chunkSize;
            this.startCoords = startCoords;
            this.bottomLeftChunkCoords = bottomLeftChunkCoords;
            this.result = result;
        }

        public void Execute()
        {
            Chunk targetChunk = Universe.INSTANCE.activePlanet.GetChunk(startCoords);
            AnimationCurve propCurve = Universe.INSTANCE.lightPropagationCurve;
            int lightRange = Planet.lightRange;

            int xyI = 0;

            for (int y = 0; y < chunkSize; y++)
            {
                for (int x = 0; x < chunkSize; x++)
                {
                    float light = targetChunk.lightData[x, y];

                    if (light == 0)
                    {
                        result[xyI] = Color.black;
                    }
                    else
                    {
                        result[xyI] = new Color32(0, 0, 0, (byte)(255 * (1 - propCurve.Evaluate(1 - ((float)(light) / (float)lightRange)))));
                    }

                    xyI++;
                }
            }
        }
    }
}
