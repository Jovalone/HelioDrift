using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shardFade : MonoBehaviour
{
    public GameObject shard;
    public float fadeTime;
    public MeshRenderer meshRenderer;
    private Color colour;
    private Color alphaColor;

    void start()
    {
        colour = meshRenderer.material.color;
        alphaColor.a = 0;
    }

    void Update()
    {
        Debug.Log("1");
        if (shard.active)
        {
            Debug.Log("turning invisable");
            meshRenderer.material.color = Color.Lerp(colour, alphaColor, fadeTime * Time.deltaTime);
        }
    }
}
