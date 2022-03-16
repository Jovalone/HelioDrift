using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransactionManager : MonoBehaviour
{
    public Inventory inventory;
    public Text[] Quantities, Prices;
    //public int[] quantities, prices;
    public int[] prices;
    //public Text coins;
    private int j;


    void Start()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
    }

    void FixedUpdate()
    {
        UpdateStore();
    }

    public void UpdateStore()
    {
        j = 1;
        foreach (Text quant in Quantities)
        {
            quant.text = "Quantity: " + inventory.inventory[j].ToString();
            Prices[j - 1].text = "Price: " + prices[j - 1].ToString();
            j++;
        }
        //coins.text = "Coins: " + inventory.inventory[0];
    }

    public void Sell(int i)
    {
        if(inventory.inventory[i+1] > 0)
        {
            inventory.inventory[0] += prices[i];
            inventory.inventory[i+1]--;
        }

        UpdateStore();
        
    }
}
