using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelText : MonoBehaviour
{

    public UpgradeManager upgradeManager;
    public Text level;
    public int i;

    void Start()
    {
        upgradeManager = GameObject.Find("StorePanel").GetComponent<UpgradeManager>();
        //level = this.gameObject.GetComponent<Text>();
    }

    void Update()
    {
        level.text = "Level: " + (upgradeManager.CurrUpgrade[i] + 1).ToString();
    }
}
