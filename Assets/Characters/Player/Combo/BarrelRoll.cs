using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelRoll : MonoBehaviour
{
    public GameObject player;
    private Player PlayerScript;
    private Boost boost;

    Animator animator;

    public float drain;
    public int combo;
    private int i = 0, j = 0;
    public int ComboNumber = 0;
    float lastTimeClicked = 0;
    public float maxComboDelay = 0.5f;
    private bool w, a, s, d;

    int[] LeftCombo = { 2, 2, 2, 0 };
    int[] RightCombo = { 4, 4, 4, 0 };

    public bool coolingDown = false;
    public float coolDownTime;

    void Start()
    {
        player = GameObject.Find("Player");
        PlayerScript = player.GetComponent<Player>();
        boost = player.GetComponent<Boost>();
        animator = player.GetComponent<Animator>();
    }

    void Update()
    {
        if (PlayerScript.Energy > 0 && PlayerScript.moveVelocity >= PlayerScript.minSpeed && !coolingDown)
        {
            if (Input.GetKey(KeyCode.Return))
            {
                if (Input.GetAxisRaw("Horizontal") == -1)
                {
                    //Activate
                    animator.SetTrigger("BRollLeft");
                    PlayerScript.LoseEnergy(drain);
                    PlayerScript.BarrelRoll(1);
                    StartCoroutine(Cooling());
                }else if (Input.GetAxisRaw("Horizontal") == 1)
                {
                    //Activate
                    animator.SetTrigger("BRollRight");
                    PlayerScript.LoseEnergy(drain);
                    PlayerScript.BarrelRoll(-1);
                    StartCoroutine(Cooling());
                }
            }
        }
    }

    IEnumerator Cooling()
    {
        coolingDown = true;
        yield return new WaitForSeconds(coolDownTime);
        coolingDown = false;
    }
}
