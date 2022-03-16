using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasAnimator_Stages : MonoBehaviour
{
    public Image image;
    public Sprite[] sprites;
    public float[] changeTime;
    private float time;
    public int currentImage;

    public int[] imagesInStage;
    public bool[] loop;
    public int stage;

    public MainMenu menu;
    public Image startButton, QuiteButton;
    public Text startText, quiteText;
    private Color colour;
    public Button sB, qB;

    bool On = false;
    private float i = 0;

    public StarLayer layer;
    private bool speeding = false;

    void Start()
    {
        image.sprite = sprites[0];
        currentImage = 0;
        stage = 0;
        time = changeTime[0];
        colour = startButton.color;
    }

    void Update()
    {
        if (On)
        {
            //turn on opacity
            i += Time.deltaTime;
            colour.a = i;
            startButton.color = colour;
            QuiteButton.color = colour;
            startText.color = colour;
            quiteText.color = colour;

            if (i > 1)
            {
                On = false;
                sB.enabled = true;
                qB.enabled = true;
            }
        }

        time -= Time.deltaTime;
        if (time < 0)
        {
            if (currentImage == imagesInStage[imagesInStage.Length - 1])
            {
                menu.PlayGame();
            }

            if(currentImage > 32 && !speeding)
            {
                for(int i = 0; i < layer.numberStars.Length; i++)
                {
                    layer.speed[i] *= 3;
                    Debug.Log(layer.speed[i]);
                }
                speeding = true;
            }

            //turn on buttons
            if (currentImage == 6)
            {
                //finished initial stage
                On = true;
            }

            //change Image
            if (currentImage == imagesInStage[stage])
            {
                if (loop[stage])
                {
                    //return to first image
                    currentImage = imagesInStage[stage - 1];
                }
                else
                {
                    stage++;
                    //Next Image
                    currentImage++;
                }
            }
            else
            {
                //Next Image
                currentImage++;
            }
            image.sprite = sprites[currentImage];
            time = changeTime[currentImage];
        }
    }
}
