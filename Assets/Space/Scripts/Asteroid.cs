using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public Transform transform;
    private Vector3 oldPosition, newPosition;
    private float Angle, oldAngle;
    public float PossibleMove = 1.5f, PossibleAngle = 20f;
    public GameObject dust;
    public Hittable hittable;
    private float ogHealth;
    private int totalDamage;

    public float BreakForce;

    void Start()
    {
        oldPosition = transform.position;
        oldAngle = Mathf.Atan2(-transform.position.x, transform.position.y) * Mathf.Rad2Deg * 180 / Mathf.PI;
        ogHealth = hittable.Health;
    }
    void Update()
    {
        if (hittable.Death)
        {
            Die();
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float collisionForce = collision.GetImpactForce();

        hittable.TakeDamage((int)(ogHealth * collisionForce / BreakForce));
        totalDamage = (int)(ogHealth * collisionForce / BreakForce);

        Hittable other = collision.gameObject.GetComponent<Hittable>();

        if(collision.gameObject.name == "Player")
        {
            other = collision.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Hittable>();
        }
        if (other != null)
        {
            other.TakeDamage((int)(ogHealth * collisionForce / BreakForce) / 10);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        float collisionForce = collision.GetImpactForce();

        hittable.TakeDamage((int)(ogHealth * collisionForce / BreakForce));

    }

    private void Die()
    {
        Instantiate(dust, transform.position, transform.rotation);
        Destroy(gameObject.transform.parent.gameObject);
    }
}

public static class Collision2DExtensions
{
    public static float GetImpactForce(this Collision2D collision)
    {
        float impulse = 0F;

        foreach (ContactPoint2D point in collision.contacts)
        {
            impulse += point.normalImpulse;
        }
        return impulse / Time.fixedDeltaTime;
    }
}


