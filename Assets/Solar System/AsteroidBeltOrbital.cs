using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBeltOrbital : MonoBehaviour
{

    public GameObject asteroid;
    private GameObject temp;
    private Transform Player;
    public float Ri, Ro;
    public int small, med, large;
    public int quantity;
    private int OGquantity;
    public Transform transform;

    private float dist, angle;
    private Vector3 Offset;

    public List<GameObject> AsteroidList;
    public bool spawned = false, activated = false;
    private float Playerdist;

    void Start()
    {
        Player = GameObject.Find("Player").transform;
    }

    void FixedUpdate()
    {
        if (spawned)
        {
            Playerdist = Vector3.Distance(Player.position, transform.position);
            if (activated)
            {
                if ((Playerdist < (Ri - 30)) || (Playerdist > (Ro + 30)))
                {
                    //despawn
                    activated = false;
                    DeSpawnAsteroids();
                }
            }
            else
            {
                if ((Playerdist > (Ri - 30)) && (Playerdist < (Ro + 30)))
                {
                    //spawn
                    activated = true;
                    ReSpawnAsteroidBelt();
                }
            }
        }
    }

    public void SpawnAsteroidBelt()
    {
        AsteroidList = new List<GameObject>();
        OGquantity = quantity;
        for(int i = 0; i < quantity; i++)
        {
            SpawnAsteroid();
        }
        spawned = true;
        activated = true;
    }

    public void ReSpawnAsteroidBelt()
    {
        AsteroidList = new List<GameObject>();
        for (int i = 0; i < quantity; i++)
        {
            SpawnAsteroid();
        }
    }

    void SpawnAsteroid()
    {
        dist = Random.Range(Ri, Ro);
        angle = Random.Range(0, 2 * Mathf.PI);
        Offset = transform.position + new Vector3(dist * Mathf.Cos(angle), dist * Mathf.Sin(angle), 0);

        temp = (GameObject)Instantiate(asteroid, Offset, Quaternion.identity);
        AsteroidList.Add(temp);
    }

    void DeSpawnAsteroids()
    {
        quantity = OGquantity;

        foreach(GameObject temp in AsteroidList)
        {
            if(temp == null)
            {
                quantity = quantity - 1;
            }
            else
            {
                Destroy(temp);
            }
        }
    }
}
