using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBlock : MonoBehaviour
{
    public List<WallBlock> blocks;
    public SpriteRenderer sprite;

    public Color alpha;
    public Color original;
    public bool active = false;
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        active = true;
        foreach(WallBlock block in blocks)
        {
            block.sprite.color = alpha;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        active = false;

        bool temp = false;

        foreach(WallBlock block in blocks)
        {
            if (block.active)
            {
                temp = true;
            }
        }

        if (!temp)
        {
            foreach (WallBlock block in blocks)
            {
                block.sprite.color = original;
            }
        }
    }
}
