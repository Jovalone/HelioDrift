using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthCollectable : MonoBehaviour
{

    public Animator animator;
    public GameObject Player;
    public Text txt;
    public int health;
    public float checkDistance = 2;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        animator.SetTrigger("Orange");
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckCloseToTag(checkDistance))
        {
            txt.enabled = true;
            if (Input.GetKeyDown(KeyCode.R) == true)
            {
                Player.GetComponent<Player>().Heal(health);
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
