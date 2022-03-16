using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplenishmentManager : MonoBehaviour
{

    Inventory inventory;
    Stats stats;
    public int[] Prices;

    void Start()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        stats = GameObject.Find("Stats").GetComponent<Stats>();
    }

    public void Purchase(int i)
    {
        
    }
}
