using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Transform transform;

    public int[] inventory;
    public int[] lastSaveInventory;
    public float coins;

    public GameObject Intro;

    public static Inventory instance;

    void Awake()
    {
        NoDestroyMethod();
    }

    public void UnParent()
    {
        transform.parent = null;
    }

    void NoDestroyMethod()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            if(Intro != null)
            {
                Intro.SetActive(true);
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SaveInventory()
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            lastSaveInventory[i] = inventory[i];
        }
    }

    public void SetInventory()
    {
        for (int j = 0; j < inventory.Length; j++)
        {
            inventory[j] = lastSaveInventory[j];
        }
    }
}
