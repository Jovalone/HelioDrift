using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PriceText : MonoBehaviour
{
    public UpgradeManager upgradeManager;
    public Text price;
    public int i;

    void Start()
    {
        upgradeManager = GameObject.Find("StorePanel").GetComponent<UpgradeManager>();
    }

    void Update()
    {
        price.text = "Price: " + upgradeManager.Prices[i][upgradeManager.CurrUpgrade[i]].ToString();//(upgradeManager.CurrUpgrade[i] + 1).ToString();
    }
}
