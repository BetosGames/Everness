using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DemoMenuOption : MonoBehaviour
{
    public TileMain tile;
    public TMP_Text label;
    public Image icon;

    public void Setup(TileMain tile)
    {
        this.tile = tile;
        label.text = tile.displayName;
        icon.sprite = Registry.GetSprite("tile", tile.tileID);
        //icon.sprite.pixelsPerUnit = 
    }

    public void OnClick()
    {
        FindObjectOfType<ClickDemo>().tileToBePlaced = tile;
        FindObjectOfType<ClickDemo>().disabled = false;
        FindObjectOfType<DemoTileMenu>().gameObject.SetActive(false);
        FindObjectOfType<DemoTileMenuButton>().currentIcon.sprite = icon.sprite;
        FindObjectOfType<DemoTileMenuButton>().currentIcon.enabled = true;
        Player.INSTANCE.disableControl = false;
    }
}
