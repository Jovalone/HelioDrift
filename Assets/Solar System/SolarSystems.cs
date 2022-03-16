using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarInfo
{
    public float size;
    public int SolarNumber;

    //HomeBase Info
    public Vector3 homePosition;

    //Player Info
    public Vector3 currentPlayerSpawn;

    //Asteroid Belt info
    public int beltNum;
    //public int[] AsteroidNum;
    public int[] LargeAsteroids, MediumAsteroids, SmallAsteroids;
    public float[] innerRadius;
    public float[] outerRadius;

    //Planets
    public int PlanetNum;
    public float[] orbitRadius;
    public Vector3[] planetLocations;

    //SquidSpawnerInfo
    public int squidSpawnerNum;
    public int[] SquidNum;
    public Vector3[] Locations;
    public bool[] squidDiscovery;

    //Faction 1
    public GameObject Faction_station_1;
    public int stationNum_1;
    public Vector3[] station_Locations_1;
    public int[] FighterNum_1;
    public int[] TankNum_1;
    public float station_Ri_1;
    public float station_Ro_1;
    public bool[] stationDiscovery_1;

    //Faction 2
    public GameObject Faction_station_2;
    public int stationNum_2;
    public Vector3[] station_Locations_2;
    public int[] FighterNum_2;
    public int[] TankNum_2;
    public float station_Ri_2;
    public float station_Ro_2;
    public bool[] stationDiscovery_2;

    //Collectables
    public int collectableNum;
    public Vector3[] collectableLocations;
    public bool[] pickedUp;
}

public class SolarSystems : MonoBehaviour
{
    bool empty = true;
    public List<StarInfo> solarSystems;
    public SolarSystem solarSystem;

    public int SolarSystemToLoad;

    void Awake()
    {
        solarSystems = new List<StarInfo>();
    }

    public void GenerateSeed()
    {
        solarSystem = GameObject.Find("SolarSystem").GetComponent<SolarSystem>();

        if (!empty)//((solarSystems.Count > (SolarSystemToLoad - 1)) && (SolarSystemToLoad > 0))
        {
            solarSystem.GenerateSolarSystem(solarSystems[SolarSystemToLoad - 1]);
        }
        else
        {
            empty = false;
            solarSystem.GenerateNewSolarSystem();
            SaveNewScene();
            SolarSystemToLoad = solarSystems.Count;
        }
    }

    public void SaveNewScene()
    {
        StarInfo star = new StarInfo();

        star.size = solarSystem.size;
        solarSystems.Add(star);
        star.SolarNumber = solarSystems.Count - 1;

        //HomeBase Info
        star.homePosition = solarSystem.homePosition;

        //Player Info
        star.currentPlayerSpawn = solarSystem.playerTransform.position;

        //Asteroid Info
        star.beltNum = solarSystem.beltNum;
        //star.AsteroidNum = solarSystem.AsteroidNum;
        star.LargeAsteroids = solarSystem.LargeAsteroids;
        star.MediumAsteroids = solarSystem.MediumAsteroids;
        star.SmallAsteroids = solarSystem.SmallAsteroids;

        star.innerRadius = solarSystem.innerRadius;
        star.outerRadius = solarSystem.outerRadius;

        //PlantInfo
        star.PlanetNum = solarSystem.PlanetNum;
        star.orbitRadius = solarSystem.orbitRadius;
        star.planetLocations = solarSystem.planetLocations;

        //Faction 1 station Info
        star.stationNum_1 = solarSystem.stationNum_1;
        star.station_Locations_1 = solarSystem.station_Locations_1;
        star.FighterNum_1 = solarSystem.FighterNum_1;
        star.TankNum_1 = solarSystem.TankNum_1;
        star.stationDiscovery_1 = solarSystem.stationDiscovery_1;

        //Faction 2 station Info
        star.stationNum_2 = solarSystem.stationNum_2;
        star.station_Locations_2 = solarSystem.station_Locations_2;
        star.FighterNum_2 = solarSystem.FighterNum_2;
        star.TankNum_2 = solarSystem.TankNum_2;
        star.stationDiscovery_2 = solarSystem.stationDiscovery_2;

        //Squid Info
        star.squidSpawnerNum = solarSystem.squidSpawnerNum;
        star.SquidNum = solarSystem.SquidNum;
        star.Locations = solarSystem.Locations;
        star.squidDiscovery = solarSystem.squidDiscovery;

        //Collectables
        star.collectableNum = solarSystem.collectableNum;
        star.collectableLocations = solarSystem.collectableLocations;
        star.pickedUp = solarSystem.pickedUp;

    }

