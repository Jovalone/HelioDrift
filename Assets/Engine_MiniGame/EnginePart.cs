using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnginePart : MonoBehaviour
{
    public bool SpawnOnStart;
    public GameObject[] block;
    public SpriteRenderer[] sprites;
    public Part_Generator generator;
    public Part test;
    public int Size;
    public int edgeSize;
    private PartBlock Pblock;
    public Color[] colour;
    public int partType;
    public int energy;
    public int resistance;
    public float value;
    public float maxValue;
    public Transform posPos, negPos;
    public int l = 0;

    void Start()
    {
        if (SpawnOnStart)
        {
            test = generator.generatePart(Size, edgeSize, partType);
            SpawnPart(test);
        }
    }

    public void Generate()
    {
        test = generator.generatePart(Size, edgeSize, partType);
        SpawnPart(test);
    }

    public void SpawnPart(Part part)
    {
        if (!SpawnOnStart)
        {
            CircuitGrid.circuitGrid.engineParts.Add(this);
        }
        
        sprites = new SpriteRenderer[Size + 1];
        l = 0;

        for (int i = 0; i < part.edgeSize; i++)
        {
            for (int j = 0; j < part.edgeSize; j++)
            {
                if(part.Layout[i,j] == 1 || part.Layout[i, j] == 2)
                {
                    //Spawn block
                    Pblock = Instantiate(block[0], transform.position + new Vector3(i - edgeSize/2 , j - edgeSize / 2, 0), Quaternion.identity, transform).GetComponent<PartBlock>();
                    Pblock.partPos = this.transform;
                    Pblock.partTran = this.transform;
                    Pblock.sprite.color = colour[part.partType];
                    sprites[l] = Pblock.sprite;
                    l++;
                }
                if (part.Layout[i, j] == 3)
                {
                    //Spawn block
                    Pblock = Instantiate(block[1], transform.position + new Vector3(i - edgeSize / 2, j - edgeSize / 2, 0), Quaternion.identity, transform).GetComponent<PartBlock>();
                    Pblock.partPos = this.transform;
                    Pblock.partTran = this.transform;
                    Pblock.sprite.color = colour[part.partType];
                    sprites[l] = Pblock.sprite;
                    posPos = Pblock.transform;
                    l++;
                }
                if (part.Layout[i, j] == 4)
                {
                    //Spawn block
                    Pblock = Instantiate(block[2], transform.position + new Vector3(i - edgeSize / 2, j - edgeSize / 2, 0), Quaternion.identity, transform).GetComponent<PartBlock>();
                    Pblock.partPos = this.transform;
                    Pblock.partTran = this.transform;
                    Pblock.sprite.color = colour[part.partType];
                    sprites[l] = Pblock.sprite;
                    negPos = Pblock.transform;
                    l++;
                }
            }
        }
        for(int j = 0; j < test.rotations; j++)
        {
            transform.Rotate(0f, 0.0f, 90f, Space.Self);
        }
    }

    public void changeLayerOrder(int layer)
    {
        for(int k = 0; k < Size + 1; k++)
        {
            sprites[k].sortingOrder = layer;
        }
    }
}
