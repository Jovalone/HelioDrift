using UnityEngine;

public class MultishotEnemy : MonoBehaviour
{
    private float time = 0;
    public float delay = 1;

    public Transform firePoint, target;
    public GameObject AllyBullet, EnemyBullet, Object;
    private GameObject bullet;
    GameObject Bullet;

    //public GameObject player;
    public float K;
    float Angle, AngleDiff;
    public ShipAI ship;
    public ShipBody shipBody;

    void Update()
    {
        if(ship == null)
        {
            if(shipBody.AI != null)
            {
                ship = shipBody.AI;

                if (ship.Ally)
                {
                    Bullet = AllyBullet;
                }
                else
                {
                    Bullet = EnemyBullet;
                }
            }
        }
        else if (ship.Battle && ship.target.transform != null)
        {
            Angle = K / (Mathf.Sqrt(Vector3.Distance(firePoint.position, ship.target.transform.position)));
            Vector3 Direction = (ship.target.transform.position - firePoint.position).normalized;
            AngleDiff = Vector3.Angle(firePoint.up, Direction);



            time += Time.deltaTime;
            if (AngleDiff < Angle)
            {


                if (time > delay)
                {
                    time = 0;
                    bullet = (GameObject)Instantiate(Bullet, firePoint.position, firePoint.rotation);
                    bullet.GetComponent<Bullet>().Origin(firePoint.gameObject);
                }
            }
        }
    }
}
