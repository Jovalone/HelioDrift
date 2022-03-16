using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipGenerator : MonoBehaviour
{
    public GameObject B1, B2, B3, B4, Ship;
    public Transform transform;

    // Start is called before the first frame update
    void Start()
    {
        //Choose a random shipbody
        int x = Random.Range(0, 3);

        switch (x)
        {
            case 0:
                Ship = (GameObject)Instantiate(B1, transform.position, transform.rotation, transform);
                break;
            case 1:
                Ship = (GameObject)Instantiate(B2, transform.position, transform.rotation, transform);
                break;
            case 2:
                Ship = (GameObject)Instantiate(B3, transform.position, transform.rotation, transform);
                break;
            case 3:
                Ship = (GameObject)Instantiate(B4, transform.position, transform.rotation, transform);
                break;
        }
    }
}
