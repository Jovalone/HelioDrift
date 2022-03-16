using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTypeButton : MonoBehaviour
{
    public FirePoint firePoint;
    public Stats stats;

    void Start()
    {
        stats = GameObject.Find("Inventory").transform.GetChild(0).gameObject.GetComponent<Stats>();
    }

    public void BulletType(int type)
    {
        firePoint.BulletType = type;
        stats.BulletType = type;
    }

    public void SlowTime()
    {
        if(Time.timeScale == 1f)
        {
            Time.timeScale = 0.2f;
        }
    }

    public void RestetTime()
    {
        Time.timeScale = 1f;
        firePoint.updateAmmo();
    }
}
