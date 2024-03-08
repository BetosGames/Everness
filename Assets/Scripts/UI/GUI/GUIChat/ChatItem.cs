using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatItem : MonoBehaviour
{
    private float showTime = 15f;

    private Color backColor;
    private Color textColor;

    private Color changingBackColor;
    private Color changingTextColor;

    private void Start()
    {
        backColor = GetComponent<Image>().color;
        textColor = GetComponentInChildren<TMP_Text>().color;

        changingBackColor = backColor;
        changingTextColor = textColor;
    }

    public void SetText(string text)
    {
        TextMeshProUGUI textMesh = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        textMesh.SetText(text);
    }

    public void Update()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());

        if (transform.parent.childCount - transform.GetSiblingIndex() >= 7) showTime = 0;

        if (GUIChat.INSTANCE.IsOpen())
        {
            GetComponent<Image>().color = backColor;
            GetComponentInChildren<TMP_Text>().color = textColor;
        }
        else
        {
            GetComponent<Image>().color = changingBackColor;
            GetComponentInChildren<TMP_Text>().color = changingTextColor;
        }

        if (showTime <= 0)
        {
            changingBackColor = Color.Lerp(changingBackColor, new Color(0, 0, 0, 0), 2 * Time.deltaTime);
            changingTextColor = Color.Lerp(changingTextColor, new Color(0, 0, 0, 0), 2 * Time.deltaTime);
        }

        showTime -= Time.deltaTime;
    }
}
