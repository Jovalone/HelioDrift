using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unlockBombs : MonoBehaviour
{

    public Inventory inventory;
    public DropBomb dropBomb;

    void Start()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
    }

    void Update()
    {
        if (inventory.inventory[10] >= 10)
        {
            dropBomb.enabled = true;
        }
        else
        {
            dropBomb.enabled = false;
        }
    }
}
