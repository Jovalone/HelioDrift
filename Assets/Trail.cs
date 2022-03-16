using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{
    public GameObject trail;
    public float time;
    public float rechargeTime;
    public bool active;
    public int shots;
    
    public void startRoutine(int Shots, float RechargeTime)
    {
        shots = Shots;
        rechargeTime = RechargeTime;
    }

    void Update()
    {
        if (shots > 0)
        if(time < 0)
        {
            time = rechargeTime;

            //Do Action
            Instantiate(trail, transform.position, transform.rotation);
                shots--;
        }
        else
        {
            time -= Time.deltaTime;
        }
    }
}
