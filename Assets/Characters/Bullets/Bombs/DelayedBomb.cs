using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedBomb : MonoBehaviour
{

    public GameObject explosion;
    public float time;
    public float damage;
    public float damageDistance;

    private List<Hittable> hittable;

    void Start()
    {
        hittable = new List<Hittable>();
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(time);

        hittable.AddRange(FindObjectsOfType<Hittable>());

        foreach(Hittable hit in hittable)
        {
            if(Vector3.Distance(hit.Object.transform.position, this.gameObject.transform.position) <= damageDistance)
            {
                float damageDealt = (damageDistance - Vector3.Distance(hit.Object.transform.position, this.gameObject.transform.position)) * damage / damageDistance;
                hit.TakeDamage((int)damageDealt);
            }
        }

        Instantiate(explosion, this.gameObject.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
