using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision : MonoBehaviour
{
    public float maxViewDist;
    public float sideViewDist;
    public float angleDiff;
    public Quaternion left, right;

    public int layermask;
    public int shieldmask;
    Vector3 forward;

    //public int a, b, c, d, e;

    public Vector3 newDirection;

    public bool tank;
    /*
    void Update()
    {
        a = LayerMask.GetMask("Default", "CosmicBody", "Shield_1");
        b = LayerMask.GetMask("Default", "CosmicBody", "Shield_2");
        c = LayerMask.GetMask("Default", "CosmicBody");
        d = LayerMask.GetMask("Shield_1");
        e = LayerMask.GetMask("Shield_2");
    }*/

    public bool checkVision()
    {
        float angle = 0;
        Vector3 rayLeft = transform.TransformDirection(Vector3.up);
        Vector3 rayRight = transform.TransformDirection(Vector3.up);

        float sightDist = maxViewDist;
        forward = transform.TransformDirection(Vector3.up);

        while (angle < 90)
        {
            RaycastHit2D raycastright = Physics2D.Raycast(transform.position, rayLeft, sightDist, layermask);
            RaycastHit2D raycastleft = Physics2D.Raycast(transform.position, rayRight, sightDist, layermask);

            //Debug.DrawRay(transform.position, rayLeft * sightDist, Color.green);
            //Debug.DrawRay(transform.position, rayRight * sightDist, Color.green);

            rayLeft = left * rayLeft;
            rayRight = right * rayRight;

            sightDist = maxViewDist * (90 - angle) / 90;
            angle += angleDiff;

            if (raycastleft.collider != null)
            {//raycast has hit
                if (!tank)
                {
                    if (shieldmask == (shieldmask | (1 << raycastleft.collider.gameObject.layer)))
                    {//ray cast has hit an enemy shield
                        if (Vector3.Angle(transform.up, -raycastleft.transform.up) < 70)
                        {
                            //ship is in collision course with shield
                            newDirection = transform.right;
                            return false;
                        }
                    }
                    else
                    {//ship has not hit a shield
                        newDirection = transform.right;
                        return false;
                    }
                }
                else
                {
                    //since it is a tank then it must swerve
                    newDirection = transform.right;
                    return false;
                }
            }

            if (raycastright.collider != null)
            {//raycast has hit
                if (!tank)
                {
                    if (shieldmask == (shieldmask | (1 << raycastright.collider.gameObject.layer)))
                    {//ray cast has hit an enemy shield
                        if (Vector3.Angle(transform.up, -raycastright.transform.up) < 70)
                        {
                            //ship is in collision course with shield
                            newDirection = -transform.right;
                            return false;
                        }
                    }
                    else
                    {//ship has not hit a shield
                        newDirection = -transform.right;
                        return false;
                    }
                }
                else
                {
                    //since it is a tank then it must swerve
                    newDirection = -transform.right;
                    return false;
                }
            }
        }
        return true;
    }
}
