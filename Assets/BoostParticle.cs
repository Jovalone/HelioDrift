using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostParticle : MonoBehaviour
{

    public float rate;
    public float lifeTime;
    public bool active;
    public float chance;
    public GameObject spark;
    public Trail trail;

    void Update()
    {
        if (active)
        {
            chance = Random.Range(0f, 1f) / rate;//Will need to rework code
            if (chance < Time.deltaTime)
            {
                Debug.Log("shooting");
                Instantiate(spark, transform.position, transform.rotation);
            }
        }
    }
    public void Activate()
    {
        trail.startRoutine(4,0.075f);
        StartCoroutine(firing());
    }

    public IEnumerator firing()
    {
        Debug.Log("firing");
        active = true;
        yield return new WaitForSeconds(lifeTime);
        active = false;
    }
}
