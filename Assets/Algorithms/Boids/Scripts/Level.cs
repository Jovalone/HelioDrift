using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Transform transform;
    public Transform position, Leader, target;
    public Transform memberPrefab;
    public Transform enemyPrefab;
    public int numberOfMembers, numberOfEnemies;
    public List<Member> members;
    public List<EnemyBoid> enemies;
    public float bounds;
    public float spawnRadius;
    public Color colour;

    //Shark stuff
    public List<Hittable> liveCharacters, temp;
    public List<Shark> sharks;
    public List<Shark> sharkLeaders;
    public int frameDelay;
    bool searching = false;

    //Manta Stuff
    public List<Manta> Mantas;

    public static Level levelInstance;

    void Update()
    {
        temp = new List<Hittable>();
        //clear all dead characters
        foreach(Hittable organism in liveCharacters)
        {
            if(organism == null)
            {
                temp.Add(organism);
            }
        }

        foreach(Hittable missing in temp)
        {
            liveCharacters.Remove(missing);
        }
    }

    void Awake()
    {
        levelInstance = this;
        members = new List<Member>();
        enemies = new List<EnemyBoid>();
        liveCharacters = new List<Hittable>();
        sharks = new List<Shark>();
        Mantas = new List<Manta>();
    }

    void SpawnMember(Transform prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(prefab, position.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius), 0), Quaternion.identity);
        }
    }

    void Spawn(Transform prefab, int count)
	{
        for(int i = 0; i < count; i++)
		{
            Instantiate(prefab, position.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius), 0), Quaternion.identity);
		}
	}

    public List<Member> getNeighbours(Member member, float radius)
	{
        List<Member> neighboursFound = new List<Member>();

        foreach (var otherMember in members)
		{
            if (otherMember == member)
                continue;
            if(Vector3.Distance(member.position, otherMember.position) <= radius)
			{
                neighboursFound.Add(otherMember);
			}
		}
        return neighboursFound;
	}
    
    public List<EnemyBoid> GetEnemyBoids(Member member, float radius)
	{
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
}
