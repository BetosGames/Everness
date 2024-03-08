using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoTileMenu : MonoBehaviour
{
    public ClickDemo clickDemo;
    public GameObject tileButton;
    public Transform tileButtonContent;

    public void Start()
    {
        StartCoroutine(WaitStart());
    }

    public IEnumerator WaitStart()
    {
        yield return new WaitForEndOfFrame();
        foreach(KeyValuePair<string, Tile> registry in Registry.tileRegistry)
        {
            if(registry.Value is TileMain && registry.Value is not IAir)
            {
                GameObject newTileButton = Instantiate(tileButton, tileButtonContent);
                newTileButton.GetComponent<DemoMenuOption>().Setup((TileMain)registry.Value.copy());
            }
        }
    }

    public void OnEnable()
    {
        Player.INSTANCE.disableControl = true;
        clickDemo.disabled = true;
    }
}
