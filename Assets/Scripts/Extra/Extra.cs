using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using System;

public static class Extra
{
    //Initial design created by Sebastian Lague, modified for seamless chunk generation by Beto Damian

    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2Int offset, Vector2Int? onlyCoords)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random prng = new System.Random(seed);

        float extraOffsetX = prng.Next(-100000, 100000);
        float extraOffsetY = prng.Next(-100000, 100000);

        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                if (onlyCoords.HasValue)
                {
                    Vector2Int adjustedOnlyCoords = new Vector2Int(onlyCoords.Value.x.mod(Planet.chunkSize), Mathf.Clamp(onlyCoords.Value.y.mod(Planet.chunkSize), 0, mapHeight - 1));
                    x = adjustedOnlyCoords.x;
                    y = adjustedOnlyCoords.y;
                }

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x + offset.x + extraOffsetX) / scale * frequency;
                    float sampleY = (y + offset.y + extraOffsetY) / scale * frequency;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);

                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                noiseMap[x, y] = noiseHeight;

                if (onlyCoords.HasValue)
                {
                    return noiseMap;
                }
            }
        }

        return noiseMap;
    }

    /*public static float[,] GenerateBlotchMap(int mapWidth, int mapHeight, int chunkSize, int seed, float chancePerBlotchInChunk, int tryNumOfBlotches, int minSize, int maxSize)
    {
		float[,] map = new float[mapWidth, mapHeight];
		System.Random prng = new System.Random(seed);

		int maxChunkX = Mathf.CeilToInt((float)mapWidth / (float)chunkSize);
		int maxChunkY = Mathf.CeilToInt((float)mapHeight / (float)chunkSize);

		for (int chunkX = 0; chunkX < maxChunkX; chunkX++)
		{
			for (int chunkY = 0; chunkY < maxChunkY; chunkY++)
			{
				for (int b = 0; b < tryNumOfBlotches; b++)
                {
					if(chancePerBlotchInChunk * 100 >= prng.Next(1, 101))
                    {
						int randX = prng.Next(0, chunkSize) + (chunkSize * chunkX);
						int randY = prng.Next(0, chunkSize) + (chunkSize * chunkY);

						if(randX < mapWidth && randX > -1 && randY < mapHeight && randY > -1)
                        {
							int clumpSize = prng.Next(minSize, maxSize + 1) - 1;

							List<Vector2Int> oreTiles = new List<Vector2Int>();

							oreTiles.Add(new Vector2Int(randX, randY));

							for(int c = 0; c < clumpSize; c++)
                            {
								Vector2Int spreadFrom = oreTiles[prng.Next(0,oreTiles.Count)];

								int horizontal = prng.Next(-1, 1);
								int vertical = prng.Next(-1, 1);

								while(horizontal == 0 && vertical == 0)
                                {
                                    horizontal = prng.Next(-1, 1);
                                    vertical = prng.Next(-1, 1);
                                }

								Vector2Int newOre = spreadFrom + new Vector2Int(horizontal, vertical);

                                if (newOre.x >= mapWidth || newOre.x < 0 || newOre.y >= mapHeight || newOre.y < 0 || oreTiles.Contains(newOre))
                                {
									c--;
                                }
                                else
                                {
									oreTiles.Add(newOre);
                                }
							}

                            foreach (Vector2Int oreTile in oreTiles)
                            {
								map[oreTile.x, oreTile.y] = 1;
							}
						}
					}
                }
			}
		}

		return map;
    }*/

    public static List<Dictionary<Vector2Int, string>> sortByTouchingTiles(Dictionary<Vector2Int, string> tilesInChunk)
    {
		List<Dictionary<Vector2Int, string>> regions = new List<Dictionary<Vector2Int, string>>();

		foreach (KeyValuePair<Vector2Int, string> tile in tilesInChunk)
        {
			Dictionary<Vector2Int, string> matchingRegion = new Dictionary<Vector2Int, string>();
			bool hasMatchingRegion = false;

			if(regions.Count != 0)
            {
				foreach (Dictionary<Vector2Int, string> region in regions)
				{
					if (region.ContainsKey(new Vector2Int(tile.Key.x + 1, tile.Key.y))) hasMatchingRegion = true;
					if (region.ContainsKey(new Vector2Int(tile.Key.x - 1, tile.Key.y))) hasMatchingRegion = true;
					if (region.ContainsKey(new Vector2Int(tile.Key.x, tile.Key.y + 1))) hasMatchingRegion = true;
					if (region.ContainsKey(new Vector2Int(tile.Key.x, tile.Key.y - 1))) hasMatchingRegion = true;

					if (hasMatchingRegion)
                    {
						matchingRegion = region;
						break;
                    }

				}
			}
            if (!hasMatchingRegion)
            {
				matchingRegion.Add(tile.Key, tile.Value);
				regions.Add(matchingRegion);
				
            }
            else
            {
				matchingRegion.Add(tile.Key, tile.Value);
			}
        }

		return regions;
    }

	public static GameObject[] GetObjectsWithTagInRange(this GameObject fromObject, string tag, float range)
    {
		HashSet<GameObject> go_collection = new HashSet<GameObject>();

		foreach (GameObject go in GameObject.FindGameObjectsWithTag(tag))
        {
			if(Vector3.Distance(fromObject.transform.position, go.transform.position) <= range)
            {
				go_collection.Add(go);
			}
        }

		GameObject[] go_collection_array = new GameObject[go_collection.Count];
		go_collection.CopyTo(go_collection_array);
		return go_collection_array;
    }

	public static T[] GetObjectsOfTypeInRange<T>(this GameObject fromObject, float range, bool includeInactive = true) where T : UnityEngine.Component
	{
		HashSet<T> t_collection = new HashSet<T>();

		foreach (T t in GameObject.FindObjectsOfType<T>(includeInactive))
		{
			if (Vector3.Distance(fromObject.transform.position, t.transform.position) <= range)
			{
				t_collection.Add(t);
			}
		}

		T[] t_collection_array = new T[t_collection.Count];
		t_collection.CopyTo(t_collection_array);
		return t_collection_array;
	}

	//Thanks Jessy!
	public static float Remap(this float value, float from1, float to1, float from2, float to2)
	{
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}

    //Thanks hardlydifficult!
    public static bool Includes(
          this LayerMask mask,
          int layer)
    {
        return (mask.value & 1 << layer) > 0;
    }

    //Thanks, DevBeeBee!
    public static string AddColor(this string text, Color col) => $"<color={ColorHexFromUnityColor(col)}>{text}</color>";
	public static string ColorHexFromUnityColor(this Color unityColor) => $"#{ColorUtility.ToHtmlStringRGBA(unityColor)}";

    public static string RandomSeed()
    {
        return UnityEngine.Random.Range(1, 100000000).ToString();
    }

    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>

    /*public static SerializableDictionary<SerializableVector2, Tile> Serialize(this Dictionary<Vector2Int, Tile> tileData)
    {
		SerializableDictionary<SerializableVector2, Tile> serialized = new SerializableDictionary<SerializableVector2, Tile>();

        foreach (KeyValuePair<Vector2Int, Tile> data in tileData)
        {
			serialized.Add(new SerializableVector2(data.Key.x, data.Key.y), data.Value);
        }

		return serialized;
    }

	public static Dictionary<Vector2Int, Tile> Deserialize(this SerializableDictionary<SerializableVector2, Tile> tileData)
    {
        Dictionary<Vector2Int, Tile> deserialized = new Dictionary<Vector2Int, Tile>();

        foreach (KeyValuePair<SerializableVector2, Tile> data in tileData)
        {
            deserialized.Add(new Vector2Int((int)data.Key.x, (int)data.Key.y), data.Value);
        }

        return deserialized;
    }*/

	public static void DestroyAllChildrenOfType(this GameObject gameObject, System.Type type)
    {
		foreach(Transform t in gameObject.GetComponentsInChildren<Transform>(true))
        {
			if (t.GetComponent(type) && t.gameObject != gameObject) GameObject.Destroy(gameObject);
        }
    }

    public static void DestroyAllChildren(this GameObject gameObject)
    {
        foreach (Transform t in gameObject.GetComponentsInChildren<Transform>(true))
        {
            if(t.gameObject != gameObject) GameObject.Destroy(t.gameObject);
        }
    }

    public static Mesh WeldVertices(this Mesh aMesh, float aMaxDelta = 0.01f)
    {
        var verts = aMesh.vertices;
        var normals = aMesh.normals;
        var uvs = aMesh.uv;
        Dictionary<Vector3, int> duplicateHashTable = new Dictionary<Vector3, int>();
        List<int> newVerts = new List<int>();
        int[] map = new int[verts.Length];

        //create mapping and find duplicates, dictionaries are like hashtables, mean fast
        for (int i = 0; i < verts.Length; i++)
        {
            if (!duplicateHashTable.ContainsKey(verts[i]))
            {
                duplicateHashTable.Add(verts[i], newVerts.Count);
                map[i] = newVerts.Count;
                newVerts.Add(i);
            }
            else
            {
                map[i] = duplicateHashTable[verts[i]];
            }
        }

        // create new vertices
        var verts2 = new Vector3[newVerts.Count];
        var normals2 = new Vector3[newVerts.Count];
        var uvs2 = new Vector2[newVerts.Count];
        for (int i = 0; i < newVerts.Count; i++)
        {
            int a = newVerts[i];
            verts2[i] = verts[a];
            normals2[i] = normals[a];
            uvs2[i] = uvs[a];
        }
        // map the triangle to the new vertices
        var tris = aMesh.triangles;
        for (int i = 0; i < tris.Length; i++)
        {
            tris[i] = map[tris[i]];
        }
        aMesh.triangles = tris;
        aMesh.vertices = verts2;
        aMesh.normals = normals2;
        aMesh.uv = uvs2;

        aMesh.RecalculateBounds();
        aMesh.RecalculateNormals();

        return aMesh;
    }

    public class CustomTileBase : TileBase
    {
		public Sprite sprite;
		public bool collidable;

        // Docs: https://docs.unity3d.com/ScriptReference/Tilemaps.TileBase.GetTileData.html

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(position, tilemap, ref tileData);
			tileData.sprite = sprite;
            tileData.colliderType = collidable ? UnityEngine.Tilemaps.Tile.ColliderType.Sprite : UnityEngine.Tilemaps.Tile.ColliderType.None;
        }
    }

    public static int ToInt(this Vector2Int vector, int sideXSize)
    {
        return vector.x + (vector.y * sideXSize);
    }

    public static Vector2Int ToVector2Int(this int integer, int sideSize)
    {
        return new Vector2Int(integer.mod(sideSize), Mathf.FloorToInt(integer / sideSize));
    }

    public static bool AlmostEqual(this float given, float value, float error)
    {
        return given >= value - error && given <= value + error;
    }

    //By ShreevatsaR on Stack Overflow

    public static int mod(this int x, int m)
    {
        return (x % m + m) % m;
    }
}