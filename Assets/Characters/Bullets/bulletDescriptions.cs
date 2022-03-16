using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletDescriptions : MonoBehaviour
{
    public GameObject[] descriptions;
    
    public void newDescription(int i)
    {
        for(int j = 0; j < descriptions.Length; j++)
        {
            if(i == j)
            {
                descriptions[j].SetActive(true);
            }
            else
            {
                descriptions[j].SetActive(false);
            }
        }
    }

    public void hideAll()
    {
        for (int j = 0; j < descriptions.Length; j++)
        {
            descriptions[j].SetActive(false);
        }
    }
}
