using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI : MonoBehaviour
{
    private static List<GUI> guis = new List<GUI>();
    private bool isOpen;

    public void Update()
    {
        Always();
    }

    public void RegisterGUI()
    {
        guis.Add(this);
    }

    public bool TryToggleGUI()
    {
        if (isOpen)
        {
            return TryCloseGUI();
        }
        else
        {
            return TryOpenGUI();
        }
    }

    public bool TryCloseGUI()
    {
        if (Fader.INSTANCE != null && Fader.INSTANCE.IsFading()) return false;

        if(isOpen)
        {
            isOpen = false;
            OnClose();
            return true;
        }

        return false;
    }

    public void TryCloseGUIVoid()
    {
        if (Fader.INSTANCE != null && Fader.INSTANCE.IsFading()) return;

        if (isOpen)
        {
            isOpen = false;
            OnClose();
        }
    }

    public bool TryOpenGUI()
    {
        if (Fader.INSTANCE != null && Fader.INSTANCE.IsFading()) return false;

        foreach (GUI screenMenu in guis)
        {
            if (screenMenu.IsOpen()) return false;
        }

        isOpen = true;
        OnOpen();
        return true;
    }

    public void TryOpenGUIVoid()
    {
        if (Fader.INSTANCE != null && Fader.INSTANCE.IsFading()) return;

        foreach (GUI screenMenu in guis)
        {
            if (screenMenu.IsOpen()) return;
        }

        isOpen = true;
        OnOpen();
    }

    public void ForceOpenGUI()
    {
        isOpen = true;
        OnOpen();
    }

    public void ForceCloseGUI()
    {
        isOpen = false;
        OnClose();
    }

    public virtual void OnOpen()
    {

    }

    public virtual void OnClose()
    {

    }

    public virtual void Always()
    {

    }

    public bool IsOpen()
    {
        return isOpen;
    }
}
