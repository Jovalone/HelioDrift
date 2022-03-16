using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBelt : MonoBehaviour
{
    public GameObject asteroid_Large, asteroid_Medium, asteroid_Small;
    private GameObject temp;
    private Transform Player;
    public float Ri, Ro;
    public int small, medium, large;
    public Transform transform;
    public Transform asteroidHolder;
    public GameObject holder;

    private float dist, angle;
    private Vector3 Pos;

    public List<GameObject> AsteroidList;
    public List<GravitySource> gravitySources;
    float orbitStrength;
    private GravitySource center;
    public bool spawned = false, activated = false;
    private float Playerdist;
    public float[] spawnDistance;
    private int size;
    private int Num = 0;
    public Vector3[] allLocations;
    public Quaternion rotationoffset;

    void Start()
    {
        Player = GameObject.Find("Player").transform;
        spawned = true;
    }

    void Update()
    {
        if (spawned)
        {
            if (AsteroidList.Count == 0)
            {
                SpawnAsteroidBelt();
            }

            Playerdist = Vector3.Distance(Player.position, transform.position);
            if (activated)
            {
                Rotate();

                if ((Playerdist < (Ri - 30)) || (Playerdist > (Ro + 30)))
                {
                    //despawn
                    holder.SetActive(false);
                    activated = false;
                }
            }
            else
            {
                if ((Playerdist > (Ri - 30)) && (Playerdist < (Ro + 30)))
                {
                    //spawn
                    activated = true;
                    holder.SetActive(true);
                }
            }
        }
    }

    public void SpawnAsteroidBelt()
    {
        AsteroidList = new List<GameObject>();
        allLocations = new Vector3[small + medium + large];
        size = 0;
        Num = 0;

        for (int i = 0; i < large; i++)
        {
            SpawnAsteroid(2);
            Num++;
        }
        size++;
        for (int i = 0; i < medium; i++)
        {
            SpawnAsteroid(1);
            Num++;
        }
        size++;
        for (int i = 0; i < small; i++)
        {
            SpawnAsteroid(0);
            Num++;
        }
        spawned = true;
        activated = true;
    }

    void SpawnAsteroid(int x)
    {
        dist = Random.Range(Ri, Ro);
        angle = Random.Range(0, 2 * Mathf.PI);
        Pos = FindNewLocation(Ri, Ro);//transform.position + new Vector3(dist * Mathf.Cos(angle), dist * Mathf.Sin(angle), 0);

        if (x == 0)
        {
            temp = (GameObject)Instantiate(asteroid_Small, Pos, Quaternion.identity, asteroidHolder);
            AsteroidList.Add(temp);
        }else if (x == 1)
        {
            temp = (GameObject)Instantiate(asteroid_Medium, Pos, Quaternion.identity, asteroidHolder);
            AsteroidList.Add(temp);
        }else if (x == 2)
        {
            temp = (GameObject)Instantiate(asteroid_Large, Pos, Quaternion.identity, asteroidHolder);
            AsteroidList.Add(temp);
        }
    }

    void DeSpawnAsteroids()
    {
        foreach (GameObject temp in AsteroidList)
        {
            if (temp != null)
            {
                Destroy(temp);
            }
        }
    }

    Vector3 FindNewLocation(float ri, float ro)
    {
        dist = Random.Range(ri, ro);
        angle = Random.Range(0, 2 * Mathf.PI);
        Vector3 Location = transform.position + new Vector3(dist * Mathf.Cos(angle), dist * Mathf.Sin(angle), 0);
        bool redo = false;

        for (int n = 0; n < Num; n++)
        {
            if (Vector3.Distance(Location, allLocations[n]) < spawnDistance[size])
            {
                redo = true;
            }
        }

        if (redo)
        {
            Location = FindNewLocation(ri, ro + 0.01f);
        }
        allLocations[Num] = Location;
        return Location;
    }

    public void Rotate()
    {
        if(gravitySources.Count == 0)
        {
            gravitySources = new List<GravitySource>();
            gravitySources.AddRange(FindObjectsOfType<GravitySource>());
            if(gravitySources.Count > 0)
            {
                SelectCenter();
            }
        }
        else
        {
            float w = (Mathf.Sqrt(orbitStrength * Ri) / Ri) * 25 / Mathf.PI;
            asteroidHolder.Rotate(Vector3.forward * w * Time.deltaTime);

            foreach(GameObject temp in AsteroidList)
            {
                if(temp != null)
                {
                    Vector3 dir = rotationoffset * (center.gameObject.transform.position - temp.transform.position);
                    //Vector3 dir = (center.gameObject.transform.position - temp.transform.position);

                    float atan2 = Mathf.Atan2(dir.y, dir.x);
                    temp.transform.rotation = Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg);
                }
            }
        }
    }

    void SelectCenter()
    {
        dist = Mathf.Infinity;
        foreach (GravitySource source in gravitySources)
        {
            if (Vector3.Distance(source.transform.position, transform.position) / source.OrbitStrength < dist)
            {
                center = source;
            }
        }
        orbitStrength = center.OrbitStrength;
    }
}
