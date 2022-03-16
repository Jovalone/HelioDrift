using UnityEngine;

public class WormJaws : MonoBehaviour
{
    public Transform JL, JR;
    public float turnAngle, w, wr;
    int state = 0;
    Quaternion LOld;
    Quaternion ROld;
    Quaternion LNew;
    Quaternion RNew;

    public Snake snake;

    //Damage
    public int directDamage;
    public int indirectDamage;

    void Start()
    {
        LOld = JL.localRotation;
        ROld = JR.localRotation;
        LNew = Quaternion.Euler(0, 0, -10);
        RNew = Quaternion.Euler(0, 0, 10);
    }

    void Update()
    {
        if (state == 1)//Close jaws
        {
            JL.localRotation = Quaternion.RotateTowards(JL.localRotation, LNew, w * Time.deltaTime);
            JR.localRotation = Quaternion.RotateTowards(JR.localRotation, RNew, w * Time.deltaTime);
            if((JL.localRotation == LNew) || (JR.localRotation == RNew))
            {
                state = 2;
            }
        }
        else if(state == 2 || state == 0)//keep or move jaws to open position
        {
            JL.localRotation = Quaternion.RotateTowards(JL.localRotation, LOld, wr * Time.deltaTime);
            JR.localRotation = Quaternion.RotateTowards(JR.localRotation, ROld, wr * Time.deltaTime);
            if(JL.localRotation == LOld || JR.localRotation == ROld && state == 2)
            {
                state = 0;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if(snake.State == 1 && state == 0)
        {
            if (hitInfo.gameObject.name == "PlayerCollider")
            {
                state = 1;
            }
            else if (hitInfo.gameObject.name == "SquidSprite")
            {
                state = 1;
            }
            else if (hitInfo.gameObject.name == "ShipCollider")
            {
                state = 1;
            }
        }
        Hittable hittable = hitInfo.GetComponent<Hittable>();
        if (hittable != null)
        {
            if (state == 2 || state == 0)
            {
                hittable.TakeDamage(indirectDamage);
            }
            else
            {
                hittable.TakeDamage(directDamage);
            }
        }
    }
}
