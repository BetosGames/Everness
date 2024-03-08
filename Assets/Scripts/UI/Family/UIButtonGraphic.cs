using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonGraphic : UIButton
{
    public string graphicType;

    private Image graphicImage;

    public override void Init()
    {
        base.Init();

        Texture2D texture = FileManager.TextureFromPNG($"ui/graphic/{graphicType}.png");

        Sprite graphicSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f, 100, 1, SpriteMeshType.Tight);

        graphicImage = transform.GetChild(0).GetComponent<Image>();

        graphicImage.sprite = graphicSprite;
        graphicImage.type = Image.Type.Simple;
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

        graphicImage.color = Color.HSVToRGB(h, s + 0.3f, v - 0.7f);
    }
}
