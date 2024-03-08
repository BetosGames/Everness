using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScope : MonoBehaviour
{
    public Slider slider;
    public Camera cam;
    public float easing;

    private float scopeGoal;

    private void Awake()
    {
        OnSliderValueChanged();
    }

    public void OnSliderValueChanged()
    {
        scopeGoal = Mathf.Lerp(8, 30, slider.value);
    }

    public void Update()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, scopeGoal, easing * Time.deltaTime);
    }
}
