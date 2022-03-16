using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trailWidth : MonoBehaviour
{

    public float width;

    public TrailRenderer trail;
    void Start()
    {
        trail.startWidth = width;
    }
}
