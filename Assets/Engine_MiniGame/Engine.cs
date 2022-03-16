using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    private List<EnginePart> parts;

    //fixed parts
    public Part[] Parts;
    public List<Part> purchasedParts;
    public Vector3[] Pos;
    public int partsNum;
    public GameObject emptyPart;

    //unfixed parts
    //note: will get spawned in the side box
    public EnginePart[] unfixedParts;
    public Vector3[] unfixedPos;

    public int newNum;
    public int[] sizes;
    public int[] type;
    public int[] resistance;
    public int[] voltage;

    bool first = true;
    public Transform boxTransform;

    void Awake()
    {
        purchasedParts = new List<Part>();
    }

    public void Activate()
    {
        boxTransform = GameObject.Find("Box").transform;
        //Debug.Log("purchased parts" + purchasedParts.Count);
        
        if(CircuitGrid.circuitGrid != null)
        {
            if (first)
            {
                first = false;
                SpawnNewParts();
            }
            else
            {
                loadParts();
            }
        }

        foreach (Part part_0 in purchasedParts)
        {
            Debug.Log("size");
            Debug.Log(part_0.Size);
            EnginePart part = Instantiate(emptyPart, boxTransform.position, Quaternion.identity, boxTransform).GetComponent<EnginePart>();
            part.Size = part_0.Size;
            part.test = part_0;
            part.partType = part_0.partType;
            part.SpawnPart(part.test);
            
            //EnginePart part = Instantiate(emptyPart, boxTransform.position, Quaternion.identity, boxTransform).GetComponent<EnginePart>();
            //part.test = part_0;
            //part.SpawnPart(part.test);
        }

        purchasedParts = new List<Part>();
    }

    public void loadParts()
    {
        Debug.Log("loading parts");
        for(int i = 0; i < partsNum; i++)
        {
            if (Parts[i].Fixed)
            {
                EnginePart part = Instantiate(emptyPart, Pos[i], Quaternion.identity).GetComponent<EnginePart>();
                part.test = Parts[i];
                part.Size = Parts[i].Size;
                part.test = Parts[i];
                part.partType = Parts[i].partType;
                part.SpawnPart(part.test);
            }
            else
            {
                EnginePart part = Instantiate(emptyPart, boxTransform.position, Quaternion.identity, boxTransform).GetComponent<EnginePart>();
                part.test = Parts[i];
                part.Size = Parts[i].Size;
                part.test = Parts[i];
                part.partType = Parts[i].partType;
                part.SpawnPart(part.test);
            }
        }
    }

    public void saveParts()
    {
        parts = CircuitGrid.circuitGrid.engineParts;

        Parts = new Part[parts.Count];
        Pos = new Vector3[parts.Count];
        int i = 0;
        foreach(EnginePart part in parts)
        {
            Parts[i] = part.test;
            Pos[i] = part.transform.position;
            i++;
        }
        partsNum = i;
    }

    public void SpawnNewParts()
    {
        for (int i = 0; i < newNum; i++)
        {
            EnginePart part = Instantiate(emptyPart, new Vector3(0,0,0), Quaternion.identity).GetComponent<EnginePart>();
            part.Size = sizes[i];
            part.partType = type[i];
            part.resistance = resistance[i];
            part.energy = voltage[i];

            part.Generate();
        }
    }
}
