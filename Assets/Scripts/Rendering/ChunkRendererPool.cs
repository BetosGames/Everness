using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkRendererPool : MonoBehaviour
{
    public static ChunkRendererPool INSTANCE;
    public int poolSize;
    public ChunkRenderer chunkRendererPrefab;
    private List<ChunkRenderer> pool = new List<ChunkRenderer>();

    private void Awake()
    {
        INSTANCE = this;

        for (int i = 0; i < poolSize; i++)
        {
            pool.Add(Instantiate(chunkRendererPrefab, transform));
        }
    }

    public ChunkRenderer GetNewChunkRenderer(Chunk chunk, Vector2Int chunkStartCoords)
    {
        if (pool.Count == 0)
        {
            throw new System.Exception("Ran out of ChunkRenderers in the pool.");
        }
        ChunkRenderer cr = pool[0];
        pool.RemoveAt(0);
        cr.transform.parent = chunk.planet.transform;
        cr.transform.position = (Vector2) chunkStartCoords - new Vector2(0.5f, 0.5f);
        cr.gameObject.SetActive(true);
        return cr;
    }

    public void RetireChunkRenderer(ChunkRenderer chunkRenderer)
    {
        pool.Add(chunkRenderer);
        chunkRenderer.gameObject.SetActive(false);
        chunkRenderer.transform.parent = transform;
        chunkRenderer.transform.position = Vector2.zero;
    }
}
