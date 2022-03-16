using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasAnimation : MonoBehaviour
{

    public float changeTime;
    public float time;
    public int currentImage;
    public Sprite[] sprites;
    public Image image;

    void Start()
    {
        image.sprite = sprites[0];
        //time = changeTime;
        currentImage = 0;
    }

    void Update()
    {
        time -= Time.deltaTime;
        if(time < 0)
        {
            //change Image
            if(currentImage + 1 == sprites.Length)
            {
                //Reached Last Image
                currentImage = 0;
            }
            else
            {
                //Next Image
                currentImage++;
            }
            image.sprite = sprites[currentImage];
            time = changeTime;
        }
    }
}
