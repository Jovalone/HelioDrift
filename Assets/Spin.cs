using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float rotationalSpeed;

    void Update()
    {
        transform.RotateAround(transform.position, transform.up, Time.deltaTime * rotationalSpeed);
    }

    //public void spin;
}
