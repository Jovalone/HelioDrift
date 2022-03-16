using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBomb : MonoBehaviour
{
    public Transform transform;
    public GameObject bomb;

    public bool loading = false;
    public float loadTime;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && !loading)
        {
            Instantiate(bomb, transform.position, Quaternion.identity);
            StartCoroutine(Load());
        }
    }

    IEnumerator Load()
    {
        loading = true;
        yield return new WaitForSeconds(loadTime);
        loading = false;
    }
}
