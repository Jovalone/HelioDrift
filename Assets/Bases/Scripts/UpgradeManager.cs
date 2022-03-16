using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    //General Variables
    //public Player player;
    public Inventory inventory;
    public int[] CurrUpgrade;
    //public Text[] Prices;
    public Text[] Levels;
    Stats stats;

    //Regular Speed Upgrades
    public float[] SpeedValues;
    public int[] SpeedPrices;

    //Rotation Upgrades
    public float[] RotationValues;
    public int[] RotationPrices;

    //Boost Speed Upgrades
    //public Boost boost;
    public float[] BoostSpeedValues, BoostDistanceValues;
    public int[] BoostSpeedPrices;

    //Shield Upgrades
    //public Shield shield;
    public float[] ShieldValues;
    public int[] ShieldPrices;

    //Armour Upgrades
    //public Hittable hittable;
    public float[] ArmourValues;
    public int[] ArmourPrices;

    //Energy Upgrades
    public float[] EnergyValues;
    public int[] EnergyPrices;

    public int[][] Prices;

    void Start()
    {
        Prices = new int[6][];
        for (int j = 0; j < 6; j++)
        {
            Prices[j] = new int[3];
        }

        for (int i = 0; i < 3; i++)
        {
            Prices[0][i] = SpeedPrices[i];
            Prices[1][i] = RotationPrices[i];
            Prices[2][i] = BoostSpeedPrices[i];
            Prices[3][i] = ShieldPrices[i];
            Prices[4][i] = ArmourPrices[i];
            Prices[5][i] = EnergyPrices[i];
        }

        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        stats = GameObject.Find("Inventory").transform.GetChild(0).GetComponent<Stats>();
    }

    public void Upgrade(int x)
    {
        switch (x)
        {
            case 0:
                if (inventory.inventory[0] >= SpeedPrices[CurrUpgrade[x]]) //check money
                {
                    inventory.inventory[0] -= SpeedPrices[CurrUpgrade[x]]; //take the cost away
                    stats.RegSpeed = SpeedValues[CurrUpgrade[x]];
                    CurrUpgrade[x]++;
                }
                break;

            case 1:
                if (inventory.inventory[0] >= RotationPrices[CurrUpgrade[x]])
                {
                    inventory.inventory[0] -= RotationPrices[CurrUpgrade[x]];
                    stats.Rotation = RotationValues[CurrUpgrade[x]];
                    CurrUpgrade[x]++;
                }
                break;

            case 2:
                if (inventory.inventory[0] >= BoostSpeedPrices[CurrUpgrade[x]])
                {
                    inventory.inventory[0] -= BoostSpeedPrices[CurrUpgrade[x]];
                    stats.Boost_Total = BoostDistanceValues[CurrUpgrade[x]];
                    stats.Boost_Speed = BoostSpeedValues[CurrUpgrade[x]];
                    CurrUpgrade[x]++;
                }
                break;

            case 3:
                if (inventory.inventory[0] >= ShieldPrices[CurrUpgrade[x]])
                {
                    inventory.inventory[0] -= ShieldPrices[CurrUpgrade[x]];
                    stats.Shield = ShieldValues[CurrUpgrade[x]];
                    CurrUpgrade[x]++; 
                }
                break;

            case 4:
                if (inventory.inventory[0] >= ArmourPrices[CurrUpgrade[x]])
                {
                    inventory.inventory[0] -= ArmourPrices[CurrUpgrade[x]];
                    stats.Armour = ArmourValues[CurrUpgrade[x]];
                    stats.MaxArmour = ArmourValues[CurrUpgrade[x]];
                    CurrUpgrade[x]++;
                }
                break;

            case 5:
                if (inventory.inventory[0] >= EnergyPrices[CurrUpgrade[x]])
                {
                    inventory.inventory[0] -= EnergyPrices[CurrUpgrade[x]];
                    stats.Energy = EnergyValues[CurrUpgrade[x]];
                    stats.MaxEnergy = EnergyValues[CurrUpgrade[x]];
                    CurrUpgrade[x]++;
                }
                break;
        }

    }
}
