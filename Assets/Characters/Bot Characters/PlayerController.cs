using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float Speed;
    public Rigidbody2D rb;
    public Transform transform;

    Vector2 Movement;
    public Animator animator;

    Vector3 characterScale;
    float characterScaleX;

    void Start()
    {
        characterScale = transform.localScale;
        characterScaleX = characterScale.x;
    }

    void Update()
    {
        //Input
        Movement.x = Input.GetAxisRaw("Horizontal");
        Movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("X", Movement.x);
        animator.SetFloat("Y", Movement.y);
        animator.SetFloat("Speed", Movement.sqrMagnitude);

        // Flip the Character:
        if (Movement.x < 0)
        {
            characterScale.x = -characterScaleX;
        }
        if (Movement.x > 0)
        {
            characterScale.x = characterScaleX;
        }
        transform.localScale = characterScale;

        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }

    void FixedUpdate()
    {
        //Movement
        rb.MovePosition(rb.position + Movement * Speed * Time.fixedDeltaTime);
    }
}