using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform transform;
    public Transform target;
    public Hittable hittable;
    public GameObject turret;

    public GameObject coordinator;
    public List<GameObject> enemies;
    public List<GameObject> temp;
    public List<GameObject> TotalEnemies;

    public float rotationalSpeed;
    public float SenseDist;
    private float currentValue;

    public bool Ally;
    public bool Battle;
    //Outdated needs to be changed to the new Ships script

    void Update()
    {
        Debug.Log("Update Script");
    }
        /*
        void Start()
        {
            coordinator = GameObject.Find("ShipCoordinator");
            shipCoordinator = coordinator.GetComponent<ShipCoordinator>();
            if(shipCoordinator != null)
            {
                if (Ally)
                {
                    shipCoordinator.allies.Add(gameObject);
                }
                else
                {
                    shipCoordinator.enemies.Add(gameObject);
                }
            }
        }

        void Update()
        {
            CheckForEnemy();
            FindNewTarget();


            if(target != null)
            {
                Battle = true;
                move();
            }
            else
            {
                Battle = false;
            }

            if(hittable.Health <= 0)
            {
                Destroy(turret);
            }
        }

        void move()
        {
            Vector3 Direction = (target.position - transform.position).normalized;

            float angle = Mathf.Atan2(Direction.x, Direction.y) * Mathf.Rad2Deg;

            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.back);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationalSpeed * Time.fixedDeltaTime);
        }

        void CheckForEnemy()
        {
            if(shipCoordinator != null)
            {
                if (Ally)
                {
                    TotalEnemies = shipCoordinator.enemies;
                }
                else
                {
                    TotalEnemies = shipCoordinator.allies;
                }
            }

            foreach (GameObject enemy in TotalEnemies)
            {
                if (enemy != null)
                {
                    if (Vector3.Distance(enemy.transform.position, transform.position) < SenseDist)
                    {
                        if (!enemies.Contains(enemy))
                        {
                            enemies.Add(enemy);
                        }
                    }
                }
            }
        }

        void FindNewTarget()
        {
            //creates a large number then replaces it
            float value = Mathf.Infinity;

            temp = new List<GameObject>();

            foreach (GameObject Enemy in enemies)
            {
                if (Enemy == null || Vector3.Distance(Enemy.transform.position, transform.position) > SenseDist)
                {
                    temp.Add(Enemy);
                }
                else
                {

                    float distValue = Mathf.Pow(Vector3.Distance(transform.position, Enemy.transform.position), 2);
                    float angleValue = Mathf.Pow(Vector3.Angle(Enemy.transform.position - transform.position, transform.up) * Mathf.PI / 180, 0.5f);
                    if (value > distValue * angleValue)
                    {
                        target = Enemy.transform;
                        currentValue = distValue * angleValue;
                    }
                }
            }

            foreach (GameObject Enemy in temp)
            {
                enemies.Remove(Enemy);
            }
        }*/
    }
