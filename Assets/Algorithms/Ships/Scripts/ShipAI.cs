using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAI : MonoBehaviour
{
    public Transform transform, Base, targetObject;
    public Rigidbody2D TargetRigidbody;
    public GameObject TargetObject, B1, B2, target;
    public GameObject coordinator;
    ShipCoordinator shipCoordinator;
    ShipBase shipBase;
    public Hittable hittable;
    public float minDistToDest;
    public Rigidbody2D rb;
    float angleDiff;
    public float speed = 5f;
    public float RotationalSpeed;
    public List<GameObject> allies, enemies, temp;
    private List<GameObject> TotalEnemies;
    public float BlindSpotRadius;
    public bool Ally;
    public float SenseDist;
    public float FormationDist;
    public float currentValue;
    public GameObject Explosion;
    public shipVision vision;

    public bool Battle = false;

    //ship generator
    public ShipGenerator shipGenerator;
    public GameObject randShip;


    // Start is called before the first frame update
    void Start()
    {
        coordinator = GameObject.Find("ShipCoordinator");
        shipCoordinator = coordinator.GetComponent<ShipCoordinator>();

        target = (GameObject)Instantiate(TargetObject, transform.position, Quaternion.identity);
        OldPos = transform.position;
        NewPos = transform.position;

        randShip = shipGenerator.Ship;

        if (Ally)
        {
            shipCoordinator.allies.Add(gameObject);
        }
        else
        {
            shipCoordinator.enemies.Add(gameObject);
        }
    }

    void LateUpdate()
    {
        if (hittable != null)
        {
            if (hittable.Death)
            {
                shipCoordinator.RemoveFromLists(gameObject);
                Instantiate(Explosion, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

    void FixedUpdate()
    {

        if (Ally)
        {
            allies = shipCoordinator.allies;
        }
        else
        {
            allies = shipCoordinator.enemies;
        }
        
        if (!Battle)
        {
            if (minDistToDest > Vector3.Distance(target.transform.position, transform.position) && !Battle)
            {
                FindNewDestination();
            }

            CheckForEnemy();
            
            if(enemies.Count != 0)
            {
                //enterBattle
                Battle = true;
            }
            
        }
        else
        {
            if (enemies.Count == 0)
            {
                //enterBattle
                Battle = false;
            }
            //battle code
            FindNewTarget();
        }

        Move();

    }

    //Function to move ship towards target
    void Move()
    {
        //Ideal Rotation
        Vector2 Direction = ((Vector2)target.transform.position - rb.position).normalized;
        Direction = ((Vector2)vision.avoidDirection() + Direction).normalized;

        //Calculate new position
        float newX = speed * Mathf.Cos(transform.eulerAngles.z * Mathf.PI / 180);
        float newY = -speed * Mathf.Sin(transform.eulerAngles.z * Mathf.PI / 180);

        rb.velocity = new Vector3(newY, newX, 0);

        float angle = Mathf.Atan2(Direction.x, Direction.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.back);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, RotationalSpeed * Time.fixedDeltaTime);
    }

    //Function to choose a new random spot to patrol
    void FindNewDestination()
    {
        target.transform.position = shipBase.NewPatrolDestination();

        if (BlindSpot(target.transform.position, B1.transform.position) || BlindSpot(target.transform.position, B2.transform.position))
        {
            FindNewDestination();
        }
    }

    //Function that checks the enemies list and scans for nearby enemies
    //need to add communication between planes so the info is relayed
    void CheckForEnemy()
    {
        if (Ally)
        {
            TotalEnemies = shipCoordinator.enemies;
        }
        else
        {
            TotalEnemies = shipCoordinator.allies;
        }

        foreach(GameObject enemy in TotalEnemies)
        {
            if(enemy != null)
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

    //Function that shipBase call at start so ship knows where home base is
    public void SetBase(GameObject shipbase)
    {
        Base = shipbase.transform;
        shipBase = shipbase.GetComponent<ShipBase>();
    }

    //Function to check if new target position is inside the ships blindspot
    bool BlindSpot(Vector3 target, Vector3 blindSpot)
    {
        if (Vector3.Distance(target, blindSpot) < BlindSpotRadius)
        {
            return true;
        }

        return false;

    }

    //Function to find the ships the easiest target
    //maybe should find an easier way to simply just confirm no other ship is easier then current target
    void FindNewTarget()
    {
        //creates a large number then replaces it
        float value = Mathf.Infinity;

        temp = new List<GameObject>();

        foreach (GameObject Enemy in enemies)
        {
            if (Enemy == null)
            {
                temp.Add(Enemy);
            }
            else
            {

                float distValue = Mathf.Pow(Vector3.Distance(transform.position, Enemy.transform.position), 2);
                float angleValue = Mathf.Pow(Vector3.Angle(Enemy.transform.position - transform.position, transform.up) * Mathf.PI / 180, 0.5f);
                if (value > distValue * angleValue)
                {
                    if (!BlindSpot(Enemy.transform.position, B1.transform.position) && !BlindSpot(Enemy.transform.position, B2.transform.position))
                    {
                        targetObject = Enemy.transform;
                        TargetRigidbody = Enemy.GetComponent<Rigidbody2D>();
                        currentValue = distValue * angleValue;
                    }
                }
            }
        }

        foreach(GameObject Enemy in temp)
        {
            enemies.Remove(Enemy);
        }



        if(targetObject == null)
        {
            FindNewDestination();
        }
        else
        {
            target.transform.position = targetObject.position + AllyFormation() + TargetOffset();
        }
    }


    //Function which checks if nearby allies are too close and have the same target causing them to merge into the same spot
    Vector3 AllyFormation()
    {
        foreach(GameObject ally in allies)
        {

            temp = new List<GameObject>();

            if (ally == null)
            {
                temp.Add(ally);
            }
            else
            {
                if (ally != this.gameObject)
                {
                    if (Vector3.Distance(ally.transform.position, transform.position) < FormationDist)
                    {
                        ShipAI shipScript = ally.GetComponent<ShipAI>();
                        if (shipScript != null)
                        {
                            if (targetObject == shipScript.targetObject)
                            {
                                if (shipScript.currentValue < currentValue)
                                {
                                    //apply target offset
                                    float X = -FormationDist * Mathf.Cos(transform.eulerAngles.z * Mathf.PI / 180);
                                    float Y = FormationDist * Mathf.Sin(transform.eulerAngles.z * Mathf.PI / 180);
                                    return shipScript.target.transform.position + new Vector3(X, Y, 0);
                                }
                            }
                        }
                    }
                }
            }
        }

        foreach (GameObject ally in temp)
        {
            allies.Remove(ally);
        }

        return new Vector3(0, 0, 0);
    }

    //              ***** Target Code *****
    
    Vector3 NewPos, OldPos, Direction;
    float EnemySpeed, dist, K, AngleDiff;
    public float BulletSpeed;
    private float OffsetDistance0;
    private Vector3 Offset;

    Vector3 TargetOffset()
    {
        if(OldPos == null)
        {
            OldPos = targetObject.position;
        }
        EnemySpeed = TargetRigidbody.velocity.magnitude;

        Direction = (new Vector2(TargetRigidbody.velocity.y, TargetRigidbody.velocity.x)).normalized;

        AngleDiff = (targetObject.eulerAngles.z - transform.eulerAngles.z) * Mathf.PI / 180;

        dist = Vector3.Distance(targetObject.position, transform.position);

        K = BulletSpeed / EnemySpeed;

        float OffsetDistance1 = (-2 * dist * Mathf.Cos(AngleDiff) + Mathf.Sqrt((Mathf.Pow((2 * dist * Mathf.Cos(AngleDiff)), 2) + 4 * (K - 1) * Mathf.Pow(dist, 2)))) / (2 * (K - 1));
        float OffsetDistance2 = (-2 * dist * Mathf.Cos(AngleDiff) - Mathf.Sqrt((Mathf.Pow((2 * dist * Mathf.Cos(AngleDiff)), 2) + 4 * (K - 1) * Mathf.Pow(dist, 2)))) / (2 * (K - 1));

        if (OffsetDistance1 > OffsetDistance2)
        {
            OffsetDistance0 = OffsetDistance1;
        }
        else
        {
            OffsetDistance0 = OffsetDistance2;
        }
        //create new Vector

        float X = -OffsetDistance0 * Mathf.Sin(targetObject.eulerAngles.z * Mathf.PI / 180);
        float Y = OffsetDistance0 * Mathf.Cos(targetObject.eulerAngles.z * Mathf.PI / 180);

        Offset = new Vector3(X, Y, 0);

        if (Mathf.Abs(AngleDiff) <= 0.0035f || Mathf.Abs(AngleDiff) - Mathf.PI <= 0.0035f)
        {
            return new Vector3(0, 0, 0);
            
        }
        else if (speed == 0f)
        {
            return new Vector3(0, 0, 0);
        }
        else if (float.IsNaN(Offset.x) || float.IsNaN(Offset.y))
        {
            return new Vector3(0, 0, 0);
        }
        else   
        {
            return Offset;
        }
    }

    void OnMouseDown()
    {
        Debug.Log("clicked 1");
        hittable.triggerMouse();
    }
}
