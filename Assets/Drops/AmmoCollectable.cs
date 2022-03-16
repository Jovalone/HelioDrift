using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCollectable : MonoBehaviour
{
    
    public Animator animator;
    public GameObject Player, FirePoint;
    public Text txt;
    public float checkDistance = 2;
    public int ammo;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        animator.SetTrigger("Green");
    }

    void Update()
	{
		if (CheckCloseToTag(checkDistance))
		{
            txt.enabled = true;
            if (Input.GetKeyDown(KeyCode.R) == true)
			{
                FirePoint.GetComponent<FirePoint>().GrabAmmo(ammo);
                txt.enabled = false;
                Destroy(gameObject);
            }

        }
		else
		{
            txt.enabled = false;
		}

    }

    bool CheckCloseToTag(float minimumDistance)
    {
            if (Vector3.Distance(transform.position, Player.transform.position) <= minimumDistance)
                return true;
        
        return false;
    }
}
