using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    public int[] drops;
    public float[] chance;
    public int[] rarity;
    private int i = 0;
    private float PUChance;

    //public int chance;
    public int value;
    private int worth;

    //Dialogue
    public bool triggerEvent;
    public Dialogue dialogue;
    public Stats stats;
    public AudioSource audio;

    void Start()
    {
        dialogue = GameObject.Find("DialogueManager").GetComponent<Dialogue>();
        stats = GameObject.Find("Inventory").transform.GetChild(0).GetComponent<Stats>();
        audio = GameObject.Find("Reward").GetComponent<AudioSource>();
    }

    public void Pickup(Inventory inventory)
    {
        audio.Play();
        foreach(int drop in drops)
        {
            for(var j = 0; j < rarity[i]; j++)
            {
                PUChance = Random.Range(0, 100);
                if(PUChance < chance[i])
                {
                    inventory.inventory[drop]++;
                }
            }
            i++;
        }
        if (triggerEvent)
        {
            GameObject.Find("OldManStory").transform.GetChild(stats.eventNum).GetComponent<Sentence>().Activate();
            stats.eventNum++;
        }
        Destroy(gameObject);
    }

}
