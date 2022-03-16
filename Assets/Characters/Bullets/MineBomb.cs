using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineBomb : MonoBehaviour
{
    public GameObject explosion;
    public float damage;
    public float damageDistance;

    public List<Hittable> hittable;

    void OnTriggerEnter2D(Collider2D collider)
    {
        hittable.AddRange(FindObjectsOfType<Hittable>());

        Debug.Log(collider.gameObject.transform.position);

        Debug.Log(Vector3.Distance(collider.gameObject.transform.position, this.gameObject.transform.position) <= damageDistance);
        foreach (Hittable hit in hittable)
        {
            /*
            if(hit.gameObject.name == collider.gameObject.name)
            {
                //Debug.Log(Vector3.Distance(collider.gameObject.transform.position, hit.gameObject.transform.position));

                //Debug.Log(Vector3.Distance(hit.Object.transform.position, this.gameObject.transform.position));

                Debug.Log(hit.Object.transform.position);
            }*/

            if (Vector3.Distance(hit.gameObject.transform.position, this.gameObject.transform.position) <= damageDistance)
            {
                Debug.Log("dealing damage");
                float damageDealt = (damageDistance - Vector3.Distance(hit.gameObject.transform.position, this.gameObject.transform.position)) * damage / damageDistance;
                hit.TakeDamage((int)damageDealt);
                if (hit.gameObject.name == collider.gameObject.name)
                {
                    Debug.Log(damageDealt);
                }
            }
        }
        //Debug.Log(Vector3.Distance(collider.gameObject.transform.position, this.gameObject.transform.position) <= damageDistance);

        Instantiate(explosion, this.gameObject.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
