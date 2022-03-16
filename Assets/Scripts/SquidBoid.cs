using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidBoid : MonoBehaviour
{
    public Member member;
    public Transform transform, MemberTransform;
    private float Angle, oldAngle;
    private Vector3 oldPosition, newPosition;
    public float coolDown = 0.05f;
    public float MinSpeed;

    void Start()
	{
        oldPosition = MemberTransform.position;
        oldAngle = 0;
    }

    // Update is called once per frame
    void Update()
    {
        newPosition = MemberTransform.position;

        Vector3 movement = newPosition - oldPosition;

        Vector3 dir = movement;
        Angle = Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg;




        if (movement.magnitude > MinSpeed)
        {
            transform.rotation = Quaternion.AngleAxis(Angle, Vector3.forward);
        }
        oldPosition = newPosition;

    }
}
