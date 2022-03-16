using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimingBar : MonoBehaviour
{

    public Slider timingSlider;
    public int direction = 1;
    public float speed;
    public float delayTime;
    private bool resetting = false;
    public float hitDelay;
    private bool hit = false;
    private bool hitAvailable = true;

    public Slider tempSlider;
    public float tempDecay;
    public float nullScore;
    public float niceScore;
    public float perfectScore;
    public GameObject bad;
    public GameObject good;
    public GameObject perfect;

    public Animator animator;

    void Start()
    {
        timingSlider.value = 0;
        tempSlider.value = 0;
    }

    void Update()
    {
        if (!hit)
        {
            timingSlider.value += direction * speed * Time.deltaTime;
        }
        if(!resetting && timingSlider.value == 1)
        {
            StartCoroutine(delay(delayTime));
        }

        if(hitAvailable && Input.GetKey(KeyCode.Return) && !resetting)
        {
            hit = true;
            animator.SetTrigger("Hammer");
            StartCoroutine(delay(hitDelay));
            hitAvailable = false;
        }

        tempSlider.value -= tempDecay * Time.deltaTime;
    }

    IEnumerator delay(float delay)
    {
        resetting = true;
        if (hit)
        {
            //Debug.Log(timingSlider.value);
            yield return new WaitForSeconds(hitDelay);
        }
        addTemp();
        yield return new WaitForSeconds(delayTime);
        hit = false;
        resetting = false;
        timingSlider.value = 0;
        hitAvailable = true;

        bad.SetActive(false);
        good.SetActive(false);
        perfect.SetActive(false);
    }

    void addTemp()
    {
        if(timingSlider.value < 0.64f || timingSlider.value > 0.92f)
        {
            tempSlider.value += nullScore;
            bad.SetActive(true);
        }else if(timingSlider.value < 0.84f && timingSlider.value > 0.8f)
        {
            tempSlider.value += perfectScore;
            if(tempSlider.value + perfectScore > 1)
            {
                tempSlider.value = 1;
            }
            perfect.SetActive(true);
        }
        else
        {
            tempSlider.value += niceScore;
            if (tempSlider.value + niceScore > 1)
            {
                tempSlider.value = 1;
            }
            good.SetActive(true);
        }
    }
}
