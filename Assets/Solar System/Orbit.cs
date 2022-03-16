using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Transform transform;
    public Rigidbody2D rb;
    public List<GravitySource> gravitySources;
    private GravitySource center;
    private Vector3 Dir, Acc, Vel, w;
    private float dist;

    private bool planet = false;

    void Start()
    {
        gravitySources = new List<GravitySource>();
        gravitySources.AddRange(FindObjectsOfType<GravitySource>());

        if (gravitySources.Count > 0)
        {
            SelectCenter();
            Vel = VelocityDirection().normalized;

            rb.velocity = Vel * Mathf.Sqrt(center.OrbitStrength / dist);
        }

    }

    void Update()
    {
        if(gravitySources.Count > 0)
        {
            //SelectCenter();

            Vel = VelocityDirection();
            rb.velocity = Vel * Mathf.Sqrt(center.OrbitStrength / Mathf.Pow(dist, 2));
        }
        else
        {
            gravitySources.AddRange(FindObjectsOfType<GravitySource>());
            SelectCenter();
        }
    }

    void SelectCenter()
    {
        dist = Mathf.Infinity;
        foreach(GravitySource source in gravitySources)
        {
            if(source != null)
            {
                if (Vector3.Distance(source.transform.position, transform.position) / source.OrbitStrength < dist && Vector3.Distance(source.transform.position, transform.position) != 0)
                {
                    dist = Vector3.Distance(source.transform.position, transform.position) / source.OrbitStrength;
                    center = source;
                }

                if (Vector3.Distance(source.transform.position, transform.position) == 0)
                {
                    planet = true;
                }
            }
        }
    }

    Vector3 VelocityDirection()
    {
        if (center.OrbitDirection)
        {
            w = new Vector3(0, 0, center.OrbitStrength);
        }
        else
        {
            w = new Vector3(0, 0, -center.OrbitStrength);
        }

        Vector3 r = (center.transform.position - transform.position);
        return Vector3.Cross(w, r);
    }

    Vector3 Acceleration()
    {
        Vector3 r = (center.transform.position - transform.position).normalized;
        Debug.Log(r * (center.GravityStrength / Mathf.Pow(dist, 2)));
        
        return r * (center.GravityStrength / Mathf.Pow(dist, 2));
    }
}
