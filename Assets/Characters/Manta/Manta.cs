using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manta : MonoBehaviour
{
    public Transform transform;
    public Rigidbody2D rb;
    public Vector3 forward, direction, Direction;
    public Quaternion a, b, c, d, e, f, g, h;
    RaycastHit2D a1, b1, c1, d1, e1, f1, g1, h1;
    public float a2, b2, c2, d2, e2, f2, g2, h2;
    int layermask = (1 << 0) | (1 << 24);
    public float speed = 5f;
    public float RotationalSpeed;
    public float wander_Ri, wander_Ro;
    
    void Start()
    {
        Level.levelInstance.Mantas.Add(this);
    }

    void Update()
    {
        Direction = avoidDirection();
        if(Direction == new Vector3())
        {
            float dist = Vector3.Distance(transform.position, new Vector3());
            if(dist > wander_Ro)
            {
                Direction = -transform.position.normalized;
            }
            if(dist < wander_Ri)
            {
                Direction = transform.position.normalized;
            }
        }
        direction += Direction;

        if(Direction != Vector3.zero)
        {
            StartCoroutine(rememberVector(Direction));
        }
        Move();
    }

    void Move()
    {
        //Calculate new position
        float newX = speed * Mathf.Cos(transform.eulerAngles.z * Mathf.PI / 180);
        float newY = -speed * Mathf.Sin(transform.eulerAngles.z * Mathf.PI / 180);

        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.back);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, RotationalSpeed * Time.deltaTime);
        rb.velocity = new Vector3(newY, newX, 0);
    }

    IEnumerator rememberVector(Vector3 vector)
    {
        yield return new WaitForSeconds(2);
        direction -= vector;
    }

    void RayCast()
    {
        forward = transform.TransformDirection(Vector3.up);

        a1 = Physics2D.Raycast(transform.position, a * forward, a2, layermask);
        b1 = Physics2D.Raycast(transform.position, b * forward, b2, layermask);
        c1 = Physics2D.Raycast(transform.position, c * forward, c2, layermask);
        d1 = Physics2D.Raycast(transform.position, d * forward, d2, layermask);
        e1 = Physics2D.Raycast(transform.position, e * forward, e2, layermask);
        f1 = Physics2D.Raycast(transform.position, f * forward, f2, layermask);
        g1 = Physics2D.Raycast(transform.position, g * forward, g2, layermask);
        h1 = Physics2D.Raycast(transform.position, h * forward, h2, layermask);
    }

    public Vector3 avoidDirection()
    {
        RayCast();

        if (a1.collider == null && b1.collider == null && c1.collider == null && d1.collider == null && e1.collider == null && f1.collider == null && g1.collider == null && h1.collider == null)
        {
            return Vector3.zero; //return empty
        }


        Vector3 avoidVector = new Vector3();

        if (a1.collider != null)
        {
            avoidVector -= a * forward;
        }
        if (b1.collider != null)
        {
            avoidVector -= b * forward * (b2 - b1.distance) / b2;
        }
        if (c1.collider != null)
        {
            avoidVector -= c * forward * (c2 - c1.distance) / c2;
        }
        if (d1.collider != null)
        {
            avoidVector -= d * forward * (d2 - d1.distance) / d2;
        }
        if (e1.collider != null)
        {
            avoidVector -= e * forward;
        }
        if (f1.collider != null)
        {
            avoidVector -= f * forward * (f2 - b1.distance) / f2;
        }
        if (g1.collider != null)
        {
            avoidVector -= g * forward * (g2 - g1.distance) / g2;
        }
        if (h1.collider != null)
        {
            avoidVector -= h * forward * (h2 - h1.distance) / h2;
        }

        return avoidVector.normalized * 4f;
    }

}
