using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarLayer : MonoBehaviour
{
    public Transform transform;
    public GameObject[] stars;
    public int[] numberStars;
    public float[] speed;
    public float spawnRadius, innerSpawnRadius;
    public Vector3 Offset;

    public List<GameObject> Stars;

    void Start()
    {
        for(int j = 0; j < numberStars.Length; j++)
        {
            for (int i = 0; i < numberStars[j]; i++)
            {//should change to a circular spawn system
                GameObject s = (GameObject)Instantiate(stars[j], transform.position + Offset + new Vector3(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius), 0), Quaternion.identity, transform);
                if (Vector3.Distance(transform.position + Offset, s.transform.position) < spawnRadius)
                {
                    Stars.Add(s);
                    s.GetComponent<Star>().j = j;

                }
                else
                {
                    Destroy(s);
                }
            }
        }
    }

    void Update()
    {
        Vector3 direction = new Vector3();

        foreach (GameObject Star in Stars)
        {
            direction = (transform.position + Offset - Star.transform.position).normalized;
            Star s = Star.GetComponent<Star>();
            Star.transform.position -= direction * speed[s.j] * Time.deltaTime;

            if(Vector3.Distance(transform.position + Offset, Star.transform.position) > spawnRadius)
            {
                s.moved();
                Star.transform.position = transform.position + Offset + StarPos();
            }
        }
    }

    Vector3 StarPos()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        float s = Mathf.Pow(x, 2) + Mathf.Pow(y, 2);
        while(0 > s || s > 1)
        {
            x = Random.Range(-1f, 1f);
            y = Random.Range(-1f, 1f);
            s = Mathf.Pow(x, 2) + Mathf.Pow(y, 2);
        }
        //Debug.Log(s);

        x = innerSpawnRadius * x * Mathf.Sqrt(-2 * Mathf.Log(s)) / Mathf.Log(s);
        y = innerSpawnRadius * y * Mathf.Sqrt(-2 * Mathf.Log(s)) / Mathf.Log(s);

        //Debug.Log(x);
        //Debug.Log(y);

        return new Vector3(x, y, 0);

    }
}
