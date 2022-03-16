using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boost : MonoBehaviour
{
    public GameObject player;
    private Player PlayerScript;
    private Flip flip;
    Animator animator;
    public int[] Combo = {1, 1, 0};
    public float drain, re_drain;
    public float speed;
    public float ogSpeed;
    public int combo;
    private int i = 0;
    public int ComboNumber = 0;
    float lastTimeClicked = 0;
    public float maxComboDelay = 0.5f;
    public bool Activate = false;
    private bool w, a, s, d;
    public float rotationIncrease;
    public float ogRotation;
    bool cooling = false;
    public float coolingTime;

    public AudioSource audio;

    public bool BoostAttempt = false;

    //Booste trail Renderers
    public TrailRenderer Normal, Booster1, Booster2;

    //Boost Particle Effect
    public ParticleSystem particle;
    public BoostParticle bp;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        PlayerScript = Player.playerInstance;
        flip = player.GetComponent<Flip>();
        animator = player.GetComponent<Animator>();

        Normal.enabled = false;
        Booster1.enabled = false;
        Booster2.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("s"))// && input.GetKey(Keycode.Return))
        {
            flip.HitBrakes();
            Normal.enabled = true;
            Booster1.enabled = false;
            Booster2.enabled = false;
            //PlayerScript.rotationSpeed = ogRotation;
            Activate = false;

            PlayerScript.DashDist = 0;
        }

        if (PlayerScript.Energy > 0 && !cooling)
        {
            if (Activate && Input.GetKeyDown(input[arrow]) && !BoostAttempt)//combo boost
            {
                if (PlayerScript.boostSlider.value < (arrowPos + 0.02f) && PlayerScript.boostSlider.value > (arrowPos - 0.02f) && PlayerScript.Energy > (re_drain/2))
                {
                    //perfect timing
                    boost(false);
                    PlayerScript.LoseEnergy(re_drain/2);
                }
                else if (PlayerScript.boostSlider.value < (arrowPos + 0.1025f) && PlayerScript.boostSlider.value > (arrowPos - 0.1025f) && PlayerScript.Energy > re_drain)
                {
                    boost(false);
                    PlayerScript.LoseEnergy(re_drain);
                }
                else
                {
                    //BoostAttempt = true;
                }
            }

            if (Activate)
            {

                if (Normal.enabled)
                {
                    Normal.enabled = false;
                    Booster1.enabled = true;
                    Booster2.enabled = true;
                }

                animator.SetBool("Boost", true);
            }
            else
            {
                if(!Normal.enabled)
                {
                    Normal.enabled = true;
                }
                animator.SetBool("Boost", false);
                //PlayerScript.speed = PlayerScript.Speed;
                w = Input.GetKeyDown("w");
                a = Input.GetKeyDown("a");
                s = Input.GetKeyDown("s");
                d = Input.GetKeyDown("d");

                combo = 0;

                if (w && !a && !s && !d)
                {
                    combo = 1;
                    Debug.Log(Combo);
                }
                else if (!w && (a || s || d))
                {
                    combo = 2;
                }

                if (Time.time - lastTimeClicked > maxComboDelay)
                {
                    i = 0;
                }

                if (Combo[i] == combo && PlayerScript.Energy > drain)
                {
                    i++;
                    lastTimeClicked = Time.time;
                    if (Combo[i] == 0)
                    {
                        PlayerScript.LoseEnergy(drain);
                        boost(true);
                    }
                    combo = 0;
                }
            }
        }

        if(PlayerScript.Energy == 0)
        {
            Normal.enabled = true;
            Booster1.enabled = false;
            Booster2.enabled = false;
            //PlayerScript.rotationSpeed = ogRotation;
            Activate = false;
        }
    }

    public void boost(bool first)
    {
        bp.Activate();

        BoostSlider();
        animator.SetTrigger("perfectboost");
        if (first)
        {
            //ogRotation = PlayerScript.rotationSpeed;
            //ogSpeed = PlayerScript.speed;
            //PlayerScript.rotationSpeed = PlayerScript.rotationSpeed * rotationIncrease;
        }
        Activate = true;
        particle.Play();
        PlayerScript.Dash();
        BoostAttempt = false;
        audio.Play();
        audio.pitch += 0.05f;
        if(audio.pitch > 1.5f)
        {
            audio.pitch = 1.2f;
        }
        i = 0;
    }

    public void endBoost()
    {
        Normal.enabled = true;
        Booster1.enabled = false;
        Booster2.enabled = false;
        //PlayerScript.rotationSpeed = ogRotation;
        Activate = false;
        PlayerScript.DashDist = 0;

        StartCoroutine(overHeating());
    }

    public IEnumerator overHeating()
    {
        cooling = true;
        audio.pitch = 0.8f;

        yield return new WaitForSeconds(coolingTime);

        cooling = false;
    }

    //Slider Parameters
    public RectTransform rt;
    public float arrowPos;
    public Image arrowImage;
    public Sprite[] sprites;
    public int arrow;
    public string[] input;

    public void BoostSlider()
    {
        arrowPos = Random.Range(-50, 0);
        rt.anchoredPosition = new Vector3(arrowPos, 0.4f, 0);
        arrow = Random.Range(0, 3);
        arrowImage.sprite = sprites[arrow];
        arrowPos = (arrowPos) / (50 / 0.3f) + 0.485f;
    }
}
