using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasPart : MonoBehaviour
{

    public Part_Generator generator;
    public Part part;
    public GameObject[] block;
    public int Size;
    public int edgeSize;
    public int partType;
    public int cost;
    public int resistance;

    public Color[] colours;
    private Image image;

    //Generation Parameters
    public int level;

    void Start()
    {
        partType = Random.Range(0, 5);

        level = Random.Range(1, 10);
        Size = Random.Range(1, 8);
        resistance = (int)(100 - 98 * (1 - Mathf.Exp(-Mathf.Sqrt(Size) * level * 0.15f)));
        cost = (int)(level * Mathf.Exp(0.5f * level) * 2);

        part = generator.generatePart(Size, edgeSize, partType);
        Spawn();
    }

    void Spawn()
    {
        for (int i = 0; i < part.edgeSize; i++)
        {
            for (int j = 0; j < part.edgeSize; j++)
            {
                if (part.Layout[i, j] == 1 || part.Layout[i, j] == 2)
                {
                    //Spawn block
                    image = GameObject.Instantiate(block[0], transform.position + new Vector3((i - edgeSize / 2) * 50, (j - edgeSize / 2) * 50, 0), Quaternion.identity, transform).GetComponent<Image>();
                    image.color = colours[partType];
                }
                if (part.Layout[i, j] == 3)
                {
                    //Spawn block
                    image = GameObject.Instantiate(block[1], transform.position + new Vector3((i - edgeSize / 2) * 50, (j - edgeSize / 2) * 50, 0), Quaternion.identity, transform).GetComponent<Image>();
                    image.color = colours[partType];
                }
                if (part.Layout[i, j] == 4)
                {
                    //Spawn block
                    image = GameObject.Instantiate(block[2], transform.position + new Vector3((i - edgeSize / 2) * 50, (j - edgeSize / 2) * 50, 0), Quaternion.identity, transform).GetComponent<Image>();
                    image.color = colours[partType];
                }
            }
        }
    }
}
