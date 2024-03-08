using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEngine.UI.Button;

public class UIScroll : MonoBehaviour
{
    public string panelType;

    private Image image;
    private TMP_Text noContentText;

    public Transform content;

    public void Start()
    {
        Texture2D texture = FileManager.TextureFromPNG($"ui/panel/{panelType}.png");

        Vector4 borders = new Vector4(5, 5, 5, 5);
        Sprite panelSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f, 100, 1, SpriteMeshType.Tight, borders);

        image = GetComponent<Image>();
        noContentText = transform.GetChild(1).GetComponent<TMP_Text>();

        image.sprite = panelSprite;
        image.type = Image.Type.Sliced;
        image.pixelsPerUnitMultiplier = 0.2f;
    }

    public void Update()
    {
        noContentText.gameObject.SetActive(content.childCount == 0);
    }

    public Transform GetContentBox()
    {
        return content;
    }

    public void DeleteAllContent()
    {
        transform.GetChild(0).GetChild(0).gameObject.DestroyAllChildren();
    }
}
