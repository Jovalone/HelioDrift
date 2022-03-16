using UnityEngine;
using UnityEngine.UI;

public class EnergyCollectible : MonoBehaviour
{
    public Animator animator;
    public GameObject player;
    public Text txt;
    public float checkDistance = 2;
    public int energy;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        animator.SetTrigger("Blue");
    }

    void Update()
    {
        if (CheckCloseToTag(checkDistance))
        {
            txt.enabled = true;
            if (Input.GetKeyDown(KeyCode.R) == true)
            {
                player.GetComponent<Player>().LoseEnergy(-energy);
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
        if (Vector3.Distance(transform.position, player.transform.position) <= minimumDistance)
            return true;

        return false;
    }
}
