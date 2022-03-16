using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBlock : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Sprite[] sprites;

    void Start()
    {
        int i = Random.Range(0, sprites.Length - 1);

        sprite.sprite = sprites[i];
    }
}
