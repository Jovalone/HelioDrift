using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningAttack : MonoBehaviour
{
    public GameObject nextLightning;

    public BoxCollider2D collider;

    public int damage;
    public GameObject origin;

    public AudioSource audio;

    void Start()
    {
        audio = GameObject.Find("LightningHit").GetComponent<AudioSource>();
    }

    public void NextAttack()
    {
        nextLightning.SetActive(true);
    }

    public void EndAttack()
    {
        this.gameObject.SetActive(false);
    }

    public void OpenHitWindow()
    {
        collider.enabled = true;
    }

    public void CloseHitWindow()
    {
        collider.enabled = false;
    }

    public void Origin(GameObject firepoint)
    {
        origin = firepoint;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Debug.Log("hit");
        Hittable Object = hitInfo.GetComponent<Hittable>();
        if (Object != null)
        {
            Object.TakeDamage(damage);
            Object.Attackers.Add(origin);
            audio.Play();
        }
    }
}
