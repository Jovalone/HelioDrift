using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipOrganiser : MonoBehaviour
{
    public List<Ship> AllyShips;
    public List<Ship> Faction1;
    public List<Ship> Faction2;

    //list which holds all sharks, snakes etc
    public List<Hittable> OtherEntities;

    //Player Reputation
    public int Faction1Rep;
    public int Faction2Rep;

    public static ShipOrganiser shipOrganiserInstance;

    void Awake()
    {
        shipOrganiserInstance = this;

        SortLayers(Faction1Rep, 1);

        SortLayers(Faction2Rep, 4);
    }

    public void SortLayers(int rep, int faction)
    {
        if(rep > 5)
        {//not friendly

            //Player and ally bullets
            Physics2D.IgnoreLayerCollision(11, 16 + faction, true);

            //PlayerBullet and allies
            Physics2D.IgnoreLayerCollision(22, 14 + faction, true);
        }
        if(rep <= 5)
        {//friendly with ships

            Physics2D.IgnoreLayerCollision(11, 16 + faction, false);

            //dont hit ships
            Physics2D.IgnoreLayerCollision(22, 14 + faction, false);
        }
    }

    private float time = 1;
    private float time1 = 0;
    private List<Ship> tempShips;

    void Update()
    {
        if(time1 > time)
        {
            time1 = 0;
            tempShips = new List<Ship>();
            foreach(Ship ship in AllyShips)
            {
                if(ship == null)
                {
                    tempShips.Add(ship);
                }
            }
            foreach(Ship ship in tempShips)
            {
                AllyShips.Remove(ship);
            }

            tempShips = new List<Ship>();
            foreach (Ship ship in Faction1)
            {
                if (ship == null)
                {
                    tempShips.Add(ship);
                }
            }
            foreach (Ship ship in tempShips)
            {
                Faction1.Remove(ship);
            }

            tempShips = new List<Ship>();
            foreach (Ship ship in Faction2)
            {
                if (ship == null)
                {
                    tempShips.Add(ship);
                }
            }
            foreach (Ship ship in tempShips)
            {
                Faction2.Remove(ship);
            }
        }
        else
        {
            time1 += Time.deltaTime;
        }
    }
}
