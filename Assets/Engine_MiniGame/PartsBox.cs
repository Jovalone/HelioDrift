using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsBox : MonoBehaviour
{
    public Transform transform;

    void OnTriggerEnter2D(Collider2D collider)
    {
        PartBlock block = collider.gameObject.GetComponent<PartBlock>();
        if (block != null)
        {
            //Debug.Log("in box");
            block.partPos.parent = transform;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        PartBlock block = collider.gameObject.GetComponent<PartBlock>();
        if (block != null)
        {
            //Debug.Log("out of box");
            block.partPos.parent = null;
        }
    }
}
