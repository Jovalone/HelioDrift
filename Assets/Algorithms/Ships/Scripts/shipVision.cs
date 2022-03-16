using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shipVision : MonoBehaviour
{
    public Vector3 forward;
    public Transform transform;
    public Quaternion left, right, farLeft, farRight;
    public Quaternion a, b, c, d, e, f, g, h;
    RaycastHit2D a1, b1, c1, d1, e1, f1, g1, h1;

    int layermask = (1 << 0)|(1 << 24);

    void Update()
    {
        forward = transform.TransformDirection(Vector3.up);

        //Debug.DrawRay(transform.position, a * forward * 1, Color.green);
        //Debug.DrawRay(transform.position, b * forward * 2.5f, Color.green);
        //Debug.DrawRay(transform.position, c * forward * 3.5f, Color.green);
        //Debug.DrawRay(transform.position, d * forward * 6, Color.green);
        //Debug.DrawRay(transform.position, e * forward * 6, Color.green);
        //Debug.DrawRay(transform.position, f * forward * 3.5f, Color.green);
        //Debug.DrawRay(transform.position, g * forward * 2.5f, Color.green);
        //Debug.DrawRay(transform.position, h * forward * 1, Color.green);

        a1 = Physics2D.Raycast(transform.position, a * forward, 1, layermask);
        b1 = Physics2D.Raycast(transform.position, b * forward, 2.5f, layermask);
        c1 = Physics2D.Raycast(transform.position, c * forward, 3.5f, layermask);
        d1 = Physics2D.Raycast(transform.position, d * forward, 6, layermask);
        e1 = Physics2D.Raycast(transform.position, e * forward, 6, layermask);
        f1 = Physics2D.Raycast(transform.position, f * forward, 3.5f, layermask);
        g1 = Physics2D.Raycast(transform.position, g * forward, 2.5f, layermask);
        h1 = Physics2D.Raycast(transform.position, h * forward, 1, layermask);

        //Debug.Log(avoidDirection());
    }

    public Vector3 avoidDirection()
    {
        if(a1.collider == null && b1.collider == null && c1.collider == null && d1.collider == null && e1.collider == null && f1.collider == null && g1.collider == null && h1.collider == null)
        {
            return Vector3.zero; //return empty
        }

        Vector3 avoidVector = new Vector3();
        
        if (d1.collider != null && e1.collider != null)
        {
            if (d1.distance < e1.distance)
            {//Only use stuff from the left side
                if (a1.collider != null)
                {
                    avoidVector -= a * forward;
                }
                if (b1.collider != null)
                {
                    avoidVector -= b * forward * (2.5f - b1.distance) / 2.5f;
                }
                if (c1.collider != null)
                {
                    avoidVector -= c * forward * (3.5f - c1.distance) / 3.5f;
                }
                if (d1.collider != null)
                {
                    avoidVector -= d * forward * (6 - d1.distance) / 6;
                }
            }
            else
            {//Only use stuff from the right side
                if (h1.collider != null)
                {
                    avoidVector -= h * forward;
                }
                if (g1.collider != null)
                {
                    avoidVector -= g * forward * (2.5f - g1.distance) / 2.5f;
                }
                if (f1.collider != null)
                {
                    avoidVector -= f * forward * (3.5f - f1.distance) / 3.5f;
                }
                if (e1.collider != null)
                {
                    avoidVector -= e * forward * (6 - e1.distance) / 6;
                }
            }
            //Debug.Log("test");
            //Debug.DrawRay(transform.position, avoidVector.normalized * 2.5f, Color.red);
            return avoidVector.normalized * 4f;
        }

        if (a1.collider != null)
        {
            avoidVector -= a * forward;
        }
        if (b1.collider != null)
        {
            avoidVector -= b * forward * (2.5f - b1.distance) / 2.5f;
        }
        if (c1.collider != null)
        {
            avoidVector -= c * forward * (3.5f - c1.distance) / 3.5f;
        }
        if (d1.collider != null)
        {
            avoidVector -= d * forward * (6 - d1.distance) / 6;
        }
        if (e1.collider != null)
        {
            avoidVector -= e * forward;
        }
        if (f1.collider != null)
        {
            avoidVector -= f * forward * (2.5f - f1.distance) / 2.5f;
        }
        if (g1.collider != null)
        {
            avoidVector -= g * forward * (3.5f - g1.distance) / 3.5f;
        }
        if (h1.collider != null)
        {
            avoidVector -= h * forward * (6 - h1.distance) / 6;
        }

        //Debug.DrawRay(transform.position, avoidVector.normalized * 2.5f, Color.red);

        return avoidVector.normalized * 4f;
    }
}
