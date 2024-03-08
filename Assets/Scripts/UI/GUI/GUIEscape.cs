using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIEscape : GUI
{
    public KeyCode openKey;
    public static GUIEscape escapeMenu;

    public GameObject elements;

    void Start()
    {
        escapeMenu = this;
        RegisterGUI();
        elements = transform.GetChild(0).gameObject;
    }

    public override void Always()
    {
        if (Input.GetKeyDown(openKey))
        {
            TryToggleGUI();
        }

        elements.SetActive(IsOpen());
    }

    public override void OnOpen()
    {
        Player.getLocalPlayer().disableControl = true;
    }

    public override void OnClose()
    {
        Player.getLocalPlayer().disableControl = false;
    }

    public void PressSaveAndQuit()
    {

    }

    public void PressQuitAndSave()
    {

    }

    public void PressQuitWithoutSaving()
    {

    }
}
