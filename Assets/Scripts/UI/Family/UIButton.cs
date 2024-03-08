using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEngine.UI.Button;

public class UIButton : MonoBehaviour
{
    public string buttonType;
    public Color colorTheme;
    public ButtonClickedEvent onPress = new ButtonClickedEvent();


    [HideInInspector] public Image buttonImage;
    [HideInInspector] public Button button;
    [HideInInspector] public TMP_Text buttonText;

    public void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        Texture2D texture = FileManager.TextureFromPNG($"ui/button/{buttonType}.png");

        Vector4 borders = new Vector4(5, 5, 5, 5);
        Sprite buttonSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f, 100, 1, SpriteMeshType.Tight, borders);

        buttonImage = GetComponent<Image>();
        button = GetComponent<Button>();
        buttonText = GetComponentInChildren<TMP_Text>();

        buttonImage.sprite = buttonSprite;
        buttonImage.type = Image.Type.Sliced;
        buttonImage.pixelsPerUnitMultiplier = 0.2f;
    }

    public void Update()
    {
        Always();
    }

    public virtual void Always()
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

        buttonText.color = Color.HSVToRGB(h, s + 0.3f, v - 0.7f);
    }

    public void OnPress()
    {
        onPress.Invoke();
    }
}
