using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircuitGrid : MonoBehaviour
{
    public List<EnginePart> engineParts;
    public List<EnginePart> fixedParts;
    public List<EnginePart> loopedParts;

    public int gridWidth;
    public int gridHeight;
    public Text text;
    public Text[] textValues;
    public string[] valueNames;
    public float[] values;
    public float[] maxValues;
    public float[] offsetValues;
    public float[] finalValues;
    public int length;
    public PartsScroller scroller;

    public static CircuitGrid circuitGrid;
    //Part info
    public Text rTxt;
    public Text vTxt;
    
    void Awake()
    {
        circuitGrid = this;
        GameObject.Find("Engine").GetComponent<Engine>().Activate();
    }

    public void CheckPlacement(EnginePart Part)
    {
        //Debug.Log("Checking Placement");
        bool f = true;
        for(int i = 0; i < Part.Size + 1; i++)
        {
            //check if any blocks are outside the grid
            if (Part.sprites[i].gameObject.transform.position.x < transform.position.x)
            {
                f = false;
            }
            if (Part.sprites[i].gameObject.transform.position.x > transform.position.x + gridWidth)
            {
                f = false;
            }
            if (Part.sprites[i].gameObject.transform.position.y > transform.position.y)
            {
                f = false;
            }
            if (Part.sprites[i].gameObject.transform.position.y < transform.position.y - gridHeight)
            {
                f = false;
            }
        }

        foreach(EnginePart enginePart in engineParts)
        {
            if(enginePart != Part)
            {
                for (int i = 0; i < Part.Size + 1; i++)
                {
                    for (int j = 0; j < enginePart.Size + 1; j++)
                    {
                        if (Part.sprites[i].gameObject.transform.position == enginePart.sprites[j].gameObject.transform.position)
                        {
                            f = false;
                        }
                    }
                }
            }
        }

        if (!f)
        {
            //Debug.Log("not fixed");
            Part.changeLayerOrder(2);
            Part.test.Fixed = false;
        }
        else
        {
            //Debug.Log("fixed");
            fixedParts.Add(Part);
            //CHECK IF THERE IS A CIRCUIT MADE
            loopCount = 0;
            loopedParts = new List<EnginePart>();
            lookForLink(Part, Part);
            Part.test.Fixed = true;
        }
    }

    public int loopCount;
    public EnginePart savedPart;
    public void lookForLink(EnginePart part_0, EnginePart partOriginal)
    {
        //looks through the fixed parts script to find links
        bool link = false;
        bool cancel = false;
        savedPart = null;
        
        foreach (EnginePart part in fixedParts)
        {
            if(part != part_0)
            {
                if (Vector3.Distance(part.posPos.position, part_0.negPos.position) < 1.1f)
                {
                    if (link)
                    {
                        //Debug.Log("too many links");
                        text.text = "too many link";
                        cancel = true;
                    }
                    else
                    {
                        link = true;
                        savedPart = part;
                    }
                }
            }
        }

        if (!link || cancel)
        {
            //Debug.Log("no loop");
            text.text = "no loop";
            if (cancel)
            {
                //Debug.Log("canceled");
                text.text = "too many connections";
            }
            for (int i = 0; i < length; i++)
            {
                textValues[i].text = valueNames[i] + offsetValues[i] + "%";
            }
            finalValues = new float[length];
            Stats.statsInstance.enginePartValues = finalValues;

        }
        if (savedPart == partOriginal && !cancel)
        {
            //Debug.Log("Circuit completed");
            text.text = "Circuit Complete";
            loopedParts.Add(part_0);
            //calculate the values for each part
            calcValues();
        }
        else if(link && !cancel && loopCount < 10000)
        {
            loopedParts.Add(part_0);
            //Debug.Log("1 part");
            loopCount++;
            lookForLink(savedPart, partOriginal);
        }
    }

    public int totalResistance;
    public int totalEnergy;
    public void calcValues()
    {
        values = new float[length];
        finalValues = new float[length];
        //System.Array.Copy(offsetValues, values, 1);//no need to copy since should be done after valuues are calculated

        totalResistance = 0;
        totalEnergy = 0;
        foreach(EnginePart part in loopedParts)
        {
            totalEnergy += part.energy;
            totalResistance += part.resistance;
            //Debug.Log(part.energy);
        }
        float current = (float)totalEnergy / (float)totalResistance;

        for(int i = 0; i < length; i++)
        {
            textValues[i].text = valueNames[i] + "0";
        }

        foreach(EnginePart part in loopedParts)
        {
            part.value = (float)part.resistance * current * current;
            values[part.partType] += part.value;
        }

        //calculate new values
        for(int i = 0; i < length; i++)
        {
            finalValues[i] = (float)System.Math.Round(maxValues[i] * (1 - Mathf.Exp(-values[i] / 2)), 2) + offsetValues[i];
            textValues[i].text = valueNames[i] + finalValues[i].ToString() + "%";
        }
        Stats.statsInstance.enginePartValues = finalValues;
    }

    public void updatePartValues(EnginePart Part)
    {
        rTxt.text = "Resistance: " + Part.resistance.ToString();
        vTxt.text = "Power: " + Part.value.ToString();

    }

    public void boxCheck(Transform transform)
    {
        //if(transform.position.)
    }
}
