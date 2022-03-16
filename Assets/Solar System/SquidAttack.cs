using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidAttack : MonoBehaviour
{
    public Member member;
    public int damage;

    public Hittable hittable;
    public float hitTime;
    public float rechargeTime;
    public bool recharging;

    public ParticleSystem particleSystem;
    public Collider2D squidCollider;

    void Update()
    {
        /*
        foreach (GameObject enemy in hittable.Attackers)
        {
            if ((Vector3.Distance(hitspot.position, enemy.transform.position) < hitRange) && !recharging)
            {
                StartCoroutine(squidAttack());
            }
        }
        */

        if(hittable.Attackers.Count > 0)
        {
            squidCollider.enabled = true;
        }
    }

    IEnumerator squidAttack()
    {
        recharging = true;
        particleSystem.Play();

        yield return new WaitForSeconds(hitTime);

        squidCollider.enabled = false;

        yield return new WaitForSeconds(rechargeTime);

        recharging = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Hittable enemyHittable = collider.GetComponent<Hittable>();
        if(enemyHittable != null)
        {
            enemyHittable.TakeDamage(damage);
        }

        if (!recharging)
        {
            StartCoroutine(squidAttack());
        }
    }
}
