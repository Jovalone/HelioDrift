using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip : MonoBehaviour
{
    public GameObject player;
    private Rigidbody2D rb;
    private Player PlayerScript;
    private Boost boost;
    public FirePoint firePoint;
    public TrailRenderer Normal;
    public gravity Gravity;

    Animator animator;
    
    public float drain;
    private int i = 0;
    float lastTimeClicked = 0;

    void Start()
    {
        player = GameObject.Find("Player");
        rb = player.GetComponent<Rigidbody2D>();
        PlayerScript = player.GetComponent<Player>();
        boost = player.GetComponent<Boost>();
        animator = player.GetComponent<Animator>();
    }

    public void HitBrakes()
    {
        lastTimeClicked = Time.time;
        i = 1;
        if (Input.GetKey(KeyCode.Return))
        {
            ActivateFlip();

        }
        else
        {
            StartCoroutine(boost.overHeating());
        }
    }

    public void ActivateFlip()
    {
        //Activate
        animator.SetTrigger("Turn");
        PlayerScript.moveVelocity = 0;
        PlayerScript.RollDist = 0;
        PlayerScript.LoseEnergy(drain);
        firePoint.enabled = false;
        Normal.enabled = false;
        PlayerScript.enabled = false;
        Gravity.enabled = false;
    }

    public void Rotate()
    {
        Gravity.enabled = true;
        Normal.enabled = true;
        firePoint.enabled = true;
        PlayerScript.enabled = true;
        animator.SetTrigger("TurnEnd");
        rb.rotation += 180;
    }
}
