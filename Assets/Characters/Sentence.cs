using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentence : MonoBehaviour
{
    public string[] sentences;
    public Dialogue dialogue;
    public bool activeOnStart;
    public bool activated = false;

    void Start()
    {
        if(dialogue == null)
        {
            dialogue = GameObject.Find("DialogueManager").GetComponent<Dialogue>();
        }
        /*
        if (activeOnStart)
        {
            Activate();
        }
        */
    }

    public void Activate()
    {
        activated = true;
        dialogue.textDisplay.text = "";
        dialogue.sentences = sentences;
        dialogue.index = 0;
        dialogue.BeginSentence();
    }
}
