using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOn : MonoBehaviour
{
    public Transform transform, target;
    private bool active = false;
    void Update()
    {
        if(target != null)
        {
            transform.position = target.position;
            active = true;
        }
        else if (active)
        {
            Destroy(gameObject);
        }
    }

    public void SetTarget(Transform Target)
    {
        target = Target;
    }
}
