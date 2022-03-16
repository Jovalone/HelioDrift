using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidSpawn : MonoBehaviour
{
    public bool Active = false, Spawned = false;
    public GameObject Squid;
    public Transform Leader;
    public int Quantity;
    public int reserve;
    public float SpawnBounds;
    public float bounds;
    public Color colour;
    public Level level;

    public List<Member> members;
    public List<Member> reserveList;
    public List<Member> destroyedList;
    public List<EnemyBoid> enemies;
    public List<EnemyBoid> totalEnemies;
    public float enemyMaxDist;

    public Transform position, target;
    public Transform squidHolder;

    private GameObject memberObject;
    private Member member;
    private Transform Player;
    public float dist;
    public GameObject Icon;
    public bool discovered;

    private bool a = false;
    private bool collecting = false;

    void Start()
    {
        Player = GameObject.Find("Player").transform;
        level = FindObjectOfType<Level>();
    }

    public void SpawnSquid()
    {

        Icon.SetActive(true);
        discovered = true;

        members = new List<Member>();
        enemies = new List<EnemyBoid>();
        Spawn(Squid, Quantity);
        Spawned = true;
    }

    void LateUpdate()
    {
        if (discovered)
        {
            Icon.SetActive(true);
        }

        if (Active)
        {

            if (members.Count == 0)
            {
                if (CheckDist(position.position))
                {
                    SpawnSquid();
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
        reserveList = new List<Member>();
        destroyedList = new List<Member>();
        a = true;
        bool close = false;
        int test = 0;

        SelectNewLeader();
        if (CheckDist(Leader.position))
        {
            //Spawn reserve squids
            SpawnReserves(Squid, reserve);
            reserve = 0;
        }

        foreach (Member squid in members)
        {
            if(squid == null)
            {
                destroyedList.Add(squid);
            }
            else if (CheckDist(squid.position))
            {
                close = true;
                test++;
            }
        }

        foreach(Member squid in destroyedList)
        {
            members.Remove(squid);
        }

        if (CheckDist(position.position))
        {
            close = true;
            test++;
        }
        if (close == false)
        {
            reserveList = members;

            foreach (Member squid in reserveList)
            {
                Reserve(squid);
            }
            members = new List<Member>();
        }
        a = false;
        yield return null;
    }

    void SpawnReserves(GameObject prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            memberObject = (GameObject)Instantiate(prefab, Leader.position + new Vector3(Random.Range(-SpawnBounds / 4, SpawnBounds / 4), Random.Range(-SpawnBounds / 4, SpawnBounds / 4), 0), Quaternion.identity, squidHolder);
            member = memberObject.GetComponent<Member>();
            members.Add(member);
            member.spawn = this.gameObject.GetComponent<SquidSpawn>();

        }
    }

    void Reserve(Member squid)
    {
        reserve++;
        Destroy(squid.gameObject);
    }

    bool CheckDist(Vector3 Pos)
    {
        if(Vector3.Distance(Pos, Player.position) > dist)
        {
            return false;
        }
        return true;
    }

    void SelectNewLeader()
    {
        if (members.Count != 0)
        {
            int i = 0;
            while (members[i] == null)
            {
                i++;
            }
            Leader = members[i].gameObject.transform;//leader;
            Leader.gameObject.GetComponent<Member>().Leader = true;
            Leader.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = colour;
            target.GetComponent<SquidSwarmTarget>().Follow(Leader);
        }
    }

    void Spawn(GameObject prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            memberObject = (GameObject)Instantiate(prefab, position.position + new Vector3(Random.Range(-SpawnBounds, SpawnBounds), Random.Range(-SpawnBounds, SpawnBounds), 0), Quaternion.identity);
            member = memberObject.GetComponent<Member>();
            members.Add(member);
            member.spawn = this.gameObject.GetComponent<SquidSpawn>();

        }
    }

    public List<Member> getNeighbours(Member member, float radius)
    {
        List<Member> neighboursFound = new List<Member>();

        foreach (var otherMember in members)
        {
            if (otherMember == member)
                continue;
            if (Vector3.Distance(member.position, otherMember.position) <= radius)
            {
                neighboursFound.Add(otherMember);
            }
        }
        return neighboursFound;
    }

    public List<EnemyBoid> GetEnemyBoids(Member member, float radius)
    {

        if (!collecting)
        {
            StartCoroutine(collectEnemyBoids());
        }

        List<EnemyBoid> returnEnemies = new List<EnemyBoid>();
        foreach (var enemyBoid in enemies)
        {
            if (Vector3.Distance(member.position, enemyBoid.position) <= radius)
            {
                returnEnemies.Add(enemyBoid);
            }
        }
        return returnEnemies;
    }

    public Vector3 NewPatrolDestination()
    {
        float dist = Random.Range(0, bounds);
        float angle = Random.Range(0, 2 * Mathf.PI);

        return transform.position + new Vector3(dist * Mathf.Cos(angle), dist * Mathf.Sin(angle), 0);
    }

    IEnumerator collectEnemyBoids()
    {
        collecting = true;

        totalEnemies = level.enemies;

        if(Leader != null)
        {
            foreach (EnemyBoid enemy in totalEnemies)
            {
                if (Vector3.Distance(enemy.position, Leader.position) < enemyMaxDist)
                {
                    enemies.Add(enemy);
                }
            }
        }
        else
        {
            foreach (EnemyBoid enemy in totalEnemies)
            {
                if (Vector3.Distance(enemy.position, transform.position) < enemyMaxDist)
                {
                    enemies.Add(enemy);
                }
            }
        }

        yield return new WaitForSeconds(1);

        collecting = false;
    }

}
