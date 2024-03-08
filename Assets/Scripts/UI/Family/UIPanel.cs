using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEngine.UI.Button;
using UnityEngine.EventSystems;

public class UIPanel : MonoBehaviour, IPointerClickHandler
{
    public string panelType;
    public Color color;
    public ButtonClickedEvent onPress = new ButtonClickedEvent();

    private Image image;

    public void Start()
    {
        Texture2D texture = FileManager.TextureFromPNG($"ui/panel/{panelType}.png");

        Vector4 borders = new Vector4(5, 5, 5, 5);
        Sprite buttonSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f, 100, 1, SpriteMeshType.Tight, borders);

        image = GetComponent<Image>();

        image.sprite = buttonSprite;
        image.type = Image.Type.Sliced;
        image.pixelsPerUnitMultiplier = 0.2f;
    }

    public void Update()
    {

        image.color = color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onPress.Invoke();
    }
}
