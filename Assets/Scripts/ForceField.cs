using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour
{

    SpriteRenderer Rend;

    // Start is called before the first frame update
    void Start()
    {
        Rend = GetComponent<SpriteRenderer>();
        StartFading();

        Invoke("Die", 1f);
    }

    IEnumerator FadeOut()
	{
        for(float f = 1f; f >= -0.05f; f -= 0.1f)
		{
            Color c = Rend.material.color;
            c.a = f;
            Rend.material.color = c;
            yield return new WaitForSeconds(0.05f);
		}
	}

    void StartFading()
	{
        StartCoroutine("FadeOut"); 
	}

    public void Die()
    {
        Destroy(gameObject);
    }
}
