using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartRotate : MonoBehaviour
{
    public GameObject part;

    public static PartRotate partRotateInstance;

    void Awake()
    {
        partRotateInstance = this;
    }

    public void Rotate()
    {
        if(part != null)
        {
            part.transform.Rotate(0f, 0.0f, 90f, Space.Self);
            part.gameObject.GetComponent<EnginePart>().test.rotations++;
            if (part.gameObject.GetComponent<EnginePart>().test.rotations == 4)
            {
                part.gameObject.GetComponent<EnginePart>().test.rotations = 0;
            }

            CircuitGrid.circuitGrid.fixedParts.Remove(part.GetComponent<EnginePart>());
            CircuitGrid.circuitGrid.CheckPlacement(part.GetComponent<EnginePart>());
        }
    }
}
