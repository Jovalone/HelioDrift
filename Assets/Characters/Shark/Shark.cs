using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : MonoBehaviour
{
    public bool Social;

    public Rigidbody2D rb;
    public float speed, Speed, chaseSpeed;
    public float RotationalSpeed, Rotation, chaseRotation;
    public Vector3 target;
    public Transform Target;
    public Hittable targetHittable;
    public float minDist;
    public float huntDist;
    public bool hunting = false;
    public int Strength;
    public Hittable hittable;

    public Transform BS1, BS2;
    public float blindSpotdist;

    public bool following;
    public int leaderStrength;
    public List<Shark> followers, temp;
    private Shark tempShark;
    public float FormationDist;
    public float leaderDist;

    public int packStrength;

    //public shipVision vision;
    public List<Hittable> Danger;
    public List<Hittable> Targets;
    public List<Hittable> followersHittable;
    private int frame;
    private int loop;

    void Start()
    {
        if (Social)
        {
            Strength = Random.Range(125, 175);
            followers.Add(this);
            Level.levelInstance.sharkLeaders.Add(this);
            Level.levelInstance.sharks.Add(this);
        }
        else
        {
            Strength = 150;
        }
        rb.velocity = transform.up * speed;
        hittable.Health = Strength;
        transform.localScale = Mathf.Pow(((float)Strength / 140), 2) * new Vector3(1, 1, 0);
        Danger = new List<Hittable>();

        frame = Random.Range(0, 5);
        loop = Random.Range(0, 5);
    }

    void Update()
    {
        //Check if dead
        if(hittable != null && hittable.Health <= 0)
        {
            if (Social)
            {
                //check if any followers and mark as not following
                if (!following)
                {
                    foreach (Shark shark in followers)
                    {
                        shark.following = false;
                    }
                    Level.levelInstance.sharkLeaders.Remove(this);
                }
            }

            //check if player killed shark
            if (hittable.Attackers.Contains(Player.playerInstance.gameObject))
            {
                //player was an attacker so give kill
                RecordKeeper.Record.statistics["SharkKillsRecent"]++;
                Debug.Log(RecordKeeper.Record.statistics["SharkKillsRecent"]);
            }

            Level.levelInstance.sharks.Remove(this);
            Destroy(this.gameObject);
        }

        if(Vector3.Distance(transform.position, Player.playerInstance.transform.position) < 100)
        {//if player is close enough to shark activate

            if (!following)//leader Shark
            {
                if(Social && followers.Count < 3)//find a stronger shark to follow
                {
                    findAlly();
                }

                if (frame < 0)
                {
                    frame = 5;
                    loop--;
                    if(loop < 0)
                    {
                        Targets = new List<Hittable>();
                        Danger = new List<Hittable>();
                        loop = 5;

                        //set up new strength in hittable
                        if (Social)
                        {
                            hittable.Allies = new List<Hittable>();
                            hittable.Allies.Add(hittable);
                            foreach (Shark shark in followers)
                            {
                                hittable.Allies.Add(shark.hittable);
                            }
                            followersHittable = hittable.Allies;

                            foreach (Shark shark in followers)//fill in allies with same info
                            {
                                shark.hittable.Allies = hittable.Allies;
                            }
                        }
                    }
                    TargetList();//sorts out who are valid targets and dangers
                }
                else
                {
                    frame--;
                }

                if (hunting)//there is an weak enemy present
                {
                    findTarget();
                }
            }
            else
            {
                if (hunting)//look for a target in the leaders list
                {
                    findTargetFollower();
                }
            }

            Move();
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    void Move()
    {
        if (hunting)
        {
            Attack();

            speed = chaseSpeed;
            if(Targets.Count == 0 && !following)
            {
                hunting = false;
                foreach (Shark shark in followers)
                {
                    shark.hunting = false;
                }
                newGoal();
            }
            else if(Target != null)
            {
                target = Target.position;
            }
        }
        else
        {
            CheckGoal();
        }

        Vector3 avoidVector = new Vector3();

        if (following)
        {
            leaderDist = Vector3.Distance(tempShark.transform.position, transform.position);

            if(Target != null)
            {
                target = Target.position;
            }
            else
            {
                following = false;
            }

            foreach(Shark shark in tempShark.followers)
            {
                if(shark != null)
                {
                    if (shark != this && Vector3.Distance(shark.transform.position, transform.position) < FormationDist)
                    {
                        avoidVector += (transform.position - shark.transform.position).normalized;
                    }
                }
            }

            if(Vector3.Distance(transform.position, tempShark.transform.position) > FormationDist * 2)
            {
                speed = chaseSpeed;
                RotationalSpeed = chaseRotation;
            }
            else
            {
                speed = Speed;
                RotationalSpeed = Rotation;
            }

            //look for potential threats
            foreach (Hittable danger in tempShark.Danger)
            {
                if(danger != null)
                {
                    if (Vector3.Distance(danger.transform.position, transform.position) < 5)
                    {
                        avoidVector += (transform.position - danger.transform.position).normalized * 1.2f;
                    }
                }
            }
            //Avoid mantas
            foreach (Manta manta in Level.levelInstance.Mantas)
            {
                if (Vector3.Distance(manta.transform.position, transform.position) < 15)
                {
                    avoidVector += (transform.position - manta.transform.position).normalized * 1.2f;
                }
            }
        }
        else
        {
            packStrength = 0;
            foreach (Shark shark in followers)
            {
                packStrength += shark.Strength;
            }

            foreach (Shark shark in followers)
            {
                if(shark != null)
                {
                    if (shark != this && Vector3.Distance(shark.transform.position, transform.position) < FormationDist)
                    {
                        avoidVector += (transform.position - shark.transform.position).normalized;
                    }
                }
            }

            //look for potential dangers
            foreach (Hittable danger in Danger)
            {
                if(danger != null)
                {
                    if (Vector3.Distance(danger.transform.position, transform.position) < 5)//will manta need another variable?
                    {
                        avoidVector += (transform.position - danger.transform.position).normalized * 1.2f;
                    }
                }
            }
            //Avoid mantas
            foreach(Manta manta in Level.levelInstance.Mantas)
            {
                if (Vector3.Distance(manta.transform.position, transform.position) < 15)
                {
                    avoidVector += (transform.position - manta.transform.position).normalized * 1.2f;
                }
            }
        }

        //Ideal Rotation
        Vector2 Direction = ((Vector2)target - rb.position).normalized + (Vector2)avoidVector;// + (Vector2)vision.avoidDirection();

        //Calculate new position
        float newX = speed * Mathf.Cos(transform.eulerAngles.z * Mathf.PI / 180);
        float newY = -speed * Mathf.Sin(transform.eulerAngles.z * Mathf.PI / 180);

        rb.velocity = new Vector3(newY, newX, 0);

        float angle = Mathf.Atan2(Direction.x, Direction.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.back);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, RotationalSpeed * Time.deltaTime);
    }

    void CheckGoal()
    {
        if(target == Vector3.zero)
        {
            newGoal();
        }else if(Vector3.Distance(target, transform.position) < minDist)
        {
            newGoal();
        }
    }

    void newGoal()
    {
        target = transform.position + new Vector3(Random.Range(-huntDist, huntDist), Random.Range(-huntDist, huntDist), 0);

        if (!BlindSpotCheck(target) && Vector3.Distance(target, Vector3.zero) < 350)
        {
            newGoal();
        }
    }

    bool BlindSpotCheck(Vector3 vector)
    {
        if(Vector3.Distance(vector, BS1.position) < blindSpotdist)
        {
            return false;
        }
        else if(Vector3.Distance(vector, BS1.position) < blindSpotdist)
        {
            return false;
        }
        return true;
    }

    void TargetList()
    {
        foreach(Hittable hittable in Level.levelInstance.liveCharacters)
        {
            if(50 > Vector3.Distance(hittable.transform.position, transform.position))
            {
                if (!Targets.Contains(hittable) && !Danger.Contains(hittable) && !followersHittable.Contains(hittable))
                {
                    //Find Strength of target
                    float allyStrength = 0;
                    foreach (Hittable ally in hittable.Allies)
                    {
                        allyStrength += ally.Health;
                    }

                    //is target weak? attack
                    if (allyStrength < (packStrength / 2))
                    {
                        hunting = true;
                        foreach (Shark shark in followers)
                        {
                            shark.hunting = true;
                        }
                        Targets.Add(hittable);
                    }

                    //is Target Strong? danger
                    if (allyStrength > packStrength)
                    {
                        Danger.Add(hittable);
                    }
                }
            }
        }
    }

    private float currentValue;
    void findTarget()
    {
        float value = Mathf.Infinity;
        foreach(Hittable hittable in Targets)
        {
            if(hittable != null)
            {
                float distValue = Mathf.Pow(Vector3.Distance(transform.position, hittable.transform.position), 3);
                float angleValue = Mathf.Pow(Vector3.Angle(hittable.transform.position - transform.position, transform.up) * Mathf.PI / 180, 1f);

                if (value > distValue * angleValue)
                {
                    if (BlindSpotCheck(hittable.transform.position))
                    {
                        Target = hittable.transform;
                        targetHittable = hittable;
                        currentValue = distValue * angleValue;
                        value = distValue * angleValue;
                    }
                }
            }
        }
    }

    void findTargetFollower()
    {
        float value = Mathf.Infinity;
        foreach (Hittable hittable in tempShark.Targets)
        {
            if(hittable != null)
            {
                float distValue = Mathf.Pow(Vector3.Distance(transform.position, hittable.transform.position), 3);
                float angleValue = Mathf.Pow(Vector3.Angle(hittable.transform.position - transform.position, transform.up) * Mathf.PI / 180, 1f);

                if (value > distValue * angleValue * Vector3.Distance(tempShark.transform.position, hittable.transform.position))
                {
                    if (BlindSpotCheck(hittable.transform.position))
                    {
                        Target = hittable.transform;
                        targetHittable = hittable;
                        currentValue = distValue * angleValue * Vector3.Distance(tempShark.transform.position, hittable.transform.position);
                        value = distValue * angleValue;

                    }
                }
            }
        }
    }

    void findAlly()
    {
        int value = 1000;
        foreach (Shark shark in Level.levelInstance.sharks)
        {

            if(shark.following != true && Vector3.Distance(shark.transform.position, transform.position) < huntDist && shark != this)
            {
                if (shark.Strength > Strength + 5)
                {
                    temp = new List<Shark>();

                    int sharkValue = 0;
                    foreach(Shark packMember in shark.followers)
                    {
                        if(packMember.Strength > Strength)
                        {
                            sharkValue++;
                        }
                    }

                    if(sharkValue < value && shark.followers.Count < 5)
                    {
                        leaderStrength = shark.Strength;
                        Target = shark.transform;
                        following = true;
                        tempShark = shark;
                        value = sharkValue;
                        Danger = new List<Hittable>();
                        Targets = new List<Hittable>();

                        foreach (Shark follower in followers)
                        {
                            follower.following = false;
                        }
                        followers = new List<Shark>();
                    }
                }
            }
        }
        if (following)
        {
            tempShark.followers.Add(this);
            if (tempShark.hunting)
            {
                Target = tempShark.Target;
                Level.levelInstance.sharkLeaders.Remove(this);
            }
        }
    }

    Vector3 RunAway(Vector3 target)
    {
        Vector3 neededVelocity = (transform.position - target).normalized;
        return neededVelocity - transform.up * speed;
    }

    //
    public float chargeTime;
    public int damage;
    public bool holding;
    private float time;
    public Transform attackTran;
    public Animator attackAnimator;

    public void Attack()
    {
        if(Target != null)
        {
            if (time < 0)
            {
                if (Vector3.Distance(attackTran.position, Target.position) < 0.5)//close enough to attack
                {
                    //Attack
                    targetHittable.TakeDamage(damage);
                    time = chargeTime;
                    attackAnimator.SetTrigger("Attack");
                }
            }
            else
            {
                time -= Time.deltaTime;
            }
        }
    }
}
