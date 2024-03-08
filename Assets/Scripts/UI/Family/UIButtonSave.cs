using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static FileManager;

public class UIButtonSave : UIButton
{
    public TMP_Text worldName;
    public TMP_Text worldDateAndTime;

    private Save save;

    public override void Init()
    {
        base.Init();
    }

    public override void Always()
    {
        float h;
        float s;
        float v;

        Color.RGBToHSV(colorTheme, out h, out s, out v);

        ColorBlock colors = button.colors;
        colors.normalColor = colorTheme;
        colors.highlightedColor = Color.HSVToRGB(h, s + 0.2f, v);
        colors.pressedColor = Color.HSVToRGB(h, s + 0.4f, v);
        colors.selectedColor = colors.highlightedColor;
        colors.disabledColor = new Color(colorTheme.r, colorTheme.g, colorTheme.b, 60);

        button.colors = colors;

        worldName.color = Color.HSVToRGB(h, s + 0.3f, v - 0.7f);
        worldDateAndTime.color = Color.HSVToRGB(h, s + 0.3f, v - 0.7f);
    }

    public void Setup(Save save)
    {
        this.save = save;
        worldName.text = save.saveName;
        worldDateAndTime.text = $"{save.saveDateTime.DayOfWeek.ToString()}, {save.saveDateTime.ToString("MMM")} {save.saveDateTime.Day.ToString()} {save.saveDateTime.Year.ToString()}, {save.saveDateTime.ToString("%h:mmtt")}";
    }

    public void Create()
    {
        Universe.isSetup = false;
        Fader.INSTANCE.FadeTo(new System.Action(StartNewUniverse), () => Universe.isSetup, 0.7f);
    }

    public void StartNewUniverse()
    {
        Universe.generateFromSave = save;
        Universe.universeGenPreset = "Default";
        Universe.universeName = save.saveName;
        Universe.universeSeed = save.saveSeed;
        //TODO add online toggle and passcode to existing worlds.
        Universe.saveIsOnline = false;
        Universe.savePasscode = "";

        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}
