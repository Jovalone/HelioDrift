using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Star : MonoBehaviour
{

    public Image image;
    private Color colour;
    public float time;
    public float time_0;
    private float i = 0;
    public int j;


    void Start()
    {
        colour = image.color;
        time_0 = time;
        time *= 2;

        //StartCoroutine(FadeIn());
    }

    void Update()
    {
        if (i < 1)
        {
            colour.a = i;
            i += Time.deltaTime / time;
        }
        else
        {
            time = time_0;
        }
        image.color = colour;
    }

    public void moved()
    {
        i = 0;
    }
}
