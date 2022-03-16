using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplenishmentSlider : MonoBehaviour
{
    public GameObject panel;
    public Slider slider;
    public Stats stats;
    public Inventory inventory;
    public int[] Prices;
    public int[] AmmoPrices;
    public int i;
    public int k;
    public Text text, Current, Quantity, Cost;

    public int cost;
    public int quantity;

    public int minMoney;
    public int minAmmo;
    public int minEnergy;

    public GameObject BulletPanel;

    void Update()
    {
        if (slider.gameObject.active)
        {
            switch (i)
            {
                case 1:
                    Current.text = "Current: " + stats.Armour;
                    Quantity.text = "Quantity: " + (int)(slider.value - slider.minValue);
                    Cost.text = "Cost: " + (int)((slider.value - slider.minValue) * Prices[i - 1]);
                    break;

                case 2:
                    Current.text = "Current: " + stats.Energy;
                    Quantity.text = "Quantity: " + (int)(slider.value - slider.minValue);
                    Cost.text = "Cost: " + (int)((slider.value - slider.minValue) * Prices[i - 1]);
                    break;

                case 3:
                    Current.text = "Current: " + stats.Ammo[k];
                    Quantity.text = "Quantity: " + (int)(slider.value - slider.minValue) * 10;
                    Cost.text = "Cost: " + (int)((slider.value - slider.minValue) * Prices[i - 1]);
                    break;
            }
        }
    }

    public void SetUp(int j)
    {
        i = j;
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        stats = GameObject.Find("Stats").GetComponent<Stats>();
        switch (i)
        {
            case 1:
                slider.minValue = stats.Armour;
                slider.maxValue = stats.MaxArmour;
                text.text = "Armour Repair";
                break;

            case 2:
                slider.minValue = stats.Energy;
                slider.maxValue = stats.MaxEnergy;
                text.text = "Energy Recharge";
                break;

            case 3:
                
                slider.minValue = stats.Ammo[k];
                slider.maxValue = stats.Ammo[k] + 10;
                text.text = "Ammo Restock";
                break;
        }
    }

    public void SetUpAmmo(int l)
    {
        k = l;
    }

    public void Purchase()
    {
        quantity = (int)(slider.value - slider.minValue);
        cost = quantity * Prices[i - 1];

        if(inventory.inventory[0] - cost > 0)
        {
            inventory.inventory[0] -= cost;
            switch (i)
            {
                case 1:
                    stats.Armour += quantity;
                    break;

                case 2:
                    stats.Energy += quantity;
                    break;

                case 3:
                    stats.Ammo[k] += quantity * 10;//
                    break;
            }

            SetUp(i);
        }
    }

    public void QuickFix()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        stats = GameObject.Find("Stats").GetComponent<Stats>();

        if(inventory.inventory[0] < minMoney)
        {
            if (stats.Ammo[0] < minAmmo)//
            {
                stats.Ammo[0] = minAmmo;//
                Debug.Log(stats.Ammo);
            }

            if (stats.Energy < minEnergy)
            {
                stats.Energy = minEnergy;
            }
        }
    }
}
