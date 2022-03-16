using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectablesTxt : MonoBehaviour
{
    public Inventory inventory;
    public Text text;

    void Start()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
    }

    void Update()
    {
        text.text =  inventory.inventory[10] + "/ 10";
    }
}
