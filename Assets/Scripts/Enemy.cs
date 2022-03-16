using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public int health;
    public GameObject Squid;
    public Member member;
    public SquidBoid squid;
    public Hittable hittable;
    private bool dead = false;

    void Start()
    {
        health = (int)hittable.Health;
        animator.Play("Squid_1", 0, Random.Range(0f,1f));
    }
    void Update()
    {
        if(health != (int)hittable.Health)
        {
            health = (int)hittable.Health;
            animator.SetInteger("Health", health);
            if (health <= 0 && !dead)
            {
                animator.SetTrigger("Death");
                member.death();
                member.enabled = false;
                squid.enabled = false;
                dead = true;

                if (hittable.Attackers.Contains(Player.playerInstance.gameObject))
                {
                    RecordKeeper.Record.statistics["SquidKillsRecent"]++;
                    Debug.Log(RecordKeeper.Record.statistics["SquidKillsRecent"]);
                }

                Invoke("Die", 1);

            }
            animator.SetTrigger("Hit");
        }
    }

    public void Die()
	{
        Destroy(Squid);
	}
}
