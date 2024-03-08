using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplayer2D : MonoBehaviour
{
    public PreviewMapping[] mapPreviewColors;

    private Renderer quadRenderer;

    public void Display(int[,] map)
    {
        if (quadRenderer == null) quadRenderer = GetComponent<Renderer>();

        quadRenderer.enabled = true;
        quadRenderer.transform.localScale = new Vector3((float)map.GetLength(0), (float)map.GetLength(1));
        Texture2D quadTexture = new Texture2D(map.GetLength(0), map.GetLength(1));

        for(int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                Color pixelColor = Color.clear;

                if (map[x, y] != 0)
                {
                    for (int p = 0; p < mapPreviewColors.Length; p++)
                    {
                        if (map[x, y] == mapPreviewColors[p].value)
                        {
                            pixelColor = mapPreviewColors[p].color;
                            break;
                        }
                    }
                }

                quadTexture.SetPixel(x, y, pixelColor);
            }
        }

        quadTexture.filterMode = FilterMode.Point;
        quadTexture.Apply();
        Material mat = new Material(Shader.Find("Unlit/Texture"));
        quadRenderer.sharedMaterial = mat;
        quadRenderer.sharedMaterial.mainTexture = quadTexture;
    }

    [System.Serializable]

    public class PreviewMapping
    {
        public string name = "New Mapping";
        public int value;
        public Color color;
    }
}
