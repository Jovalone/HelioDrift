using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetTracker : MonoBehaviour
{
    public Ship ship;
    void FixedUpdate()
    {
        transform.position = (Vector3)ship.Target;
    }
}
