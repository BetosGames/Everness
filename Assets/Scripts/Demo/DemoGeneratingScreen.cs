using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DemoGeneratingScreen : MonoBehaviour
{
    public TMP_Text generatingText;
    public Transform loadingIcon;
    public float loadingIconSpinSpeed;
    public GameObject elements;
    public GameObject generatingBG;
    public Image generatingBGImage;
    public Animator generatingBGAnimator;
    private void Start()
    {
        generatingBG.SetActive(true);
        generatingText.text = $"Generating Seed\n{Universe.universeSeed}";
        generatingBGImage.raycastTarget = true;
        StartCoroutine(LoadWhenPlayerIsGrounded());
    }
    private void Update()
    {
        if (loadingIcon.gameObject.activeInHierarchy) loadingIcon.Rotate(Vector3.forward * loadingIconSpinSpeed * Time.deltaTime);
    }

    private IEnumerator LoadWhenPlayerIsGrounded()
    {
        yield return new WaitUntil(() => Player.INSTANCE != null);
        yield return new WaitUntil(() => Player.INSTANCE.isGrounded);
        yield return new WaitForSeconds(1.5f);
        generatingBGImage.raycastTarget = false;
        elements.SetActive(false);
        generatingBGAnimator.SetTrigger("FadeAway");
    }
}
