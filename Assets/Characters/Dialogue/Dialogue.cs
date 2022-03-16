using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;

public class Dialogue : MonoBehaviour
{

    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    public int index;
    public float TypingSpeed;
    public bool Completed;
    public AudioSource audio;

    public GameObject continueButton, skipButton, Hologram;
    public float delayTime;

    public void BeginSentence()
    {
        StartCoroutine(Type()); 
    }

    void Update()
    {
        if (!Completed)
        {
            if (textDisplay.text == sentences[index])
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    NextSentence();
                }
            }
        }
    }

    IEnumerator Type()
    {
        if (Completed)
        {
            yield return new WaitForSeconds(delayTime);
        }

        TurnOnHologram();
        audio.pitch = Random.Range(0.7f, 1.3f);
        audio.Play();
        Completed = false;
        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(TypingSpeed);
        }
        continueButton.SetActive(true);
    }

    public void NextSentence()
    {
        continueButton.SetActive(false);

        if (index < sentences.Length - 1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
            Completed = false;
        }
        else
        {
            textDisplay.text = "";
            continueButton.SetActive(false);
            Hologram.SetActive(false);
            Completed = true;
        }
    }

    public void TurnOnHologram()
    {
        audio.Play();
        skipButton.SetActive(true);
        Hologram.SetActive(true);
    }

    public void SkipDialogue()
    {
        textDisplay.text = "";
        continueButton.SetActive(false);
        skipButton.SetActive(false);
        Hologram.SetActive(false);
        Completed = true;
    }

}
