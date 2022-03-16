using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public GameObject Player;
    public Transform transform, target;
    public Rigidbody2D rb;
    float angleDiff;
    public float speed = 5f;
    public float RotationalSpeed;
    public AudioSource audioSource;


    //Charge variables
    public float ChargeCoolDown, ChargeRange, ChargeSpeed, range, angle;
    private float ChargeDist;
    public float time = Mathf.Infinity;
    private int charging = 0;

    public float ChargeWarningTime;
    public float time1 = 0;
    public int State = 0;
    public float speedReduction;

    //Animation
    public Animator animator;

    //HingeJoint test
    public GameObject hingeJoint;

    //TargetList
    public List<GameObject> SquidTargets;
    public List<GameObject> ShipTargets;
    public float squidPriority;
    public float shipPriority;
    public float playerPriority;
    private float value, distValue, angleValue;
    bool findingTarget;

    public Transform BSL, BSR;
    public float BlindSpotDistance;
    public bool LockedOn = false;

    //Death
    public GameObject snake;
    public Hittable hittable;

    void Start()
    {
        Player = GameObject.Find("Player");
        findTargets();
    }

    void FixedUpdate()
    {
        Death();
        findTargets();
        move();
        CheckForTarget();
        //Debug.Log(hingeJoint.GetComponent<HingeJoint2D>().jointAngle);
    }

    void move()
    {
        if(target == null)
        {
            target = transform;
        }

        //Ideal Rotation
        Vector2 Direction = ((Vector2)target.position - rb.position).normalized;

        float dist = charging * Time.fixedDeltaTime * ChargeSpeed;

        //Calculate new position
        float newX = speed * Mathf.Cos(transform.eulerAngles.z * Mathf.PI / 180) + dist * Mathf.Cos(transform.eulerAngles.z * Mathf.PI / 180);
        float newY = -speed * Mathf.Sin(transform.eulerAngles.z * Mathf.PI / 180) - dist * Mathf.Sin(transform.eulerAngles.z * Mathf.PI / 180);


        //Make ChargeDist == 0
        if (ChargeDist != 0)
        {
            if (ChargeDist > 0.05)
            {
                ChargeDist -= Mathf.Abs(dist);
                if (ChargeDist < 0.05)
                {
                    ChargeDist = 0;
                }
            }
            if(ChargeDist == 0)
            {
                State = 0;
                charging = 0;
                animator.SetBool("Charging", false);
            }
        }
        if(State == 0)
        {
            time += Time.deltaTime;
        }

        rb.velocity = new Vector3(newY, newX, 0);

        float angle = Mathf.Atan2(Direction.x, Direction.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle + 50 * Mathf.Sin(Time.time * 3), Vector3.back);
        if (ChargeDist == 0)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, RotationalSpeed * Time.deltaTime);
        }
        if (State == 1)
        {
            time1 += Time.deltaTime;
            if(time1 > ChargeWarningTime)
            {
                time1 = 0;
                Charge();
            }
        }
        
    }

    void CheckForTarget()
    {
        if (time > ChargeCoolDown && LockedOn)
        {
            Debug.Log("checking");
            float angleValue = Vector3.Angle(target.position - transform.position, transform.up);
            float distValue = Vector3.Distance(target.position, transform.position);
            if (angleValue < angle && distValue < range)
            {
                ChargeWarning();
            }
        }
    }

    void ChargeWarning()
    {
        State = 1;
        speed = speed / speedReduction;
        animator.SetTrigger("Warning");
        time = 0;
    }

    void Charge()
    {
        audioSource.Play();
        State = 2;
        speed = speed * speedReduction ;
        ChargeDist = ChargeRange;
        time = 0;
        charging = 1;
        animator.SetBool("Charging", true);
    }

    void findTargets()
    {
        findingTarget = true;
        SquidTargets = new List<GameObject>();
        
        
        
        //= new List<GameObject>();
        ShipTargets.AddRange(GameObject.FindGameObjectsWithTag("AllyShip"));
        ShipTargets.AddRange(GameObject.FindGameObjectsWithTag("EnemyShip"));
        SquidTargets.AddRange(GameObject.FindGameObjectsWithTag("Squid"));
        chooseTarget();
        findingTarget = false;
    }

    void chooseTarget()
    {
        //creates a large number then replaces it
        value = Mathf.Infinity;
        foreach (GameObject enemy in SquidTargets)
        {
            if (enemy == null)
            {
                SquidTargets.Remove(enemy);
            }
            else
            {
                distValue = Mathf.Pow(Vector3.Distance(transform.position, enemy.transform.position), 2);
                angleValue = Mathf.Pow(Vector3.Angle(enemy.transform.position - transform.position, transform.up) * Mathf.PI / 180, 0.5f);
                if (value > distValue * angleValue * shipPriority)
                {
                    LockedOn = true;
                    if (Vector3.Distance(BSL.position, enemy.transform.position) > BlindSpotDistance && Vector3.Distance(BSR.position, enemy.transform.position) > BlindSpotDistance)
                    {
                        //LockedOn = true;
                        target = enemy.transform;
                        value = distValue * angleValue * shipPriority;
                    }
                }
            }
        }
        foreach (GameObject enemy in ShipTargets)
        {
            if (enemy == null)
            {
                ShipTargets.Remove(enemy);
            }
            else
            {
                distValue = Mathf.Pow(Vector3.Distance(transform.position, enemy.transform.position), 2);
                angleValue = Mathf.Pow(Vector3.Angle(enemy.transform.position - transform.position, transform.up) * Mathf.PI / 180, 0.5f);
                if (value > distValue * angleValue * squidPriority)
                {
                    LockedOn = true;
                    if (Vector3.Distance(BSL.position, enemy.transform.position) > BlindSpotDistance && Vector3.Distance(BSR.position, enemy.transform.position) > BlindSpotDistance)
                    {
                        LockedOn = true;
                        target = enemy.transform;
                        value = distValue * angleValue * squidPriority;
                    }
                }
            }
        }

        distValue = Mathf.Pow(Vector3.Distance(transform.position, Player.transform.position), 2);
        angleValue = Mathf.Pow(Vector3.Angle(Player.transform.position - transform.position, transform.up) * Mathf.PI / 180, 0.5f);
        if (value > distValue * angleValue * playerPriority)
        {
            LockedOn = true;
            if (Vector3.Distance(BSL.position, Player.transform.position) > BlindSpotDistance && Vector3.Distance(BSR.position, Player.transform.position) > BlindSpotDistance)
            {
                target = Player.transform;
                value = distValue * angleValue * playerPriority;
            }
        }

        if(value == Mathf.Infinity)
        {
            LockedOn = false;
            target = transform;
        }
    }

    void Death()
    {
        if (hittable.Death)
        {
            if (hittable.Attackers.Contains(Player))
            {
                RecordKeeper.Record.statistics["SnakeKillsRecent"]++;
                Debug.Log(RecordKeeper.Record.statistics["SnakeKillsRecent"]);
            }

            Destroy(snake);
        }
    }
}