    public void UpdateScene()
    {
        //StarInfo star = solarSystems[SolarSystemToLoad - 1];
        StarInfo star = solarSystems[0];
        star.size = solarSystem.size;
        solarSystems.Add(star);
        star.SolarNumber = solarSystems.Count - 1;
        solarSystem.firePoint.SaveAmmo();

        //HomeBase Info
        star.homePosition = solarSystem.homeTransform.position;

        //Player Info
        star.currentPlayerSpawn = solarSystem.playerTransform.position;

        //AsteroidInfo
        star.beltNum = solarSystem.beltNum;
        //star.AsteroidNum = solarSystem.AsteroidNum;
        star.LargeAsteroids = solarSystem.LargeAsteroids;
        star.MediumAsteroids = solarSystem.MediumAsteroids;
        star.SmallAsteroids = solarSystem.SmallAsteroids;

        star.innerRadius = solarSystem.innerRadius;
        star.outerRadius = solarSystem.outerRadius;

        //PlantInfo
        star.PlanetNum = solarSystem.PlanetNum;
        star.orbitRadius = solarSystem.orbitRadius;
        star.planetLocations = new Vector3[solarSystem.PlanetNum];
        for(int i = 0; i < solarSystem.PlanetNum; i++)
        {
            star.planetLocations[i] = solarSystem.planetTransforms[i].position;
        }

        //Faction 1 Info
        star.stationNum_1 = solarSystem.stationNum_1;//assumes that it will be possible to later destroy stations
        star.TankNum_1 = new int[star.stationNum_1];
        star.FighterNum_1 = new int[star.stationNum_1];

        for (int j = 0; j < solarSystem.stationNum_1; j++)
        {
            star.station_Locations_1[j] = solarSystem.Faction_Stations_1[j].transform.position;
            //Count Ships
            solarSystem.Faction_Stations_1[j].DespawnShips();//despawn ships to allow count
            star.FighterNum_1[j] = solarSystem.Faction_Stations_1[j].FighterNum;
            star.TankNum_1[j] = solarSystem.Faction_Stations_1[j].TankNum;
            star.stationDiscovery_1[j] = solarSystem.Faction_Stations_1[j].discovered;
        }

        //Faction 2 Info
        star.stationNum_2 = solarSystem.stationNum_2;
        star.TankNum_2 = new int[star.stationNum_2];
        star.FighterNum_2 = new int[star.stationNum_2];

        for (int j = 0; j < solarSystem.stationNum_2; j++)
        {
            star.station_Locations_2[j] = solarSystem.Faction_Stations_2[j].transform.position;
            //Count Ships
            solarSystem.Faction_Stations_2[j].DespawnShips();//despawn ships to allow count
            star.FighterNum_2[j] = solarSystem.Faction_Stations_2[j].FighterNum;
            star.TankNum_2[j] = solarSystem.Faction_Stations_2[j].TankNum;
            star.stationDiscovery_2[j] = solarSystem.Faction_Stations_2[j].discovered;
        }

        //SquidInfo
        star.squidSpawnerNum = solarSystem.squidSpawnerNum;
        solarSystem.UpdateSquidNum();
        star.SquidNum = solarSystem.SquidNum;
        star.Locations = solarSystem.Locations;
        star.squidDiscovery = new bool[solarSystem.squidSpawnerNum];

        for (int l = 0; l < solarSystem.squidSpawnerNum; l++)
        {
            star.squidDiscovery[l] = solarSystem.squidSpawns[l].discovered;
        }

        //Collectables
        star.collectableNum = solarSystem.collectableNum;
        star.collectableLocations = solarSystem.collectableLocations;
        star.pickedUp = solarSystem.pickedUp;

        //GameObject[] list = solarSystem.collectableList;

        for (int l = 0; l < star.collectableNum; l++)
        {
            if(solarSystem.collectableList[l] == null)
            {
                star.pickedUp[l] = true;
                //Debug.Log("false");
            }
            else
            {
                //Debug.Log("true");
            }
        }
    }
}
