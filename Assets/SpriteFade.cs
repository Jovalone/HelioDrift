using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFade : MonoBehaviour
{
    public float a = 1;
    public SpriteRenderer sprite;
    public Color colour;
    public float lifeTime;

    public bool active = true;

    void Start()
    {
        colour = sprite.color;
    }

    void Update()
    {
        if (active)
        {
            if(a > 0)
            {
                a -= Time.deltaTime / lifeTime;

                colour.a = a;
                sprite.color = colour;
            }
            else
            {
                active = false;
            }
        }
    }
}
