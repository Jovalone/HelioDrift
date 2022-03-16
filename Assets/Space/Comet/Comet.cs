using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comet : MonoBehaviour
{

    public Rigidbody2D rb;
    public float speed;

    void Start()
    {
        rb.velocity = transform.up * speed;
    }
}
