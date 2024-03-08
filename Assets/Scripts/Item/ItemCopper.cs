using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCopper : Item
{
    public ItemCopper()
    {
        itemID = "copper";
        displayName = "Copper";
        inventoryTextColor = Color.white;
        sellValue = 5;

        SetVisual(1);
    }

    public override void OnItemPickup(Player player)
    {
        base.OnItemPickup(player);
    }
}
