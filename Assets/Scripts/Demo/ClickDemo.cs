using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDemo : MonoBehaviour
{
    public Player player;
    public Transform tileCursor;
    public GameObject breakParticles;
    public TileMain tileToBePlaced;

    public float miningRange;
    private bool showRange;

    public float placeCooldown;
    private float placingTimer;

    public bool disabled;

    private Vector2 rawMousePos;

    private Planet planet;

    void Update()
    {
        U_Mining();
    }

    private void U_Mining()
    {
        if(planet == null && Universe.INSTANCE.activePlanet != null)
        {
            planet = Universe.INSTANCE.activePlanet;
        }

        if (disabled)
        {
            tileCursor.gameObject.SetActive(false);

            return;
        }
        else
        {
            tileCursor.gameObject.SetActive(true);
        }

        rawMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 adjustedMousePos;

        if (Vector3.Distance(player.GetCoordinates(), rawMousePos) <= miningRange)
        {
            adjustedMousePos = rawMousePos;
            showRange = false;

        }
        else
        {
            adjustedMousePos = player.GetCoordinates() + (rawMousePos - player.GetCoordinates()).normalized * miningRange;

            if (!showRange)
            {
                //miningRangeVisual.GetComponent<Animator>().SetTrigger("Show");
                showRange = true;
            }

        }

        tileCursor.position = new Vector2(Mathf.Round(adjustedMousePos.x), Mathf.Round(adjustedMousePos.y));

        if (Input.GetMouseButton(0))
        {
            Vector2Int cursorTilePos = new Vector2Int((int)tileCursor.position.x, (int)tileCursor.position.y);

            if (planet.GetMainTile(cursorTilePos) is not IAir)
            {
                GameObject newBreakParticles = GameObject.Instantiate(breakParticles, (Vector3Int)cursorTilePos, Quaternion.identity);
                newBreakParticles.GetComponent<ParticleSystem>().startColor = AverageColorFromTexture(Registry.GetTexture("tile", planet.GetMainTile(cursorTilePos).tileID));

                TileBack replaceWithBack = Universe.INSTANCE.activePlanet.GetMainTile(cursorTilePos).generatesWithBack;

                
                if(replaceWithBack != null)
                {
                    Universe.INSTANCE.activePlanet.ChangeMainTile(cursorTilePos, new TileAirMain(), false);
                    Universe.INSTANCE.activePlanet.ChangeBackTile(cursorTilePos, replaceWithBack);
                }
                else
                {
                    Universe.INSTANCE.activePlanet.ChangeMainTile(cursorTilePos, new TileAirMain(), true);
                }
            }
        }

        else if (Input.GetMouseButton(1))
        {
            if (placingTimer <= 0)
            {
                if(tileToBePlaced != null)
                {
                    Universe.INSTANCE.activePlanet.ChangeMainTile(new Vector2Int((int)tileCursor.position.x, (int)tileCursor.position.y), tileToBePlaced);
                }

                placingTimer = placeCooldown;
            }
        }

        placingTimer -= Time.deltaTime;
    }

    private Color32 AverageColorFromTexture(Texture2D tex)
    {

        Color32[] texColors = tex.GetPixels32();

        int total = texColors.Length;

        float r = 0;
        float g = 0;
        float b = 0;

        for (int i = 0; i < total; i++)
        {

            r += texColors[i].r;

            g += texColors[i].g;

            b += texColors[i].b;

        }

        return new Color32((byte)(r / total), (byte)(g / total), (byte)(b / total), 255);

    }
}
