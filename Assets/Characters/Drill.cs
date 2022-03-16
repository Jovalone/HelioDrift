using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour
{
    public float time;
    public Animator animator;

    void OnTriggerEnter2D(Collider2D collider)
    {
        transform.SetParent(collider.gameObject.transform);
        StartCoroutine(drillDelay());
    }

    IEnumerator drillDelay()
    {
        yield return new WaitForSeconds(time);
        animator.SetTrigger("Begin");
    }
}
