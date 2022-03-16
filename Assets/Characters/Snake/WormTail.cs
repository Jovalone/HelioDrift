using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormTail : MonoBehaviour
{
    public Rigidbody2D rb;

    public float x, force;//, y;

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        //y = Input.GetAxisRaw("Vertical");

        rb.AddForce(this.gameObject.transform.right * x * force);
    }
}
