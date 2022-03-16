using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryQuantityTxt : MonoBehaviour
{

    Inventory inventory;
    public Text quantity;
    public int i;


    void Start()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        quantity.text = "Quantity: " + inventory.inventory[i].ToString();
    }
}
