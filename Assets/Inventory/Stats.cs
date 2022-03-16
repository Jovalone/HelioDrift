using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public static Stats statsInstance;

    public float RegSpeed;
    public float Rotation;
    public float Boost_Total;
    public float Boost_Speed;
    public float Shield;
    public float MaxArmour;
    public float Armour;
    public float MaxEnergy;
    public float Energy;
    public int[] Ammo;
    public int[] lastSaveAmmo;

    public float[] enginePartValues;

    public Player playerScript;
    public Shield shield;
    public Boost boost;
    public Hittable hittable;
    public GameObject playerObject;
    public FirePoint firePoint;
    public int BulletType;
    public ShipStation allyStation;
    public int Followers;

    public int eventNum;

    public int SceneToLoad;

    public bool tutorial;

    void Awake()
    {
        if(statsInstance == null)
        {
            statsInstance = this;
        }
        else
        {
            Destroy(this);
        }
        
        if(GameObject.Find("SolarSystem") != null)
        {
            GameObject.Find("SolarSystem").GetComponent<SolarSystem>().tutorial = tutorial;
        }
    }

    public void SetUp()
    {
        playerObject = GameObject.Find("Player");
        playerScript = playerObject.GetComponent<Player>();
        boost = playerObject.GetComponent<Boost>();
        hittable = playerObject.transform.GetChild(0).GetChild(0).GetComponent<Hittable>();
        shield = playerObject.transform.GetChild(0).GetChild(1).GetComponent<Shield>();
        firePoint = playerObject.transform.GetChild(0).GetChild(4).GetComponent<FirePoint>();
        allyStation = GameObject.Find("AllyBase").GetComponent<ShipStation>();

        hittable.Health = Armour;
        playerScript.Energy = Energy;
        firePoint.SetAmmo(Ammo);

        if(playerScript != null)
        {
            if (tutorial)
            {
                //Regular Speed
                playerScript.Speed = RegSpeed;

                //Rotational Speed
                playerScript.rotationSpeed = Rotation;

                //Boost Speed
                playerScript.DashTotal = Boost_Total;
                playerScript.DashVelocity = Boost_Speed;

                //Shield Strength
                shield.ShieldStrength = Shield;

                //Health
                playerScript.HealthMax = MaxArmour;
                playerScript.hittable.Health = Armour;
                //Armour = playerScript.Health;

                //Energy
                playerScript.EnergyMax = MaxEnergy;
                Energy = playerScript.Energy;

                //Ammo
                Ammo = firePoint.Ammo;
                firePoint.engineFactor = 1;

                //Followers
                allyStation.FighterNum = Followers;
            }
            else
            {
                //Regular Speed
                playerScript.Speed = RegSpeed * enginePartValues[1] / 100;

                //Rotational Speed
                playerScript.rotationSpeed = Rotation * enginePartValues[1] / 100;

                //Boost Speed
                playerScript.DashTotal = Boost_Total * enginePartValues[2] / 100;
                playerScript.DashVelocity = Boost_Speed * enginePartValues[2] / 100;

                //Shield Strength
                shield.ShieldStrength = Shield * enginePartValues[5] / 100;

                //Health
                playerScript.HealthMax = MaxArmour;
                playerScript.hittable.Health = Armour;
                //Armour = playerScript.Health;

                //Energy
                playerScript.EnergyMax = MaxEnergy * enginePartValues[3] / 100;
                Energy = playerScript.Energy;

                //Ammo
                Ammo = firePoint.Ammo;
                firePoint.engineFactor = enginePartValues[4] / 100;

                //Followers
                allyStation.FighterNum = Followers;
            }
        }
    }

    public void SaveStats()
    {
        Armour = playerScript.Health;
        Energy = playerScript.Energy;
        Ammo = firePoint.Ammo;
    }

    public void SaveAmmo()
    {
        for (int i = 0; i < Ammo.Length; i++)
        {
            lastSaveAmmo[i] = Ammo[i];
        }
    }

    public void SetAmmo()
    {
        for (int j = 0; j < Ammo.Length; j++)
        {
            Ammo[j] = lastSaveAmmo[j];
        }
    }
}
