using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoTileMenuButton : MonoBehaviour
{
    public DemoTileMenu demoTileMenu;
    public Image currentIcon;

    public void OnPressDemoTileMenuButton()
    {
        demoTileMenu.gameObject.SetActive(!demoTileMenu.gameObject.activeInHierarchy);

        Player.INSTANCE.disableControl = false;
        demoTileMenu.clickDemo.disabled = false;
    }
}
