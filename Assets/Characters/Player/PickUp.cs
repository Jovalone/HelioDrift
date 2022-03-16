using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUp : MonoBehaviour
{
    public Inventory inventory;
    public Transform player;
    private GameObject ClosestDrop;
    private float BestDist;
    private float currDist;
    public float MinDist;
    public Text txt;
    private bool first = true;

    void Start()
    {
        if(GameObject.Find("Player") != null)
        {
            player = GameObject.Find("Player").transform;
            inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
            txt = GameObject.Find("PickUp").GetComponent<Text>();
        }
    }

    void Update()
    {
        if(player == null && first)
        {
            OnNewScene();
        }

        BestDist = Mathf.Infinity;
        //Fill list
        foreach (GameObject drop in GameObject.FindGameObjectsWithTag("drop"))
        {
            currDist = Vector2.Distance(player.position, drop.transform.position);
            if(currDist < BestDist)
            {
                BestDist = currDist;
                ClosestDrop = drop;
            }
        }
        if(BestDist < MinDist)
        {
            //Allow Pickup
            txt.enabled = true;
            if (Input.GetKeyDown(KeyCode.Return) == true)
            {
                ClosestDrop.GetComponent<Drop>().Pickup(inventory);
                txt.enabled = false;
            }
        }
        else if (txt != null)
        {
            txt.enabled = false;
        }
    }

    void OnNewScene()
    {
        if(GameObject.Find("Player") != null)
        {
            player = GameObject.Find("Player").transform;
        }

        if(player != null)
        {
            txt = GameObject.Find("PickUp").GetComponent<Text>();
        }
    }
}
