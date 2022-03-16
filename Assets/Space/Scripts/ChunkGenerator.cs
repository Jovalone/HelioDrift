using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    public GameObject SpaceBlock;
    public Transform transform;

    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Vector3 position = transform.position + new Vector3((4 * j - 20), (4 * i - 20), 0f);

                Instantiate(SpaceBlock, position, Quaternion.identity, transform);
            }
        }
    }
}
