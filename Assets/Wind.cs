using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public float distance;
    public float dist;
    public bool timer = true;
    public bool active;
    public float width, length;
    public GameObject wind;
    public Transform player;
    public Transform transform;
    private float chance;

    void Start()
    {
        transform = this.gameObject.transform;
        player = GameObject.Find("Player").transform;
        StartCoroutine(Timer(1));
    }

    public void Update()
    {
        //check if timer is done
        if (timer)
        {
            //Start cooldown
            dist = (Vector3.Distance(transform.position, player.position));
            StartCoroutine(Timer(1 + (int)(dist / 50)));
            if(dist < distance)
            {
                active = true;
            }
            else
            {
                active = false;
            }
        }

        if (active)
        {
            //Spawn wind
            chance = Random.Range(0f, 1f) / 12;
            if (chance < Time.deltaTime)
            {
                Vector3 Offset = /*transform.rotation */ new Vector3(Random.Range(-width, width), Random.Range(-length, length), 0);
                Instantiate(wind, transform.position + Offset, transform.rotation);
            }
        }
    }

    IEnumerator Timer(int factor)
    {
        timer = false;
        yield return new WaitForSeconds(0.5f * factor);
        timer = true;
    }
}
