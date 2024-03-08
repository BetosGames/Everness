using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Item
{
    public string itemID;
    public string displayName;
    public Texture2D texture;
    public Sprite sprite;

    //Color of the item name in the inventory, white by default.
    public Color inventoryTextColor = Color.white;

    //What you can sell this item for in the market, default is 0.
    public int sellValue = 0;

    public Item()
    {

    }

    public void SetVisual(int scale)
    {
        texture = Registry.GetTexture("item", itemID);
        sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), scale * 12);
    }

    public virtual void OnItemPickup(Player player)
    {
        GUIInventory.INSTANCE.AddItem(this, 1);
    }

    public virtual void OnClickWithItem()
    {

    }

    public Item copy()
    {
        return (Item) this.MemberwiseClone();
    }
}
