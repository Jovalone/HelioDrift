using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCheck : MonoBehaviour
{
    public Image image;

    void Start()
    {
        image = this.gameObject.GetComponent<Image>();
    }

    void OnBecameInvisible()
    {
        image.enabled = false;
        Debug.Log("invisable");
    }

    void OnBecameVisible()
    {
        image.enabled = true;
        Debug.Log("Visable");
    }
}
