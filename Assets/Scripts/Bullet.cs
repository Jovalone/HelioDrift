using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public bool Homing;
    public int damage;
    public float speed;
    public Rigidbody2D rb;
    public GameObject bulletImpact;
    public Transform bullet;
    public GameObject origin;
    public Transform Target;
    public float RotationalSpeed;
    public float MaxDist;
    public int durability;
    private Vector3 ogPos;

    //SizeFluctuation
    public float period;
    public float Amplitude;
    public float ogSize;
    private float timeOffset;


    void Start()
    {
        rb.velocity = transform.up * speed;
        Physics2D.IgnoreLayerCollision(8, 10);
        ogPos = bullet.position;
        timeOffset = Random.Range(-Mathf.PI, Mathf.PI);
    }

    void FixedUpdate()
    {
        if (Homing && Target != null)
        {
            Vector2 Direction = ((Vector2)Target.position - rb.position).normalized;

            float angle = Mathf.Atan2(Direction.x, Direction.y) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.back);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, RotationalSpeed * Time.fixedDeltaTime);
            rb.velocity = bullet.up * speed;
        }
        //destroy bullet if it gets too far
        if (Vector3.Distance(bullet.position, ogPos) > MaxDist)
        {
            Destroy(gameObject);
        }

        //SizeFluctuation
        float size = Mathf.Sin((Time.time + timeOffset) / period) * Amplitude + ogSize;
        transform.localScale = new Vector3(size, size, size);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.gameObject.name != "ShipShield")
        {
            Hit(hitInfo);
        }
        else if (Vector2.Angle(-rb.velocity, (Vector2)hitInfo.transform.up) < 70)
        {//check if they are moving in the right direction
            Hit(hitInfo);
        }
    }

    void Hit(Collider2D hitInfo)
    {
        Hittable Object = hitInfo.GetComponent<Hittable>();

        if (Object != null)
        {
            Object.TakeDamage(damage);
            Object.Attackers.Add(origin);

            if (Object.Health <= 0)
            {
                Instantiate(bulletImpact, bullet.position, bullet.rotation);
            }
            else
            {
                Instantiate(bulletImpact, bullet.position, bullet.rotation, Object.gameObject.transform);
            }
        }


        durability--;

        if (durability <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Origin(GameObject firepoint)
    {
        origin = firepoint;
    }

    public void SetTarget(Transform target)
    {
        Target = target;
    }
}