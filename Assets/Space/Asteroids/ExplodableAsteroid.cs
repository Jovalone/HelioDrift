using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodableAsteroid : MonoBehaviour
{
    public Transform transform;
    public Hittable hittable;
    private float ogHealth;
    public Explodable _explodable;
    public float timedDelay;
    public GameObject dust;
    public float BreakForce;

    void Start()
    {
        ogHealth = hittable.Health;
    }

    void Update()
    {
        if (hittable.Death && this != null)
        {
            StartCoroutine(Die());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float collisionForce = collision.GetImpactForce();
        hittable.TakeDamage((int)(ogHealth * collisionForce / BreakForce));

    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(timedDelay);
        //Instantiate(dust, transform.position, transform.rotation);
        _explodable.explode();
        ExplosionForce ef = GameObject.FindObjectOfType<ExplosionForce>();
        ef.doExplosion(transform.position);
    }
}
