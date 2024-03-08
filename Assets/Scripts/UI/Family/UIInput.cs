using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEngine.UI.Button;
using System.Linq;
using UnityEngine.EventSystems;

public class UIInput : MonoBehaviour
{
    public string inputType;
    public Color colorTheme;
    public KeyCode enterKey;
    public ButtonClickedEvent onPressEnter = new ButtonClickedEvent();

    [HideInInspector]
    public bool editingInput;

    private Image image;
    private TMP_InputField inputField;
    private TMP_Text placeholderText;

    public void Start()
    {
        Texture2D texture = FileManager.TextureFromPNG($"ui/button/{inputType}.png");

        Vector4 borders = new Vector4(5, 5, 5, 5);
        Sprite inputFieldSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f, 100, 1, SpriteMeshType.Tight, borders);

        image = GetComponent<Image>();
        inputField = GetComponent<TMP_InputField>();
        placeholderText = GetComponentInChildren<TMP_Text>();

        image.sprite = inputFieldSprite;
        image.type = Image.Type.Sliced;
        image.pixelsPerUnitMultiplier = 0.49f;
    }

    public void Update()
    {
        float h;
        float s;
        float v;

        Color.RGBToHSV(colorTheme, out h, out s, out v);

        ColorBlock colors = inputField.colors;
        colors.normalColor = colorTheme;
        colors.highlightedColor = Color.HSVToRGB(h, s + 0.2f, v);
        colors.pressedColor = Color.HSVToRGB(h, s + 0.4f, v);
        colors.selectedColor = colors.highlightedColor;
        colors.disabledColor = new Color(colorTheme.r, colorTheme.g, colorTheme.b, 60);

        inputField.colors = colors;

        placeholderText.color = Color.HSVToRGB(h, s + 0.3f, v - 0.7f);
        placeholderText.gameObject.SetActive(!editingInput);
        inputField.caretColor = placeholderText.color;

        inputField.text = inputField.text.TrimEnd('\n');

        if (editingInput)
        {
            if (Input.GetKeyDown(enterKey))
            {
                onPressEnter.Invoke();
                print("AAAA");
            }
        }
    }

    public string ReadText()
    {
        return GetComponent<TMP_InputField>().text;
    }

    public void SetText(string text)
    {
        GetComponent<TMP_InputField>().text = text;
    }

    public void OnSelect()
    {
        editingInput = true;
    }

    public void OnDeselect()
    {
        editingInput = false;
    }
}
