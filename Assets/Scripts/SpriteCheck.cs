using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCheck : MonoBehaviour
{
    public Transform transform, player;
    public SpriteRenderer sprite;
    private float viewDist = 35;
    public bool visable;
    public bool waiting = false;

    void Start()
    {
        transform = this.gameObject.transform;
        player = GameObject.Find("Player").transform;
        sprite = this.gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        /*
        if (!waiting)
        {
            StartCoroutine(Check());
        }\
        */

        if (Vector3.Distance(transform.position, player.position) < viewDist)
        {
            sprite.enabled = true;
            visable = true;
        }
        else
        {
            sprite.enabled = false;
            visable = false;
        }
    }

    IEnumerator Check()
    {
        waiting = true;
        if (Vector3.Distance(transform.position, player.position) < viewDist)
        {
            sprite.enabled = true;
            visable = true;
        }
        else
        {
            sprite.enabled = false;
            visable = false;
        }

        yield return new WaitForSeconds(0.2f);
        waiting = false;
    }
}
