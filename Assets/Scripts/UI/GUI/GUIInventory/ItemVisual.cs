using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemVisual : MonoBehaviour
{
    public bool isFocused;

    public void SetItem(Item item, int count)
    {
        transform.GetChild(2).GetComponent<TMP_Text>().text = item.displayName;
        transform.GetChild(2).GetComponent<TMP_Text>().color = item.inventoryTextColor;
        transform.GetChild(1).GetComponent<TMP_Text>().text = $"x{count}";
        transform.GetChild(1).GetComponent<TMP_Text>().color = item.inventoryTextColor;
        transform.GetChild(0).GetComponent<Image>().sprite = item.sprite;


    }

    public void Focus()
    {
        isFocused = true;
    }

    public void UnFocus()
    {
        isFocused = false;
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, isFocused ? Vector3.one * 1.5f : Vector3.one, 10 * Time.deltaTime);
    }
}
