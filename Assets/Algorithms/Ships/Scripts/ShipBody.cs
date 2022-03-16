using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBody : MonoBehaviour
{
    public Transform transform;
    public Transform WingSpotR, WingSpotL;
    public Transform BoosterSpotR, BoosterSpotL;
    private GameObject Part, PartParent;
    SpriteRenderer sprite;
    public SpriteRenderer Cockpit;
    public GameObject Collider;

    Color colour;
    bool Ally;

    public GameObject W1, W2, W3, W4, W1R, W2R, W3R, W4R, B1, B2, B3;
    public Gradient AllyGradient, EnemyGradient, CockpitGradient;

    //send hittable to ShipAI
    public Hittable hittable;
    public ShipAI AI;
    public GameObject shipAI;

    // Start is called before the first frame update
    void Start()
    {
        shipAI = this.transform.parent.gameObject;
        shipAI = shipAI.transform.parent.gameObject;
        AI = shipAI.GetComponent<ShipAI>();
        AI.hittable = hittable;
        Ally = AI.Ally;

        if (Ally)
        {
            Collider.layer = 14;
        }
        else
        {
            Collider.layer = 15;
        }

        if (Ally)
        {
            colour = AllyGradient.Evaluate(Random.Range(0f, 1f));
        }
        else
        {
            colour = EnemyGradient.Evaluate(Random.Range(0f, 1f));
        }

        sprite = gameObject.GetComponent<SpriteRenderer>();
        sprite.color = colour;

        colour = CockpitGradient.Evaluate(Random.Range(0f, 1f));
        Cockpit.color = colour;

        int x = Random.Range(0, 3);

        if (Ally)
        {
            colour = AllyGradient.Evaluate(Random.Range(0f, 1f));
        }
        else
        {
            colour = EnemyGradient.Evaluate(Random.Range(0f, 1f));
        }

        switch (x)
        {
            case 0:
                
                PartParent = (GameObject)Instantiate(W1, WingSpotL.position, transform.rotation, WingSpotL);
                Part = PartParent.transform.GetChild(0).gameObject;
                sprite = Part.GetComponent<SpriteRenderer>();
                sprite.color = colour;

                PartParent = (GameObject)Instantiate(W1R, WingSpotR.position, transform.rotation, WingSpotR);
                Part = PartParent.transform.GetChild(0).gameObject;
                sprite = Part.GetComponent<SpriteRenderer>();
                sprite.color = colour;
                break;

            case 1:
                PartParent = (GameObject)Instantiate(W2, WingSpotL.position, transform.rotation, WingSpotL);
                Part = PartParent.transform.GetChild(0).gameObject;
                sprite = Part.GetComponent<SpriteRenderer>();
                sprite.color = colour;

                PartParent = (GameObject)Instantiate(W2R, WingSpotR.position, transform.rotation, WingSpotR);
                Part = PartParent.transform.GetChild(0).gameObject;
                sprite = Part.GetComponent<SpriteRenderer>();
                sprite.color = colour;
                break;

            case 2:
                PartParent = (GameObject)Instantiate(W3, WingSpotL.position, transform.rotation, WingSpotL);
                Part = PartParent.transform.GetChild(0).gameObject;
                sprite = Part.GetComponent<SpriteRenderer>();
                sprite.color = colour;

                PartParent = (GameObject)Instantiate(W3R, WingSpotR.position, transform.rotation, WingSpotR);
                Part = PartParent.transform.GetChild(0).gameObject;
                sprite = Part.GetComponent<SpriteRenderer>();
                sprite.color = colour;
                break;

            case 3:
                PartParent = (GameObject)Instantiate(W4, WingSpotL.position, transform.rotation, WingSpotR);
                Part = PartParent.transform.GetChild(0).gameObject;
                sprite = Part.GetComponent<SpriteRenderer>();
                sprite.color = colour;

                PartParent = (GameObject)Instantiate(W4R, WingSpotR.position, transform.rotation, WingSpotR);
                Part = PartParent.transform.GetChild(0).gameObject;
                sprite = Part.GetComponent<SpriteRenderer>();
                sprite.color = colour;
                break;
        }

        if (Ally)
        {
            colour = AllyGradient.Evaluate(Random.Range(0f, 1f));
        }
        else
        {
            colour = EnemyGradient.Evaluate(Random.Range(0f, 1f));
        }

        int y = Random.Range(0, 2);

        switch (y)
        {
            case 0:
                Part = (GameObject)Instantiate(B1, BoosterSpotL.position, transform.rotation, transform);
                sprite = Part.GetComponent<SpriteRenderer>();
                sprite.color = colour;

                Part = (GameObject)Instantiate(B1, BoosterSpotR.position, transform.rotation, transform);
                sprite = Part.GetComponent<SpriteRenderer>();
                sprite.color = colour;
                break;

            case 1:
                Part = (GameObject)Instantiate(B2, BoosterSpotL.position, transform.rotation, transform);
                sprite = Part.GetComponent<SpriteRenderer>();
                sprite.color = colour;

                Part = (GameObject)Instantiate(B2, BoosterSpotR.position, transform.rotation, transform);
                sprite = Part.GetComponent<SpriteRenderer>();
                sprite.color = colour;
                break;

            case 2:
                Part = (GameObject)Instantiate(B3, BoosterSpotL.position, transform.rotation, transform);
                sprite = Part.GetComponent<SpriteRenderer>();
                sprite.color = colour;

                Part = (GameObject)Instantiate(B3, BoosterSpotR.position, transform.rotation, transform);
                sprite = Part.GetComponent<SpriteRenderer>();
                sprite.color = colour;
                break;
        }
    }
}
