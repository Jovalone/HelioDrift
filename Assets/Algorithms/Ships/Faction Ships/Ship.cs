using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    //General Variables
    public bool uncontrolled;
    public Transform transform;
    public Rigidbody2D rb;
    public ShipStation shipStation;

    private List<GameObject> tempObjects;
    private List<Ship> tempShips;
    public Hittable hittable;
    public int ogHealth;
    public GameObject SmokeEffect;
    public GameObject explosion;
    public Animator animator;

    //Ship Parameters
    public float speed;
    public float OGSpeed;
    public float speedMax;
    public float speedMin;
    public float RotationalSpeed;

    //Target
    public float value;
    public float targetAngle;
    public bool Combat;
    public Vector2 Target;
    public Transform target;
    public Rigidbody2D targetRB;
    public Ship targetShip;
    public float patrolDist;
    public float patrolProximity;

    public Transform leftBlindSpot;
    public Transform rightBlindSpot;
    public float blindSpotDistance;
    public float currentValue;
    public bool follow;
    public float ogImportance;
    public float importance;

    //Ship Coordination
    public List<Ship> allies;
    public List<Ship> enemies;
    public List<GameObject> otherEnemies;

    public List<Ship> ShipsChasing;
    public List<Ship> ShipsBehind;

    public bool chased;
    public float FormationDist;

    //Search Parameters
    public float minSearch;
    public float maxSight;
    public float sightAngle;
    public Vision vision;

    //Run Time
    public int time;
    private float time_0 = 0;

    private float distHabit;

    void Start()
    {
        Target = (Vector2)transform.position;
        ShipsBehind = new List<Ship>();
        ShipsChasing = new List<Ship>();

        time_0 = Random.Range(0, time);
        distHabit = Random.Range(1.5f, 2.5f);
    }

    void FixedUpdate()
    {
        //Check if dead
        if (!uncontrolled && hittable.Death)
        {
            //destroy ship
            Instantiate(explosion, transform.position, Quaternion.identity);

            if (targetShip != null)
            {
                targetShip.ShipsBehind.Remove(this);
                targetShip.ShipsChasing.Remove(this);
            }
            
            shipStation.Ships.Remove(this);

            if(helpShip != null)
            {
                helpShip.helpShip = null;
                helpShip.original = false;
            }

            if (hittable.Attackers.Contains(Player.playerInstance.gameObject))
            {
                if (Tank)
                {
                    RecordKeeper.Record.statistics["Faction"+ shipStation.Faction +"TankKillsRecent"]++;
                    Debug.Log(RecordKeeper.Record.statistics["Faction" + shipStation.Faction + "TankKillsRecent"]);
                }
                else
                {
                    RecordKeeper.Record.statistics["Faction" + shipStation.Faction + "FighterKillsRecent"]++;
                    Debug.Log(RecordKeeper.Record.statistics["Faction" + shipStation.Faction + "FighterKillsRecent"]);
                }
            }

            Destroy(this.gameObject);
        }
        else
        {
            if(hittable.Health / ogHealth < 0.2)
            {
                SmokeEffect.SetActive(true);
            }
        }

        if (!uncontrolled)
        {
            Move();
        }

        time_0--;

        if(time_0 < 0)
        {
            time_0 = time;

            if (!Combat)
            {
                speed = OGSpeed - speedMin;
                CheckForEnemies();
                //Check if following the player
                if (follow)
                {
                    Follow();
                }
                else
                {
                    //Check if close to patrol point
                    Patrol();
                }
            }
            else
            {
                //Battle
                CheckForEnemies();
                if (helpShip == null)
                {
                    Chase();
                    if (target != null)
                    {
                        Target = (Vector2)target.position + (Vector2)TargetOffset() + (Vector2)AllyFormation();
                        if (LockedOn() && !behind && targetShip != null)
                        {
                            Behind();
                        }

                        if (LockedOn())// && ShipsBehind.Count == 0)
                        {
                            //Check if target ship is slowing down relative
                            Speed();
                        }
                    }
                }
                else
                {
                    if (original)
                    {
                        if (ShipsBehind.Count == 0)
                        {
                            chased = false;
                            helpShip.helpShip = null;
                            helpShip = null;
                            original = false;
                        }
                    }
                }
            }
        }
    }

    //          ***Advanced Scripts***

    void Move()
    {
        if (follow)
        {
            if(!Combat && 10 < Vector2.Distance(Player.playerInstance.transform.position, transform.position))
            {
                speed = 7;
            }
            else
            {
                speed = 5;
            }
        }

        Vector2 Direction = new Vector2();

        if (vision.checkVision() || uncontrolled)
        {
            //find ideal direction
            Direction = ((Vector2)Target - rb.position).normalized;
            if (helpShip != null)
            {
                Direction = BackUp();
            }
        }
        else
        {
            Direction = vision.newDirection;
        }

        //Calculate new position
        float newX = speed * Mathf.Cos(transform.eulerAngles.z * Mathf.PI / 180);
        float newY = -speed * Mathf.Sin(transform.eulerAngles.z * Mathf.PI / 180);

        rb.velocity = new Vector3(newY, newX, 0);

        float angle = Mathf.Atan2(Direction.x, Direction.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.back);

        //Debug.Log(Quaternion.Angle(rotation, transform.rotation));
        if(Quaternion.Angle(rotation, transform.rotation) >= 30)
        {
            //play turning animation
            //first find which way to turn
            if(Vector3.Angle(transform.right, Direction) > 90)
            {
                animator.SetInteger("X", 1);
            }
            else
            {
                animator.SetInteger("X", -1);
            }
        }
        else if (Quaternion.Angle(rotation, transform.rotation) >= 5)
        {
            animator.SetInteger("X", 0);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, RotationalSpeed * Time.fixedDeltaTime);
    }

    void Patrol()
    {
        if (Vector2.Distance(Target, rb.position) < patrolProximity)
        {
            newPatrolTarget();
        }

        //Check that patrol position isnt too close to the ship
        if (Vector2.Distance((Vector2)leftBlindSpot.position, Target) < blindSpotDistance)
        {
            newPatrolTarget();
        }
        else if (Vector2.Distance((Vector2)rightBlindSpot.position, Target) < blindSpotDistance)
        {
            newPatrolTarget();
        }
    }

    public float space;
    void Follow()
    {
        Vector2 pos = (Vector2)Player.playerInstance.transform.position;
        float allyDist = Mathf.Infinity;
        space = Vector3.Distance(transform.position, Player.playerInstance.transform.position);

        foreach (Ship ally in shipStation.Ships)
        {
            if (2 > Vector3.Distance(transform.position, ally.transform.position) && space > ally.space)
            {
                if(allyDist > Vector3.Distance(transform.position, ally.transform.position))
                {
                    pos = (Vector2)ally.transform.position;
                }
            }
        }
        
        float X = -5 * Mathf.Cos(transform.eulerAngles.z * Mathf.PI / 180);
        float Y = 5 * Mathf.Sin(transform.eulerAngles.z * Mathf.PI / 180);

        Target = pos + new Vector2(X, Y);
    }

    void newPatrolTarget()
    {
        float dist = Random.Range(0, patrolDist);
        float angle = Random.Range(0, 2 * Mathf.PI);

        Target = (Vector2)shipStation.transform.position + new Vector2(dist * Mathf.Cos(angle), dist * Mathf.Sin(angle));
    }

    void CheckForEnemies()
    {
        tempShips = new List<Ship>();
        tempObjects = new List<GameObject>();
        bool found = false;

        //Look for enemy Ships
        foreach (Ship ship in shipStation.totalEnemyShips)
        {
            if(ship != null)
            {
                if (Vector3.Distance(ship.transform.position, transform.position) < maxSight)
                {
                    if (Vector3.Angle(ship.transform.position - transform.position, transform.up) < sightAngle)
                    {
                        //Add ship to found ship list
                        shipStation.enemyShips.Add(ship);
                        tempShips.Add(ship);

                        found = true;
                    }
                }
            }
            else
            {

            }
        }

        //Look For other Enemies
        foreach (GameObject Object in shipStation.potentialEnemies)
        {
            if (Vector3.Distance(Object.transform.position, transform.position) < maxSight)
            {
                if (Vector3.Angle(Object.transform.position - transform.position, transform.up) < sightAngle)
                {
                    //Add ship to found ship list
                    if (!shipStation.otherEnemies.Contains(Object))
                    {
                        shipStation.otherEnemies.Add(Object);
                        tempObjects.Add(Object);

                        found = true;
                    }
                }
            }
        }

        //Remove Objects from List
        if (tempShips.Count != 0)
        {
            foreach (Ship ship in tempShips)
            {
                shipStation.totalEnemyShips.Remove(ship);
            }
        }

        if (tempObjects.Count != 0)
        {
            foreach (GameObject Object in tempObjects)
            {
                shipStation.potentialEnemies.Remove(Object);
            }
        }

        if (!Combat && found)
        {
            shipStation.BattleOn();
        }
    }

    public void Chase()
    {
        //Check for closest ship
        value = Mathf.Infinity;//creates a large number then replaces it
        int n = 0;

        foreach (GameObject Object in shipStation.otherEnemies)
        {
            n++;
            float distValue = Mathf.Pow(Vector3.Distance(transform.position, Object.transform.position), 3);
            float angleValue = Mathf.Pow(Vector3.Angle(Object.transform.position - transform.position, transform.up) * Mathf.PI / 180, 1f);

            if (value > distValue * angleValue)
            {
                if (!BlindSpot(Object.transform.position, leftBlindSpot.transform.position) && !BlindSpot(Object.transform.position, rightBlindSpot.transform.position))
                {
                    target = Object.transform;
                    targetRB = Object.GetComponent<Rigidbody2D>();
                    currentValue = distValue * angleValue;
                    behind = false;
                    value = distValue * angleValue;
                    targetAngle = angleValue * 180 / Mathf.PI;

                    if (targetShip != null)
                    {
                        targetShip.ShipsBehind.Remove(this);
                        targetShip.ShipsChasing.Remove(this);
                    }
                }
            }
        }

        tempShips = new List<Ship>();
        foreach (Ship ship in shipStation.enemyShips)
        {
            if(ship != null)
            {
                n++;
                float distValue = Mathf.Pow(Vector3.Distance(transform.position, ship.transform.position), 3);
                float angleValue = Mathf.Pow(Vector3.Angle(ship.transform.position - transform.position, transform.up) * Mathf.PI / 180, 1f);

                if (value > (distValue * angleValue) / importance)
                {
                    if (!BlindSpot(ship.transform.position, leftBlindSpot.transform.position) && !BlindSpot(ship.transform.position, rightBlindSpot.transform.position))
                    {
                        if (targetShip != null)
                        {
                            if (targetShip != ship)
                            {
                                behind = false;
                                targetShip.ShipsBehind.Remove(this);
                                targetShip.ShipsChasing.Remove(this);
                                ship.ShipsChasing.Add(this);
                            }
                        }

                        target = ship.transform;
                        targetRB = ship.rb;
                        currentValue = distValue * angleValue;
                        value = distValue * angleValue;
                        targetAngle = angleValue * 180 / Mathf.PI;
                        ship.chased = true;
                        targetShip = ship;
                    }
                }
            }
            else
            {
                tempShips.Add(ship);
            }
            
        }
        //remove all old ships
        foreach(Ship ship in tempShips)
        {
            shipStation.enemyShips.Remove(ship);
        }
        if(n == 0)
        {
            Combat = false;
        }

        //Debug.Log(value);
    }

    //Function to check if new target position is inside the ships blindspot
    bool BlindSpot(Vector3 target, Vector3 blindSpot)
    {
        if (Vector3.Distance(target, blindSpot) < blindSpotDistance)
        {
            return true;
        }

        return false;
    }

    //              ***** Target Code *****

    Vector3 Direction;
    public float EnemySpeed, dist, K, AngleDiff;
    public float BulletSpeed;
    private float OffsetDistance0;
    private Vector3 Offset;

    Vector3 TargetOffset()
    {

        EnemySpeed = targetRB.velocity.magnitude;
        if(EnemySpeed > BulletSpeed)
        {//make sure target is not moving too fast
            EnemySpeed = BulletSpeed - 0.5f;
        }

        Direction = Quaternion.Euler(0, 0, targetRB.angularVelocity) * (Vector3)targetRB.velocity.normalized * dist / BulletSpeed; 

        AngleDiff = Vector3.Angle(transform.up, target.up) * Mathf.PI / 180;

        dist = Vector3.Distance(target.position, transform.position);

        float A = Mathf.Pow(EnemySpeed, 2) - Mathf.Pow(BulletSpeed, 2);
        float B = -2 * dist * EnemySpeed * Mathf.Cos(AngleDiff);
        float C = Mathf.Pow(dist, 2);

        float offsetDistance = EnemySpeed * (-B + Mathf.Sqrt(Mathf.Pow(B, 2) - 4*A*C))/(2*A) * 0.6f;

        Offset = -Direction * Mathf.Clamp(offsetDistance, -dist / distHabit, dist / distHabit);
        return Offset;
    }
    public Ship testShip;

    Vector3 AllyFormation()
    {
        //Ally Formation
        if (targetShip != null)
        {
            //foreach (Ship ally in targetShip.ShipsChasing)
            foreach(Ship ally in shipStation.Ships)
            {
                if (ally != this)
                {
                    //Check ally distance
                    float ally_dist = Vector3.Distance(ally.transform.position, transform.position);

                    if (ally_dist < FormationDist || ally_dist < ally.FormationDist)
                    {
                        //compare current values
                        if (ally.currentValue < currentValue)
                        {
                            if(ally.FormationDist > FormationDist)
                            {
                                ally_dist = ally.FormationDist;
                            }
                            else
                            {
                                ally_dist = FormationDist;
                            }
                            //apply target offset
                            float X = -2 * ally_dist * Mathf.Cos(transform.eulerAngles.z * Mathf.PI / 180);
                            float Y = 2 * ally_dist * Mathf.Sin(transform.eulerAngles.z * Mathf.PI / 180);
                            //return shipScript.target.transform.position + new Vector3(X, Y, 0);
                            testShip = ally;
                            return new Vector3(X, Y, 0);
                        }
                    }
                }
            }
        }
        return new Vector3();
    }

    public float LockOnAngle;
    private float Angle;
    private float AngleDiffB;
    private bool behind;

    //Check if right behind targeted ship
    public bool LockedOn()
    {
        Angle = K / (Mathf.Sqrt(Vector3.Distance(transform.position, target.position)));
        Vector3 Direction = (target.position - transform.position).normalized;
        AngleDiff = Vector3.Angle(transform.up, Direction);

        return AngleDiff < Angle; //might need to tweak this number to become more accurate possibly raise the power
    }

    public void Behind()
    {
        Vector3 Direction = (transform.position - target.position).normalized;//direction ship is facing
        AngleDiffB = Vector3.Angle(-target.up, Direction);//check if other ship is facing the other way

        if (AngleDiffB < LockOnAngle)
        {
            //if the ship is locked on, send message to ship being chased that it has a ship on its tail
            targetShip.ShipsBehind.Add(this);
            behind = true;
            importance = ogImportance * 5;
        }
        else
        {
            importance = ogImportance;
        }
    }

    public bool Tank;
    public Ship helpShip;
    private bool original = false;
    private Vector2 midAxis;
    private float AngleValue;
    private bool helpReady;

    public void CoordinateHelp()
    {
        //Find the most appropriate ship to help
        foreach (Ship ship in shipStation.Ships)
        {
            //for now find the nearest Tank
            if (ship.Tank && ship != this && ship.helpShip == null)
            {
                helpShip = ship;
                ship.helpShip = this;
                original = true;
                return;
            }
        }

        //Choose another random Ship
        foreach (Ship ship in shipStation.Ships)
        {
            //for now find the nearest ship
            if (ship != this && ship.helpShip == null)
            {
                helpShip = ship;
                ship.helpShip = this;
                original = true;
                return;
            }
        }
    }

    public Vector2 BackUp()
    {
        if (!helpReady)
        {
            //Coordinate movement
            float D = 180 * speed / (Mathf.PI * RotationalSpeed) + 180 * helpShip.speed / (Mathf.PI * helpShip.RotationalSpeed);

            //first check if they are far enough away
            if (Vector3.Distance(transform.position, helpShip.transform.position) > D)
            {
                //find the mid Axis
                if (original)
                {
                    midAxis = Vector2.Perpendicular(helpShip.transform.position - transform.position).normalized;
                }
                else
                {
                    midAxis = helpShip.midAxis;
                }

                //calculate value
                AngleValue = Vector2.Angle(midAxis, transform.up);

                //Check if value is less, greater or equal
                if (Mathf.Abs(AngleValue - helpShip.AngleValue) < 5)
                {
                    helpReady = true;
                    helpShip.helpReady = true;
                }
                else if (AngleValue > helpShip.AngleValue)
                {
                    //turn towards mid Axis
                    return midAxis;
                }
                else
                {
                    return -midAxis;
                }
            }
            return transform.up;
        }
        else
        {
            //help is ready
            return (helpShip.transform.position - transform.position).normalized;
        }
    }

    public float idealFollowDist;
    public float Kp;

    void Speed()
    {
        float followDist = Vector3.Distance(targetRB.position, transform.position);
        followDist -= idealFollowDist;

        speed += Kp * followDist * Time.fixedDeltaTime;
        speed = Mathf.Clamp(speed, OGSpeed - speedMin, OGSpeed + speedMax);
    }

}

