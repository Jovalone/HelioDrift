using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystem : MonoBehaviour
{
    public bool testing;

    public GameObject StarPrefab;
    public GameObject Planet;
    public GameObject AsteroidBelt;
    public GameObject SquidSpawner;
    private GameObject star, aBelt, sSpawn, planet;
    public float size;
    SolarSystems Systems;
    GravitySource gravitySource;
    AsteroidBelt asteroidBelt;
    SquidSpawn squidSpawn;
    public float[] spawnDistance;
    private int x = 0;
    public float[] asteroidOffset;
    public FirePoint firePoint;

    private float angle, dist;
    public Vector3[] allLocations;
    private int Num = 0;

    //HomeBase
    public GameObject HomeBase;
    public float homeRi, homeRo;
    public Vector3 homePosition;
    public Transform homeTransform;

    //Player
    public GameObject Player;
    public Transform playerTransform;
    public FarStars farStars;
    public FarStars farStars1;

    //Planets
    public int PlanetNum;
    public float[] orbitRadius;
    public Vector3[] planetLocations;
    public Transform[] planetTransforms;

    //AsteroidBelts
    public int beltNum;
    public int[] LargeAsteroids, MediumAsteroids, SmallAsteroids;
    public float[] innerRadius;
    public float[] outerRadius;

    public Transform transform;

    //SquidSpawnerInfo
    public int squidSpawnerNum;
    public SquidSpawn[] squidSpawns;
    public int[] SquidNum;
    public Vector3[] Locations;
    public bool[] squidDiscovery;

    // **Bases**
    private GameObject shipBase;
    ShipStation Station;

    //Faction 1
    public GameObject Faction_station_1;
    public int stationNum_1;
    public Vector3[] station_Locations_1;
    public int[] FighterNum_1;
    public int[] TankNum_1;
    public float station_Ri_1;
    public float station_Ro_1;
    public ShipStation[] Faction_Stations_1;
    public bool[] stationDiscovery_1;

    //Faction 2
    public GameObject Faction_station_2;
    public int stationNum_2;
    public Vector3[] station_Locations_2;
    public int[] FighterNum_2;
    public int[] TankNum_2;
    public float station_Ri_2;
    public float station_Ro_2;
    public ShipStation[] Faction_Stations_2;
    public bool[] stationDiscovery_2;

    //Collectables
    public int collectableNum;
    public GameObject collectable;
    public GameObject[] collectableList;
    private GameObject temp;
    public Vector3[] collectableLocations;
    public bool[] pickedUp;
    public GameObject Intro;

    //Sharks
    public GameObject shark;
    public int sharkNum;
    public Vector3[] sharkLocations;

    public GameObject shark2;
    public int sharkNum2;
    public Vector3[] sharkLocations2;

    //Tutorial Scene
    public bool tutorial;


    void Start()
    {
        tutorial = Stats.statsInstance.tutorial;
        Systems = GameObject.Find("SolarSystems").GetComponent<SolarSystems>();

        Systems.GenerateSeed();
    }

    public void GenerateNewSolarSystem()
    {
        Debug.Log("generate new");

        star = (GameObject)Instantiate(StarPrefab, transform);
        gravitySource = star.GetComponent<GravitySource>();
        size = Random.Range(0.75f, 1.25f);
        gravitySource.GravityStrength = 0.5f * size;
        gravitySource.OrbitStrength = 1 * size;
        star.transform.localScale = size * new Vector3(500, 500, 0);
        firePoint.SaveAmmo();
        Intro.SetActive(true);

        allLocations = new Vector3[PlanetNum + squidSpawnerNum + stationNum_1 + stationNum_2 + 1];//sharkQuantity + 1];

        //Asteroid Generation
        beltNum = 4;

        innerRadius = new float[beltNum];
        outerRadius = new float[beltNum];

        LargeAsteroids = new int[beltNum];
        MediumAsteroids = new int[beltNum];
        SmallAsteroids = new int[beltNum];

        for (int j = 0; j < beltNum; j++)
        {
            LargeAsteroids[j] = Random.Range(20, 40) + j * 20;
            MediumAsteroids[j] = Random.Range(100, 200) + j * 100;
            SmallAsteroids[j] = Random.Range(50, 100) + j * 50;

            innerRadius[j] = Random.Range(85, 105) + j * 150;
            outerRadius[j] = Random.Range(120, 145) + j * 150;

            aBelt = (GameObject)Instantiate(AsteroidBelt, transform);
            asteroidBelt = aBelt.GetComponent<AsteroidBelt>();

            asteroidBelt.Ri = innerRadius[j];
            asteroidBelt.Ro = outerRadius[j];
            asteroidBelt.large = LargeAsteroids[j];
            asteroidBelt.medium = MediumAsteroids[j];
            asteroidBelt.small = SmallAsteroids[j];

            asteroidBelt.SpawnAsteroidBelt();
        }

        //HomeBase Generation
        homePosition = FindNewLocation(homeRi, homeRo);
        temp = (GameObject)Instantiate(HomeBase, transform);
        homeTransform = temp.transform;
        temp.transform.position = homePosition;
        Num++;
        x++;

        //Player Location
        if (!testing)
        {
            Player.transform.position = homePosition;
        }
        playerTransform = Player.transform;
        farStars.SetUp();
        farStars1.SetUp();

        //Planet Generation
        orbitRadius = new float[PlanetNum];
        planetLocations = new Vector3[PlanetNum];
        planetTransforms = new Transform[PlanetNum];

        for (int h = 0; h < PlanetNum; h++)
        {
            dist = orbitRadius[h];
            angle = Random.Range(0, 2 * Mathf.PI);
            planetLocations[h] = FindNewLocation(100, 400);//transform.position + new Vector3(dist * Mathf.Cos(angle), dist * Mathf.Sin(angle), 0);
            Num++;
            planet = (GameObject)Instantiate(Planet, transform);
            planetTransforms[h] = planet.transform;
            planet.transform.position = planetLocations[h];
        }
        x++;

        //Faction 1 Station Generation
        station_Locations_1 = new Vector3[stationNum_1];
        FighterNum_1 = new int[stationNum_1];
        TankNum_1 = new int[stationNum_1];
        Faction_Stations_1 = new ShipStation[stationNum_1];
        stationDiscovery_1 = new bool[stationNum_1];

        for(int k = 0; k < stationNum_1; k++)
        {
            //Spawn Station
            station_Locations_1[k] = FindNewLocation(station_Ri_1, station_Ro_1);
            Num++;
            Station = Instantiate(Faction_station_1, transform).GetComponent<ShipStation>();
            Faction_Stations_1[k] = Station;
            Station.transform.position = station_Locations_1[k];

            //Set up Ship numbers
            Station.FighterNum = Random.Range(5, 10);
            Station.TankNum = Random.Range(0, 2);

            Station.activated = true;
        }

        //Faction 2 Station Generation
        station_Locations_2 = new Vector3[stationNum_2];
        FighterNum_2 = new int[stationNum_2];
        TankNum_2 = new int[stationNum_2];
        Faction_Stations_2 = new ShipStation[stationNum_2];
        stationDiscovery_2 = new bool[stationNum_2];

        for (int k = 0; k < stationNum_2; k++)
        {
            //Spawn Station
            station_Locations_2[k] = FindNewLocation(station_Ri_2, station_Ro_2);
            Num++;
            Station = Instantiate(Faction_station_2, transform).GetComponent<ShipStation>();
            Faction_Stations_2[k] = Station;
            Station.transform.position = station_Locations_2[k];

            //Set up Ship numbers
            Station.FighterNum = Random.Range(5, 10);
            Station.TankNum = Random.Range(0, 2);

            Station.activated = true;
        }
        x++;

        //Squid spawner Generation
        SquidNum = new int[squidSpawnerNum];
        squidSpawns = new SquidSpawn[squidSpawnerNum];
        Locations = new Vector3[squidSpawnerNum];
        squidDiscovery = new bool[squidSpawnerNum];

        for (int i = 0; i < squidSpawnerNum; i++)
        {
            SquidNum[i] = Random.Range(45, 60);
            //dist = Random.Range(100, 300);
            angle = Random.Range(0, 2 * Mathf.PI);
            Locations[i] = FindNewLocation(100, 300);//transform.position + new Vector3(dist * Mathf.Cos(angle), dist * Mathf.Sin(angle), 0);
            Num++;
            sSpawn = (GameObject)Instantiate(SquidSpawner, transform);
            squidSpawn = sSpawn.GetComponent<SquidSpawn>();

            squidSpawns[i] = squidSpawn;
            squidSpawn.Quantity = SquidNum[i];
            sSpawn.transform.position = Locations[i];

            //squidSpawn.SpawnSquid();
            squidSpawn.Active = true;
        }

        //Shark Generation
        sharkLocations = new Vector3[sharkNum];

        for (int s = 0; s < sharkNum; s++)
        {
            dist = Random.Range(80, 350);
            angle = Random.Range(0, 2 * Mathf.PI);
            sharkLocations[s] = transform.position + new Vector3(dist * Mathf.Cos(angle), dist * Mathf.Sin(angle), 0);

            if (!tutorial)
            {
                temp = (GameObject)Instantiate(shark, transform);
                temp.transform.position = sharkLocations[s];
            }
        }

        sharkLocations2 = new Vector3[sharkNum2];
        for (int s = 0; s < sharkNum2; s++)
        {
            dist = Random.Range(80, 350);
            angle = Random.Range(0, 2 * Mathf.PI);
            sharkLocations2[s] = transform.position + new Vector3(dist * Mathf.Cos(angle), dist * Mathf.Sin(angle), 0);

            if (!tutorial)
            {
                temp = (GameObject)Instantiate(shark2, transform);
                temp.transform.position = sharkLocations2[s];
            }
        }

        //Collectable Generation
        collectableLocations = new Vector3[collectableNum];
        pickedUp = new bool[collectableNum];
        collectableList = new GameObject[collectableNum];

        for(int p = 0; p < collectableNum; p++)
        {
            pickedUp[p] = false;

            dist = Random.Range(80, 350);
            angle = Random.Range(0, 2 * Mathf.PI);
            collectableLocations[p] = transform.position + new Vector3(dist * Mathf.Cos(angle), dist * Mathf.Sin(angle), 0);

            temp = (GameObject)Instantiate(collectable, transform);
            temp.transform.position = collectableLocations[p];
            collectableList[p] = temp;
            if (tutorial)
            {
                temp.SetActive(false);
            }

        }
}

    public void GenerateSolarSystem(StarInfo starInfo)
    {
        Debug.Log("regenerate");

        star = (GameObject)Instantiate(StarPrefab, transform);
        gravitySource = star.GetComponent<GravitySource>();
        size = starInfo.size;
        gravitySource.GravityStrength = 500 * size;
        gravitySource.OrbitStrength = 1 * size;
        star.transform.localScale = size * new Vector3(500, 500, 0);
        beltNum = starInfo.beltNum;
        squidSpawnerNum = starInfo.squidSpawnerNum;
        firePoint.SetAmmo();

        //Asteroid

        LargeAsteroids = starInfo.LargeAsteroids;
        MediumAsteroids = starInfo.MediumAsteroids;
        SmallAsteroids = starInfo.SmallAsteroids;

        innerRadius = starInfo.innerRadius;
        outerRadius = starInfo.outerRadius;

        for (int k = 0; k < beltNum; k++)
        {

            aBelt = (GameObject)Instantiate(AsteroidBelt, transform);
            asteroidBelt = aBelt.GetComponent<AsteroidBelt>();

            asteroidBelt.Ri = starInfo.innerRadius[k];
            asteroidBelt.Ro = starInfo.outerRadius[k];
            //asteroidBelt.quantity = starInfo.AsteroidNum[k];
            asteroidBelt.large = starInfo.LargeAsteroids[k];
            asteroidBelt.medium = starInfo.MediumAsteroids[k];
            asteroidBelt.small = starInfo.SmallAsteroids[k];

            asteroidBelt.SpawnAsteroidBelt();
        }

        //HomeBase
        temp = (GameObject)Instantiate(HomeBase, transform);
        temp.transform.position = starInfo.homePosition;
        homeTransform = temp.transform;
        //Debug.Log(starInfo.homePosition);

        //Player
        Player.transform.position = starInfo.currentPlayerSpawn;
        playerTransform = Player.transform;
        farStars.SetUp();
        farStars1.SetUp();

        //Planets
        PlanetNum = starInfo.PlanetNum;
        orbitRadius = starInfo.orbitRadius;
        planetLocations = starInfo.planetLocations;
        planetTransforms = new Transform[PlanetNum];

        for (int m = 0; m < PlanetNum; m++)
        {
            planet = (GameObject)Instantiate(Planet, transform);
            planet.transform.position = planetLocations[m];
            planetTransforms[m] = planet.transform;
        }

        //Faction 1 Station Re-Generation
        stationNum_1 = starInfo.stationNum_1;
        station_Locations_1 = starInfo.station_Locations_1;
        FighterNum_1 = starInfo.FighterNum_1;
        TankNum_1 = starInfo.TankNum_1;
        stationDiscovery_1 = starInfo.stationDiscovery_1;
        Faction_Stations_1 = new ShipStation[stationNum_1];

        if (!tutorial)
        {
            for (int k = 0; k < stationNum_1; k++)
            {
                //Spawn Station
                Station = Instantiate(Faction_station_1, transform).GetComponent<ShipStation>();
                Faction_Stations_1[k] = Station;
                Station.transform.position = station_Locations_1[k];

                //Set up Ship numbers
                Station.FighterNum = FighterNum_1[k];
                Station.TankNum = TankNum_1[k];
                Debug.Log(TankNum_1[k]);

                //Set up Discovery
                Station.discovered = stationDiscovery_1[k];
                Station.activated = true;
            }
        }

        //Faction 2 Station Re-Generation
        stationNum_2 = starInfo.stationNum_2;
        station_Locations_2 = starInfo.station_Locations_2;
        FighterNum_2 = starInfo.FighterNum_2;
        TankNum_2 = starInfo.TankNum_2;
        stationDiscovery_2 = starInfo.stationDiscovery_2;
        Faction_Stations_2 = new ShipStation[stationNum_2];

        if (!tutorial)
        {
            for (int k = 0; k < stationNum_2; k++)
            {
                //Spawn Station
                Station = Instantiate(Faction_station_2, transform).GetComponent<ShipStation>();
                Faction_Stations_2[k] = Station;
                Station.transform.position = station_Locations_2[k];

                //Set up Ship numbers
                Station.FighterNum = FighterNum_2[k];
                Station.TankNum = TankNum_2[k];

                //Set up Discovery
                Station.discovered = stationDiscovery_2[k];
                Station.activated = true;
            }
        }

        //Squid Spawn
        SquidNum = starInfo.SquidNum;
        Locations = starInfo.Locations;
        squidSpawns = new SquidSpawn[starInfo.squidSpawnerNum];
        squidDiscovery = starInfo.squidDiscovery;

        for (int l = 0; l < squidSpawnerNum; l++)
        {
            sSpawn = (GameObject)Instantiate(SquidSpawner, transform);
            squidSpawn = sSpawn.GetComponent<SquidSpawn>();

            squidSpawns[l] = squidSpawn;
            SquidNum = starInfo.SquidNum;
            squidSpawn.Quantity = starInfo.SquidNum[l];
            sSpawn.transform.position = starInfo.Locations[l];
            squidSpawn.discovered = squidDiscovery[l];
            squidSpawn.Active = true;
        }

        collectableLocations = starInfo.collectableLocations;
        pickedUp = starInfo.pickedUp;
        collectableList = new GameObject[collectableNum];


        //Collectable
        if (!tutorial)
        {
            for (int q = 0; q < collectableNum; q++)
            {
                if (!pickedUp[q] && !tutorial)
                {
                    temp = (GameObject)Instantiate(collectable, transform);
                    temp.transform.position = collectableLocations[q];
                    collectableList[q] = temp;
                }
            }
        }
    }

    Vector3 FindNewLocation(float Ri, float Ro)
    {
        dist = Random.Range(Ri, Ro);
        angle = Random.Range(0, 2 * Mathf.PI);
        Vector3 Location = transform.position + new Vector3(dist * Mathf.Cos(angle), dist * Mathf.Sin(angle), 0);
        bool redo = false;
        for(int m = 0; m < beltNum; m++)
        {
            if (dist > (innerRadius[m] - asteroidOffset[x])  && dist < (innerRadius[m] + asteroidOffset[x]))
            {
                redo = true;
            }
        }

        for(int n = 0; n < Num; n++)
        {
            if(Vector3.Distance(Location, allLocations[n]) < spawnDistance[x])
            {
                redo = true;
            }
        }

        if (redo)
        {
            Location = FindNewLocation(Ri, Ro + 1);
        }
        allLocations[Num] = Location;
        return Location;
    }

    public void UpdateSquidNum()
    {
        int v = 0;
        foreach(SquidSpawn squid in squidSpawns)
        {
            SquidNum[v] = squid.Quantity;
            v++;
        }
    }
}
