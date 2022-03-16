using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlay : MonoBehaviour
{
    public GameObject Tutorial;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.name == "Player")
        {
            Tutorial.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        Debug.Log("test");
        if (collider.name == "Player")
        {
            Tutorial.SetActive(false);
        }
    }
}
