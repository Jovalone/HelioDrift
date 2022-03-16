using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidSwarmTarget : MonoBehaviour
{

    public Transform transform, Target;
    Vector3 Pos;

    void Start()
    {
        Pos = transform.position;
    }

    void Update()
    {
        if(Target != null)
        {
            Pos = Target.position;
            transform.position = Pos;
        }
        else
        {
            transform.position = Pos;
        }
    }

    public void Follow(Transform target)
    {
        Target = target;
    }
}
