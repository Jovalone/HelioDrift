using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Image PanelImage;
    public Sprite[] spriteList;
    public int i = 0;
    public GameObject noImagetext;

    public void ChangeImage(int j)
    {
        if((i+j) < spriteList.Length && (i+j) >= 0)
        {
            i += j;
            PanelImage.sprite = spriteList[i];
        }
        else
        {
            StartCoroutine(noImage());
        }
    }

    IEnumerator noImage()
    {
        noImagetext.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        noImagetext.SetActive(false);
    }
}
