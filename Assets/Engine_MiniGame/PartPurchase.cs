using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartPurchase : MonoBehaviour
{
    public Inventory inventory;
    public CanvasPart canvasPart;
    public Text cost;
    public Text resistance;
    public Text partName;
    public string[] names;

    public static PartPurchase partPurchaseInstance;

    void Start()
    {
        inventory = Inventory.instance;
        partPurchaseInstance = this;
    }

    public void purchase()
    {
        if(canvasPart.cost <= inventory.inventory[0])
        {
            //buy the part
            inventory.inventory[0] -= canvasPart.cost;

            //add part to Engine List
            GameObject.Find("Engine").GetComponent<Engine>().purchasedParts.Add(canvasPart.part);

            //destroy the canvas part
            Destroy(canvasPart.gameObject);

            //turn off nesseccary UI

        }
        else
        {
            Debug.Log("not enough money");
        }
    }

    public void SetUp(CanvasPart CP)
    {
        cost.text = "Cost: " + CP.cost;
        resistance.text = "Resistance: " + CP.resistance;
        partName.text = names[CP.partType];

        canvasPart = CP;
    }
}
