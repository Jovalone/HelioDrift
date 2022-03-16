using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCoordinator : MonoBehaviour
{
    public List<GameObject> allies, enemies;
    //public List<GameObject> Ships;
    ShipAI shipAI;
    private GameObject Player;


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        allies.Add(Player);

    }

   public void RemoveFromLists(GameObject ship)
    {
        if (allies.Contains(ship))
        {
            allies.Remove(ship);
        }else if (enemies.Contains(ship))
        {
            enemies.Remove(ship);
        }

        shipAI = ship.GetComponent<ShipAI>();

        foreach (GameObject Ship in allies)
        {
            if (shipAI.allies.Contains(ship))
            {
                shipAI.allies.Remove(ship);
            }
            else if (shipAI.enemies.Contains(ship))
            {
                shipAI.enemies.Remove(ship);
            }
        }

        foreach (GameObject Ship in enemies)
        {
            if (shipAI.allies.Contains(ship))
            {
                shipAI.allies.Remove(ship);
            }
            else if (shipAI.enemies.Contains(ship))
            {
                shipAI.enemies.Remove(ship);
            }
        }
    }
}
