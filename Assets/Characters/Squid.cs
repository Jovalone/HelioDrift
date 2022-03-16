using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squid : MonoBehaviour
{

    public Transform hitspot;
    public Hittable hittable;
    public float hitRange;
    public float hitTime;
    public float rechargeTime;
    public bool recharging;

    public GameObject SquidAttack;
    public ParticleSystem particleSystem;

    List<GameObject> tempList;
    void Update()
    {
        tempList = new List<GameObject>();
        foreach (GameObject enemy in hittable.Attackers)
        {
            if(enemy != null)
            {
                if ((Vector3.Distance(hitspot.position, enemy.transform.position) < hitRange) && !recharging)
                {
                    StartCoroutine(squidAttack());
                }
            }
            else
            {
                tempList.Add(enemy);
            }
        }

        foreach(GameObject temp in tempList)
        {
            hittable.Attackers.Remove(temp);
        }
    }

    IEnumerator squidAttack()
    {
        recharging = true;
        SquidAttack.SetActive(true);
        particleSystem.Play();
        yield return new WaitForSeconds(rechargeTime);

        recharging = false;
        SquidAttack.SetActive(false);
    }
}
