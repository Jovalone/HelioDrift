using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawnerSmall : MonoBehaviour
{
    public GameObject A0, A1, A2, A3;
    public Transform transform, AsteroidHolder;
    private GameObject Player;

    private float dist;

    private int x;
    //public float chance;

    // Start is called before the first frame update
    void Start()
    {

        Player = GameObject.Find("Player");

        x = Random.Range(0, 3);

        switch (x)
        {
            case 0:
                Instantiate(A0, transform.position, Quaternion.identity, transform);
                break;

            case 1:
                Instantiate(A1, transform.position, Quaternion.identity, transform);
                break;

            case 2:
                Instantiate(A2, transform.position, Quaternion.identity, transform);
                break;

            case 3:
                Instantiate(A3, transform.position, Quaternion.identity, transform);
                break;
        }

    }

    void Update()
    {
        float x1 = transform.position.x;
        float x2 = Player.transform.position.x;
        float y1 = transform.position.y;
        float y2 = Player.transform.position.y;

        dist = Mathf.Sqrt(Mathf.Pow((x1 - x2), 2) + Mathf.Pow((y1 - y2), 2));

        if (dist > 50f)
        {
            Destroy(gameObject);
        }
    }
}
