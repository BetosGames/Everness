using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIInventory : GUI
{
    public static GUIInventory INSTANCE;

    public KeyCode toggleInventory;
    public KeyCode closeInventory;
    public GameObject ItemVisualGO;

    //[itemID, count]
    private List<InventorySlot> inventory = new List<InventorySlot>();
    public int selectedItemIndex;

    private GameObject itemsHolder;
    private GameObject itemFocus;
    private GameObject elements;

    public int focusedIndex = 0;
    private Vector3 originalIHPos;
    private Vector3 oldIHPos;

    public float testRot;
    public float testSpeed;

    private float scrollTimer;

    private void Start()
    {
        INSTANCE = this;
        RegisterGUI();
        elements = transform.GetChild(0).gameObject;
        itemsHolder = elements.transform.GetChild(0).GetChild(0).gameObject;
        itemFocus = elements.transform.GetChild(1).gameObject;
        originalIHPos = itemsHolder.transform.position;
        oldIHPos = originalIHPos;
    }

    public override void Always()
    {
        if (Input.GetKeyDown(toggleInventory) )
        {
            TryToggleGUI();
        }

        if (Input.GetKeyDown(closeInventory))
        {
            TryCloseGUI();
        }

        if (IsOpen())
        {
            elements.gameObject.SetActive(true);
            

            float smallestDistance = Mathf.Infinity;
            ItemVisual focusedItem = null;

            //Vector3 scrollVelocity = (itemsHolder.transform.position - oldIHPos) / Time.deltaTime;
            //oldIHPos = itemsHolder.transform.position;

            foreach (ItemVisual itemVisual in itemsHolder.GetComponentsInChildren<ItemVisual>())
            {
                float thisDistance = Vector3.Distance(itemVisual.transform.position, itemFocus.transform.position);

                if (thisDistance < smallestDistance)
                {
                    smallestDistance = thisDistance;
                    focusedItem = itemVisual;
                }
            }

            foreach(ItemVisual itemVisual in itemsHolder.GetComponentsInChildren<ItemVisual>())
            {
                if(itemVisual == focusedItem)
                {
                    itemVisual.Focus();
                    focusedIndex = itemVisual.transform.GetSiblingIndex();
                }
                else
                {
                    itemVisual.UnFocus();
                }

                //itemVisual.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, scrollVelocity.x * Inventory.INSTANCE.testRot), Inventory.INSTANCE.testSpeed * Time.deltaTime);

            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(itemsHolder.GetComponent<RectTransform>());

            //itemsHolder.transform.position = Vector3.Lerp(itemsHolder.transform.position, originalIHPos + (Vector3.right * focusedIndex * -116), 7 * Time.deltaTime);

        }
        else
        {
            elements.gameObject.SetActive(false);
        }
    }

    public override void OnOpen()
    {
        Player.getLocalPlayer().disableControl = true;

        if (transform.childCount != 0)
        {
            foreach (Transform t in itemsHolder.transform)
            {
                if (t != itemsHolder.transform) Destroy(t.gameObject);
            }
        }

        foreach (InventorySlot slot in inventory)
        {
            ItemVisual newItem = Instantiate(ItemVisualGO, itemsHolder.transform).GetComponent<ItemVisual>();
            newItem.SetItem(slot.item, slot.count);

        }
    }

    public override void OnClose()
    {
        Player.getLocalPlayer().disableControl = false;
    }

    public void AddItem(Item item, int count)
    {
        foreach(InventorySlot slot in inventory)
        {
            if(slot.item.itemID == item.itemID)
            {
                //Item stacks! Data of inventory items is kept. Data of new item is lost.

                slot.count += count;
                return;
            }
        }

        inventory.Add(new InventorySlot(item, count));
    }

    public Item GetSelectedItem()
    {
        return inventory[selectedItemIndex].item;
    }

    public class InventorySlot
    {
        public Item item;
        public int count;

        public InventorySlot(Item item, int count)
        {
            this.item = item;
            this.count = count;
        }
    }
}
