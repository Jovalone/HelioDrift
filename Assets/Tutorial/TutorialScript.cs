using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;
using System.Collections;

public class TutorialScript : MonoBehaviour
{

    public Image image;
    public Sprite sprite;
    public int[] Ammo;
    public GameObject OldManMission;
    private bool first = true;
    public Sentence oldManSentence;
    public Dialogue dialogue;

    public int tutorialStage;
    // Canvas pointers
    public GameObject[] events;

    public bool f = true;

    //Stage 1
    private float one = 1;
    public FirePoint firePoint;

    //Stage 2
    public GameObject bulletPanel;
    public GameObject bulletPanelArrow;
    private bool two = false;

    //Stage 3
    public Boost boost;
    public float neededBoostTime = 5;

    //Stage 4
    public int neededRolls;
    public Animator animator;

    //Stage 5
    public int neededFlips;

    //Stage 6
    public float DriftTime;

    void Start()
    {
        StartCoroutine(activateEvent());
    }

    void SetUp()
    {
        if (Stats.statsInstance.tutorial)
        {
            //replace oldman sprite with the gardener
            image.sprite = sprite;
            OldManMission.SetActive(false);
            firePoint.Ammo = Ammo;
            NextEvent();
        }
        else
        {
            //turn off script
            this.enabled = false;
            this.gameObject.SetActive(false);
            oldManSentence.Activate();
        }
    }

    void Update()
    {
        if (first)
        {
            SetUp();
            first = false;
        }
        else
        {
            switch (tutorialStage)
            {
                case 0:
                    //Learn to shoot
                    if (dialogue.Completed && f == false)
                    {
                        events[tutorialStage].SetActive(true);
                    }
                    else
                    {
                        events[tutorialStage].SetActive(false);
                    }

                    if (!firePoint.loaded)
                    {
                        one -= Time.deltaTime;
                        if (one < 0)
                        {
                            //move onto the next stage
                            events[tutorialStage].SetActive(false);
                            NextEvent();
                        }
                    }
                    break;

                case 1:
                    //see other bullets
                    if (dialogue.Completed && f == false)
                    {
                        events[tutorialStage].SetActive(true);
                    }
                    else
                    {
                        events[tutorialStage].SetActive(false);
                    }

                    if (bulletPanel.activeSelf)
                    {
                        //move onto the next stage
                        two = true;
                    }
                    else if (two)
                    {
                        events[tutorialStage].SetActive(false);
                        f = true;
                        NextEvent();
                    }
                    break;

                case 2:
                    //Learn to boost
                    if (dialogue.Completed && !bulletPanel.activeSelf)
                    {
                        events[tutorialStage].SetActive(true);
                    }
                    else
                    {
                        events[tutorialStage].SetActive(false);
                    }

                    if (boost.Activate)
                    {
                        neededBoostTime -= Time.deltaTime;
                        if (neededBoostTime < 0)
                        {
                            //move onto the next stage
                            events[tutorialStage].SetActive(false);
                            f = true;
                            NextEvent();
                        }
                    }
                    break;

                case 3:
                    //learn to double boost
                    if (dialogue.Completed && f == false)
                    {
                        events[tutorialStage].SetActive(true);
                    }
                    else
                    {
                        events[tutorialStage].SetActive(false);
                    }

                    if (boost.Activate && Input.GetKeyDown("w") && !boost.BoostAttempt)
                    {
                        if (Player.playerInstance.boostSlider.value < 0.114 && Player.playerInstance.boostSlider.value > 0.041)
                        {
                            //move onto the next stage
                            events[tutorialStage].SetActive(false);
                            f = true;
                            NextEvent();
                        }
                    }
                    break;

                case 4:
                    //Learn to barrel roll
                    if (dialogue.Completed && f == false)
                    {
                        events[tutorialStage].SetActive(true);
                    }
                    else
                    {
                        events[tutorialStage].SetActive(false);
                    }

                    if (animator.GetCurrentAnimatorStateInfo(0).IsTag("1"))
                    {
                        neededRolls++;
                    }
                    else if (animator.GetCurrentAnimatorStateInfo(0).IsTag("1"))
                    {
                        neededRolls++;
                    }
                    if (neededRolls > 2)
                    {
                        //move onto the next stage
                        events[tutorialStage].SetActive(false);
                        f = true;
                        NextEvent();
                    }
                    break;

                case 5:
                    //Learn to flip
                    if (dialogue.Completed && f == false)
                    {
                        events[tutorialStage].SetActive(true);
                    }
                    else
                    {
                        events[tutorialStage].SetActive(false);
                    }

                    if (animator.GetCurrentAnimatorStateInfo(0).IsTag("2"))
                    {
                        neededFlips++;
                        if (neededFlips > 2)
                        {
                            //move onto the next stage
                            events[tutorialStage].SetActive(false);
                            f = true;
                            NextEvent();
                        }
                    }
                    break;

                case 6:
                    //Learn to drift
                    if (dialogue.Completed && f == false)
                    {
                        events[tutorialStage].SetActive(true);
                    }
                    else
                    {
                        events[tutorialStage].SetActive(false);
                    }

                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        DriftTime -= Time.deltaTime;
                        if (DriftTime < 0)
                        {
                            //finish Tutorial
                            events[tutorialStage].SetActive(false);
                            Debug.Log("end tutorial");
                            Stats.statsInstance.tutorial = false;
                            QuestLog.SetQuestState("Tutorial", "success");
                            Stats.statsInstance.Ammo = Stats.statsInstance.lastSaveAmmo;
                            NextEvent();
                        }
                    }
                    break;
            }
        }
    }

    void NextEvent()
    {
        StartCoroutine(activateEvent());
        tutorialStage++;
        if(GameObject.Find("Tutorial").transform.GetChild(tutorialStage) != null)
        {
            GameObject.Find("Tutorial").transform.GetChild(tutorialStage).GetComponent<Sentence>().Activate();
        }
    }

    IEnumerator activateEvent()
    {
        f = true;
        yield return new WaitForSeconds(2);

        f = false;
    }
}
