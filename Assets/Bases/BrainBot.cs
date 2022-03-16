using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainBot : MonoBehaviour
{
    public Animator animator;
    public Transform transform, player;
    public int current = 0;

    void Update()
    {
        if(Vector2.Angle((Vector2)player.position - (Vector2)transform.position, (Vector2)transform.right * -1) < 50)
        {
            if (current == 0)
            {
                current = 1;
                animator.SetTrigger("Left");
            }
        }
        else if (Vector2.Angle((Vector2)player.position - (Vector2)transform.position, (Vector2)transform.right) < 50)
        {
            if (current == 0)
            {
                current = 2;
                animator.SetTrigger("Right");
            }
        }
        else if (current != 0)
        {
            current = 0;
            animator.SetTrigger("Straight");
        }
    }
}
