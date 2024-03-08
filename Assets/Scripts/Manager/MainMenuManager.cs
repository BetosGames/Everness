using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager INSTANCE;

    public GameObject[] submenus;

    void Start()
    {
        INSTANCE = this;
        GoToSubmenu("SM_Title");
    }

    public void GoToSubmenu(string submenuName)
    {
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("MainMenu")) return;

        foreach (GameObject submenu in submenus)
        {
            submenu.SetActive(submenu.name == submenuName);
        }
    }
}
