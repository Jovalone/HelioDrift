using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gravity : MonoBehaviour
{
    public List<GravitySource> gravitySources;
    public Transform transform;
    public Rigidbody2D rb;

    private float dist;
    private Vector3 ForceDir;

    private bool first = true;
    /*
    void Start()
    {
        gravitySources = new List<GravitySource>();
        gravitySources.AddRange(FindObjectsOfType<GravitySource>());
    }
    */
    void Update()
    {
        if (first)
        {
            gravitySources = new List<GravitySource>();
            gravitySources.AddRange(FindObjectsOfType<GravitySource>());
            first = false;
        }

        foreach(GravitySource source in gravitySources)
        {
            dist = Vector3.Distance(source.transform.position, transform.position);

            if(dist < source.distance)
            {
                ForceDir = (source.transform.position - transform.position).normalized;

                rb.AddForce(ForceDir * source.GravityStrength * (source.distance - dist) * Time.deltaTime, ForceMode2D.Impulse);
            }
        }
    }
}
