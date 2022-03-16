using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hittable : MonoBehaviour
{
    public float Health;
    public GameObject Object;
    public bool Death = false;
    public bool inanimate;
    public GameObject drop;

    public List<GameObject> Attackers;
    public List<Hittable> Allies;

    public float dropChance;
    public bool player;

    void Start()
    {
        Attackers = new List<GameObject>();
        Allies.Add(this);
        if (!inanimate)
        {
            Level.levelInstance.liveCharacters.Add(this);
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health < 0 || Health == 0)
        {
            if(dropChance > Random.Range(0, 100))
            {
                Instantiate(drop, this.transform.position, Quaternion.identity);
            }
            if (!inanimate)
            {
                Level.levelInstance.liveCharacters.Remove(this);
            }
            Death = true;
        }
        if (player)
        {
            ScreenShakeController.instance.StartShake(0.004f * damage, 0.05f * damage);
        }
    }

    void OnMouseDown()
    {
        FirePoint firePoint = GameObject.Find("FirePoint").GetComponent<FirePoint>();

        firePoint.Target = this.gameObject.transform;
        firePoint.SpawnLockOn();
    }

    public void triggerMouse()
    {
        FirePoint firePoint = GameObject.Find("FirePoint").GetComponent<FirePoint>();

        firePoint.Target = this.gameObject.transform;
        firePoint.SpawnLockOn();
    }
}
