using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class FirePoint : MonoBehaviour
{

    public Transform firePoint;
    public GameObject[] bulletPrefab;
    public int BulletType;
    public GameObject Object;
    private GameObject bullet;
    public Text txt;
    public int[] Ammo;
    public float[] shootStall;
    public float[] bulletDelay;
    public bool loaded;
    Inventory inventory;
    public float engineFactor;
    public ParticleSystem firingParticle;

    public Transform Target;

    public AudioSource Audio;

    public GameObject LockOn, LockOnObject;
    public Player player;
    public Stats stats;
    public GameObject NoAmmoWarning;

    void Start()
	{
        txt.text = "Ammo : " + Ammo[BulletType];
    }


    void Update()
    {
        if (Ammo[BulletType] != 0)
        {
            if(Input.GetMouseButtonDown(0) && loaded)
            {
                Shoot();
                Ammo[BulletType] -= 1;
                txt.text = "Ammo : " + Ammo[BulletType];
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Space) && loaded)
            {
                StartCoroutine(NoAmmo());
            }
        }
    }

    public void updateAmmo()
    {
        txt.text = "Ammo : " + Ammo[BulletType];
    }

    void Shoot()
	{
        //testing mouse shooting
        Vector3 shootDirection;
        shootDirection = Input.mousePosition;
        shootDirection.z = 0.0f;
        shootDirection = Camera.main.ScreenToWorldPoint(shootDirection);
        shootDirection = shootDirection - transform.position;
        Debug.Log(Mathf.Rad2Deg * Mathf.Atan2(shootDirection.y, shootDirection.x));


        player.moveVelocity = player.moveVelocity * shootStall[BulletType];
        bullet = (GameObject)Instantiate(bulletPrefab[BulletType], firePoint.position, Quaternion.Euler(0, 0, -Mathf.Rad2Deg * Mathf.Atan2(shootDirection.x, shootDirection.y)));
        if(BulletType == 5)
        {
            bullet.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<LightningAttack>().Origin(Object);
            bullet.transform.GetChild(0).transform.GetChild(1).gameObject.GetComponent<LightningAttack>().Origin(Object);
            Audio.Play();
            ScreenShakeController.instance.StartShake(0.03f, 0.3f);
        }
        else
        {
            bullet.GetComponent<Bullet>().Origin(Object);
            bullet.GetComponent<Bullet>().SetTarget(Target);
            //bullet.GetComponent<Bullet>().speed += player.moveVelocity / 2;
            Audio.Play();
            ScreenShakeController.instance.StartShake(0.03f, 0.3f);
            firingParticle.Play();
        }
        StartCoroutine(Stall());

        //RecordKeeper.Record.statistics["Bullet" + BulletType + "Shots"]++;
        //Debug.Log(RecordKeeper.Record.statistics["Bullet" + BulletType + "Shots"]);
	}

    public void GrabAmmo(int value)
    {
        Ammo[BulletType] += value;
        txt.text = "Ammo : " + Ammo[BulletType];
    }

    public void SetAmmo(int[] value)
    {
        Ammo = value;
        txt.text = "Ammo : " + Ammo[BulletType];
    }

    public void SpawnLockOn()
    {
        if(LockOnObject != null)
        {
            Destroy(LockOnObject);
        }
        LockOnObject = (GameObject)Instantiate(LockOn, Target.position, Quaternion.identity);
        LockOnObject.GetComponent<LockOn>().SetTarget(Target);
    }

    IEnumerator Stall()
    {
        loaded = false;
        yield return new WaitForSeconds(bulletDelay[BulletType] / engineFactor);
        loaded = true;
    }

    IEnumerator NoAmmo()
    {
        NoAmmoWarning.SetActive(true);
        yield return new WaitForSeconds(2);
        NoAmmoWarning.SetActive(false);
    }

    public void SaveAmmo()
    {
        stats = GameObject.Find("Inventory").transform.GetChild(0).gameObject.GetComponent<Stats>();
        for(int i = 0; i < stats.Ammo.Length; i++)
        {
            stats.lastSaveAmmo[i] = stats.Ammo[i];
        }

        GameObject.Find("Inventory").GetComponent<Inventory>().SaveInventory();
    }

    public void SetAmmo()
    {
        stats = GameObject.Find("Inventory").transform.GetChild(0).gameObject.GetComponent<Stats>();
        for(int j = 0; j < stats.Ammo.Length; j++)
        {
            stats.Ammo[j] = stats.lastSaveAmmo[j];
        }
        updateAmmo();

        BulletType = stats.BulletType;
        GameObject.Find("Inventory").GetComponent<Inventory>().SetInventory();
    }
}
