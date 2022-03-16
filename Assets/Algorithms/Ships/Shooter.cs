using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    private float time = 0;
    public float reloadTime;
    private int shots = 0;
    public int bullet_num;
    private float time1 = 0;
    public float coolDownTime;

    public Ship ship;

    public GameObject bullet_prefab;
    private GameObject bullet;

    public float K;
    float Angle, AngleDiff;

    void Update()
    {
        if (ship.Combat)
        {
            Angle = K / (Mathf.Sqrt(Vector2.Distance((Vector2)transform.position, ship.Target)));
            Vector2 Direction = (ship.Target - (Vector2)transform.position).normalized;
            AngleDiff = Vector3.Angle(transform.up, Direction);

            if (time1 < 0)
            {
                //not cooling
                if (shots > bullet_num)
                {
                    //start cooling
                    time1 = coolDownTime;
                    shots = 0;
                }
                else
                {
                    if (time < 0)
                    {
                        //ready to shoot
                        if (AngleDiff < Angle)
                        {
                            shots++;
                            time = reloadTime;

                            bullet = (GameObject)Instantiate(bullet_prefab, transform.position, transform.rotation);
                            bullet.GetComponent<Bullet>().Origin(transform.gameObject);
                        }
                    }
                    else
                    {
                        //not ready to shoot
                        time -= Time.deltaTime;
                    }
                }
            }
            else
            {
                //cool down
                time1 -= Time.deltaTime;
            }
        }
    }
}
