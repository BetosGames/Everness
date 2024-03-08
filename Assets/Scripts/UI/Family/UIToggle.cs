using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static FileManager;
using static UnityEngine.UI.Button;
using static UnityEngine.UI.Toggle;
using Image = UnityEngine.UI.Image;
using Toggle = UnityEngine.UI.Toggle;

public class UIToggle : MonoBehaviour
{
    public Color colorTheme;
    public ToggleEvent onToggle = new ToggleEvent();
    public ButtonClickedEvent whileTrue = new ButtonClickedEvent();
    public ButtonClickedEvent whileFalse = new ButtonClickedEvent();
    public bool value;


    [HideInInspector] public Image backingImage;
    [HideInInspector] public Image knobImage;
    [HideInInspector] public Toggle toggle;

    private void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        toggle = GetComponent<Toggle>();

        Texture2D backingTexture = FileManager.TextureFromPNG($"ui/toggle/backing.png");
        Texture2D knobTexture = FileManager.TextureFromPNG($"ui/toggle/knob.png");

        Vector4 borders = new Vector4(5, 5, 5, 5);
        Sprite backingSprite = Sprite.Create(backingTexture, new Rect(0, 0, backingTexture.width, backingTexture.height), Vector2.one * 0.5f, 100, 1, SpriteMeshType.Tight, borders);
        Sprite knobSprite = Sprite.Create(knobTexture, new Rect(0, 0, knobTexture.width, knobTexture.height), Vector2.one * 0.5f, 100, 1, SpriteMeshType.Tight, borders);

        backingImage = transform.GetComponent<Image>();
        knobImage = transform.GetChild(0).GetComponent<Image>();

        backingImage.sprite = backingSprite;
        knobImage.sprite = knobSprite;

        backingImage.type = Image.Type.Sliced;
        knobImage.type = Image.Type.Sliced;
        backingImage.pixelsPerUnitMultiplier = 0.49f;
        knobImage.pixelsPerUnitMultiplier = 0.49f;
    }

    public void Update()
    {
        value = toggle.isOn;
        knobImage.transform.localPosition = Vector2.right * (value ? 1 : -1) * 11.8554f;
        WhileSomething(value);

        Always();
    }

    public virtual void Always()
    {
        float h;
        float s;
        float v;

        Color.RGBToHSV(colorTheme, out h, out s, out v);

        ColorBlock colors = toggle.colors;
        colors.normalColor = colorTheme;
        colors.highlightedColor = Color.HSVToRGB(h, s + 0.2f, v);
        colors.pressedColor = Color.HSVToRGB(h, s + 0.4f, v);
        colors.selectedColor = colors.highlightedColor;
        colors.disabledColor = new Color(colorTheme.r, colorTheme.g, colorTheme.b, 60);

        toggle.colors = colors;

        knobImage.color = value ? Color.HSVToRGB(h, s + 0.7f, v) : Color.HSVToRGB(0, 0, 0.83f);
    }

    public void OnToggle()
    {
        onToggle.Invoke(value);
    }

    private void WhileSomething(bool value)
    {
        if(value) whileTrue.Invoke(); else whileFalse.Invoke();
    }
}
