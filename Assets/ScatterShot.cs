using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterShot : MonoBehaviour
{
    public GameObject Bullet;
    public int shots;
    public float rtime;
    public float rotationalSpeed;
    private float t;
    public float rotation = 0;
    private int offset = 0;
    

    // Update is called once per frame
    void Update()
    {
        if (t < 0)
        {
            //Fire Shots
            float angle = 360 / shots;
            for(int i = 0; i < shots; i++)
            {
                Instantiate(Bullet, transform.position, Quaternion.Euler(0, 0, rotation + angle * i + offset * angle / 2));
            }

            if(offset == 0)
            {
                offset++;
            }
            else
            {
                offset--;
            }

            t = rtime;
            rotation += rotationalSpeed;
        }
        else
        {
            t -= Time.deltaTime;
        }
    }
}
