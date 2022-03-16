using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flare : MonoBehaviour
{
    public GameObject flare;
    private float chance;
    public bool active = false;
    public bool cooling = false;
    public float rate;
    public float lifeTime;

    void Update()
    {
        if (active)
        {
            chance = Random.Range(0f, 1f) / rate;
            if (chance < Time.deltaTime)
            {
                Instantiate(flare, transform.position, transform.rotation);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.G) && !cooling)
            {
                StartCoroutine(firing());
            }
        }
    }

    IEnumerator firing()
    {
        active = true;
        yield return new WaitForSeconds(lifeTime);
        StartCoroutine(Cooling());
        active = false;
    }

    IEnumerator Cooling()
    {
        cooling = true;
        yield return new WaitForSeconds(1.5f);
        cooling = false;
    }
}
