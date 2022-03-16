using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarStars : MonoBehaviour
{
    public Vector3 startPos;
    public GameObject Stars;
    public Transform Player;
    public Transform transform;
    public Transform StarHolder;

    public bool follow;
    public float followSpeed;
    public bool setup = false;

    //Respawning Parameters
    public float X, Y;
    public bool[] Spots;
    public List<GameObject> Chunks;
    public List<GameObject> notNeeded;


    public void SetUp()
    {
        setup = true;
        startPos = Player.position;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                X = Mathf.Round(Player.position.x / 40f);
                Y = Mathf.Round(Player.position.y / 40f);

                Vector3 position = new Vector3((40 * j) + X * 40 - 40, (40 * i) + Y * 40 - 40, 0f);

                GameObject chunk = (GameObject)Instantiate(Stars, position, Quaternion.identity, StarHolder);
                Chunks.Add(chunk);
            }
        }
    }

    void Update()
    {
        if (setup)
        {
            if (follow)
            {
                transform.position = startPos - new Vector3((startPos.x - Player.position.x) / followSpeed, (startPos.y - Player.position.y) / followSpeed, 0);
            }

            X = Mathf.Round((Player.position.x - transform.position.x) / 40f);
            Y = Mathf.Round((Player.position.y - transform.position.y) / 40f);

            Spots = new bool[9];
            notNeeded = new List<GameObject>();
            for(int k = 0; k < 9; k++)
            {
                Spots[k] = false;
            }

            foreach (GameObject Chunk in Chunks)
            {
                bool needed = false;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (Spots[3 * i + j] == false)
                        {
                            if (Vector3.Distance(Chunk.transform.position, transform.position + new Vector3((40 * j) + X * 40 - 40, (40 * i) + Y * 40 - 40, 0f)) < 10f)
                            {
                                needed = true;
                                Spots[3 * i + j] = true;
                            }
                        }
                    }
                }

                if (!needed)
                {
                    notNeeded.Add(Chunk);
                }
            }

            foreach (GameObject Chunk in notNeeded)
            {
                Chunks.Remove(Chunk);
                Destroy(Chunk);
            }
            
            for (int i = 0; i < 9; i++)
            {
                if (!Spots[i])
                {
                    GameObject chunk = (GameObject)Instantiate(Stars, transform.position + new Vector3(X * 40 + (float)((i % 3) * 40) - 40, Y * 40 + (float)((int)(i / 3)) * 40 - 40, 0), Quaternion.identity, StarHolder);
                    Chunks.Add(chunk);
                }
            }
        }
    }
}
