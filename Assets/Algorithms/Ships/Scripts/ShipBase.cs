using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBase : MonoBehaviour
{
    public GameObject EnemyShip, AllyShip;
    public GameObject Turret;
    GameObject temp;
    ShipAI shipAI;
    public Transform transform;
    public int numberOfShips;
    public int numberofTurrets;
    public int reserve;
    public float spawnRadius;
    public float turretSpawnRadius;
    public float maxDist, minDist;
    public bool Ally;
    private Transform Player;
    public List<GameObject> ships;
    public List<GameObject> reservelist;
    private GameObject Ship;
    public float dist;
    public GameObject Icon;
    public bool discovered;

    public bool a = false;

    public bool Active = false, Spawned = false;

    void Start()
    {
        Player = GameObject.Find("Player").transform;
    }

    void SpawnShips()
    {
        Icon.SetActive(true);
        discovered = true;

        ships = new List<GameObject>();
        Spawned = true;

        if (Ally)
        {
            Spawn(AllyShip, numberOfShips);
        }
        else
        {
            Spawn(EnemyShip, numberOfShips);
        }

        //Test
        for (int i = 0; i < numberofTurrets; i++)
        {
            Instantiate(Turret, transform.position + new Vector3(Random.Range(-turretSpawnRadius, turretSpawnRadius), Random.Range(-turretSpawnRadius, turretSpawnRadius), 0), Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
        }
        numberofTurrets = 0;
    }

    void Spawn(GameObject prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            temp = (GameObject)Instantiate(prefab, transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius), 0), Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
            shipAI = temp.GetComponent<ShipAI>();
            shipAI.SetBase(gameObject);
            ships.Add(temp);
        }
    }

    public Vector2 NewPatrolDestination()
    {
        float dist = Random.Range(minDist, maxDist);
        float angle = Random.Range(0, 2 * Mathf.PI);

        return transform.position + new Vector3(dist * Mathf.Cos(angle), dist * Mathf.Sin(angle));
    }

    bool CheckDist(Vector3 Pos)
    {
        if (Vector3.Distance(Pos, Player.position) > dist)
        {
            return false;
        }
        return true;
    }

    void Reserve(GameObject reserveShip)
    {
        reserve++;
        reservelist.Add(reserveShip);
    }

    void LateUpdate()
    {
        if (discovered)
        {
            Icon.SetActive(true);
        }

        if (Active)
        {

            if (ships.Count == 0)
            {
                if (CheckDist(transform.position))
                {
                    SpawnShips();
                }
            }
            else
            {
                if (a == false)
                {
                    StartCoroutine(NumberControl());
                }
            }
        }
    }

    IEnumerator NumberControl()
    {
        a = true;
        reservelist = new List<GameObject>();
        bool close = false;

        foreach (GameObject Ship in ships)
        {
            if(Ship == null)
            {
                //Remove(Ship);
                //numberOfShips--;
                reservelist.Add(Ship);
            }
            else
            {
                if (CheckDist(Ship.transform.position))
                {
                    close = true;
                }

                if (Ship.GetComponent<ShipAI>().Battle)
                {
                    close = true;
                }
            }
        }
        foreach(GameObject Ship in reservelist)
        {
            ships.Remove(Ship);
            numberOfShips--;
        }

        if (CheckDist(transform.position))
        {
            close = true;
        }

        if(close == false)
        {
            reserve = ships.Count;

            foreach(GameObject Ship in ships)
            {
                Destroy(Ship);
            }
            ships = new List<GameObject>();
        }
        a = false;
        yield return null;
    }
}
