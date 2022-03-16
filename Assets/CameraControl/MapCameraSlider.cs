using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapCameraSlider : MonoBehaviour
{
    public Camera camera;
    public Slider slider;

    public int minValue;
    public int maxValue;


    void Start()
    {
        slider.minValue = minValue;
        slider.maxValue = maxValue;
    }

    void Update()
    {
        camera.fieldOfView = slider.value;
    }
}
