using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    public static Fader INSTANCE;
    private Image image;
    private bool fading;

    private void Start()
    {
        DontDestroyOnLoad(this);
        INSTANCE = this;

        image = GetComponent<Image>();
    }

    public void FadeTo(Action toDoWhenBlack, Func<bool> returnCondition, float duration)
    {
        if (fading) return;
        fading = true;
        image.raycastTarget = true;
        StartCoroutine(FadeToBlack_CR(toDoWhenBlack, returnCondition, duration));
    }

    private IEnumerator FadeToBlack_CR(Action toDoWhenBlack, Func<bool> returnCondition, float duration)
    {
        StartCoroutine(Fade(Color.black, duration));
        yield return new WaitUntil(() => image.color == Color.black);
        toDoWhenBlack.Invoke();
        yield return new WaitUntil(returnCondition);
        StartCoroutine(Fade(new Color(0, 0, 0, 0), duration));
        yield return new WaitUntil(() => image.color == new Color(0, 0, 0, 0));
        fading = false;
        image.raycastTarget = false;
    }

    private IEnumerator Fade(Color toColor, float duration)
    {
        Color fromColor = image.color;

        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            image.color = Color.Lerp(fromColor, toColor, normalizedTime);
            yield return null;
        }

        image.color = toColor;
    }

    public bool IsFading()
    {
        return fading;
    }
}
