using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject[] AsteroidList;
    public Transform transform;
    private float dist;
    private int x;

    void Start()
    {

        x = Random.Range(0, AsteroidList.Length);

        Instantiate(AsteroidList[x], transform.position, Quaternion.identity, transform);
    }
}
