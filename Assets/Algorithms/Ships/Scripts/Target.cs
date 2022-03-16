using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public Transform ship, target, transform;
    Vector3 NewPos, OldPos, Direction;
    float speed, dist, K, AngleDiff;
    public float BulletSpeed;
    private float OffsetDistance0;
    private Vector3 Offset;
    public bool Battle;

    void Start()
    {
        OldPos = transform.position;
        NewPos = transform.position;
    }

    void Update()
    {
        if (Battle)
        {
            //Calculate target speed
            speed = Vector3.Distance(NewPos, OldPos) / Time.deltaTime;
            OldPos = NewPos;
            //Calculate direction which should be extended
            Direction = ((Vector2)NewPos - (Vector2)ship.position).normalized;

            AngleDiff = (target.eulerAngles.z - ship.eulerAngles.z) * Mathf.PI / 180;

            dist = Vector3.Distance(target.position, ship.position);

            K = BulletSpeed / speed;

            float OffsetDistance1 = (-2 * dist * Mathf.Cos(AngleDiff) + Mathf.Sqrt((Mathf.Pow((2 * dist * Mathf.Cos(AngleDiff)), 2) + 4 * (K - 1) * Mathf.Pow(dist, 2)))) / (2 * (K - 1));
            float OffsetDistance2 = (-2 * dist * Mathf.Cos(AngleDiff) - Mathf.Sqrt((Mathf.Pow((2 * dist * Mathf.Cos(AngleDiff)), 2) + 4 * (K - 1) * Mathf.Pow(dist, 2)))) / (2 * (K - 1));

            if (OffsetDistance1 > OffsetDistance2)
            {
                OffsetDistance0 = OffsetDistance1;
            }
            else
            {
                OffsetDistance0 = OffsetDistance2;
            }
            //create new Vector

            float X = -OffsetDistance0 * Mathf.Sin(target.eulerAngles.z * Mathf.PI / 180);
            float Y = OffsetDistance0 * Mathf.Cos(target.eulerAngles.z * Mathf.PI / 180);

            //Offset = new Vector3(0, OffsetDistance0, 0);
            Offset = target.position + new Vector3(X, Y, 0);


            if (AngleDiff == 0 || AngleDiff == 180)
            {

                Offset = new Vector3(0, 0, 0);

            }
            else if (speed == 0f)
            {

                Offset = target.position;

            }
            else if (float.IsNaN(Offset.x) || float.IsNaN(Offset.y))
            {
                transform.position = ship.position;
            }
            else
            {
                transform.position = Offset;
            }
        }
    }

    public void UpdateTarget(Vector3 position)
    {
        NewPos = position;
    }

    public void SetShip(Transform transfrom)
    {//Creator ship sends script its transform to be used for the calculation.
        //might not be needed since I could change the variable directly from the other script
        //will also need to chang bullet speed
        ship = transform;
    }
}
