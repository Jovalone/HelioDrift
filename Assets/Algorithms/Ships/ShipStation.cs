using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipStation : MonoBehaviour
{
    //General Variables
    public Transform transform;
    public bool active;
    public bool activated;
    public bool discovered;
    public float radius;
    public bool Combat;
    public GameObject Icon;
    public Ship player;

    //Ship Counts
    public int FighterNum;
    public int TankNum;

    //Ship prefabs
    public GameObject Fighter;
    public GameObject Tank;

    //Spawn Variables
    public float spawnRadius;

    //temporary variables
    GameObject temp;

    //fleet
    public List<Ship> Ships;

    //Enemies
    public int Faction;
    public List<Ship> totalEnemyShips;
    public List<Ship> enemyShips;
    public List<GameObject> potentialEnemies;
    public List<GameObject> otherEnemies;
    List<Ship> shipTemp;

    //Wander Variables
    public float MaxWander;

    //Search Variables
    int search = 0;
    public int SearchFrecuency;

    //Barks
    public Sentence[] Barks;

    void Start()
    {
        Ships = new List<Ship>();
        totalEnemyShips = new List<Ship>();
        enemyShips = new List<Ship>();
        potentialEnemies = new List<GameObject>();
        otherEnemies = new List<GameObject>();

        if (discovered)
        {
            Icon.SetActive(true);
        }
    }

    void Update()
    {
        if (activated)
        {
            if (Faction == 0 || (radius > Vector3.Distance(transform.position, Player.playerInstance.transform.position) && enemyShips.Count == 0 && otherEnemies.Count == 0))
            {
                if (active)
                {
                    //Search
                    if (search < 1)
                    {
                        search = SearchFrecuency;
                        if(Faction == 0)//player allies
                        {
                            totalEnemyShips = new List<Ship>();
                            //Is Faction1 an enemy
                            if(ShipOrganiser.shipOrganiserInstance.Faction1Rep < 5)
                            {
                                //add faction to enemies
                                totalEnemyShips.AddRange(ShipOrganiser.shipOrganiserInstance.Faction1);
                            }
                            //Is Faction2 an enemy
                            if (ShipOrganiser.shipOrganiserInstance.Faction2Rep < 5)
                            {
                                //add faction to enemies
                                totalEnemyShips.AddRange(ShipOrganiser.shipOrganiserInstance.Faction2);
                            }
                        }
                        else if (Faction == 1)
                        {
                            //Search For enemy Ships
                            totalEnemyShips = ShipOrganiser.shipOrganiserInstance.Faction2;

                            //Check if player is an Enemy
                            if (ShipOrganiser.shipOrganiserInstance.Faction1Rep < 5)
                            {
                                if (!totalEnemyShips.Contains(Player.playerInstance.gameObject.GetComponent<Ship>()))
                                {
                                    totalEnemyShips.AddRange(ShipOrganiser.shipOrganiserInstance.AllyShips);
                                }
                            }
                        }
                        else if (Faction == 2)
                        {
                            //Search For enemy Ships
                            totalEnemyShips = ShipOrganiser.shipOrganiserInstance.Faction1;
                            
                            //Check if Player is an enemy
                            if (ShipOrganiser.shipOrganiserInstance.Faction2Rep < 5)
                            {
                                if (!totalEnemyShips.Contains(Player.playerInstance.gameObject.GetComponent<Ship>()))
                                {
                                    totalEnemyShips.AddRange(ShipOrganiser.shipOrganiserInstance.AllyShips);
                                }
                            }
                        }
                        shipTemp = new List<Ship>();
                        foreach (Ship ship in totalEnemyShips)
                        {
                            if (enemyShips.Contains(ship))
                            {
                                shipTemp.Add(ship);
                            }
                        }
                        foreach(Ship ship in shipTemp)
                        {
                            enemyShips.Remove(ship);
                        }
                    }

                    search--;
                }
                else
                {
                    SpawnShips();
                    active = true;
                    discovered = true;//will have to mark on map
                    Icon.SetActive(true);
                }
            }
            else if(active && !Combat)
            {
                DespawnShips();
                active = false;
            }
        }
    }

    void SpawnShips()
    {
        Ships = new List<Ship>();
        totalEnemyShips = new List<Ship>();
        enemyShips = new List<Ship>();
        potentialEnemies = new List<GameObject>();
        otherEnemies = new List<GameObject>();

        Spawn(Fighter, FighterNum);
        Spawn(Tank, TankNum);

        //Add ships to database
        if(Faction == 1)
        {
            ShipOrganiser.shipOrganiserInstance.Faction1.AddRange(Ships);
        }

        if (Faction == 2)
        {
            ShipOrganiser.shipOrganiserInstance.Faction2.AddRange(Ships);
        }

        if(Faction == 0)
        {
            Ships.Add(player);

            ShipOrganiser.shipOrganiserInstance.AllyShips.AddRange(Ships);

            foreach (Ship ship in Ships)
            {
                ship.follow = true;
            }
        }

        foreach(Ship ship in Ships)
        {
            ship.hittable.Allies = new List<Hittable>();

            foreach (Ship ship_0 in Ships)
            {
                ship.hittable.Allies.Add(ship_0.hittable);
            }
        }
    }

    public void DespawnShips()
    {
        foreach (Ship ship in Ships)
        {
            if (ship.Tank && ship == null)
            {
                TankNum--;
            }
            else if (ship == null)
            {
                FighterNum--;
            }

            Destroy(ship.gameObject);
        }
        Ships = new List<Ship>();
    }

    void Spawn(GameObject prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            temp = (GameObject)Instantiate(prefab, transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius), 0), Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
            Ships.Add(temp.GetComponent<Ship>());
            temp.GetComponent<Ship>().shipStation = this;
        }
    }

    public void BattleOn()
    {
        //Trigger the stations bark
        //later check if it was the player that was spotted
        if(Barks.Length != 0)
        {
            //Barks[Random.Range(0, Barks.Length - 1)].Activate();
        }
        Debug.Log(Barks.Length);

        Combat = true;
        foreach(Ship ship in Ships)
        {
            ship.Combat = true;
        }
    }
}
