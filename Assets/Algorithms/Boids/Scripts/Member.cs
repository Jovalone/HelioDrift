using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Member : MonoBehaviour
{

    public Vector3 position;
    public Vector3 velocity;
    public Vector3 acceleration;

    public Vector3 save, save1;

    public SquidSpawn spawn;
    public MemberConfig conf;

    private Vector3 wanderTarget;
    private GameObject Player;

    public bool Leader;

    private Vector3 cohesion, alignment, seperate, avoidance, pullBack;

    private float LeaderDist = 0;
    public Vector3 LeaderTarget;
    public Transform Target;

    public Hittable hittable;
    private float dist;
    private float baseDist;
    private GameObject target;
    public float fearFactor = 1f;
    public bool Battle = false;
    public SpriteRenderer sprite;
    public Color colour;

    //Charge Variables
    public ParticleSystem InkAttack;
    public float time, attackCharge;

    //frame improvement
    bool findingNeighbours = false;
    bool findingEnemies = false;
    List<Member> neighbours;
    List<EnemyBoid> enemyList;
    public float testRadius;

    public bool follow;
    public float X, Y;
    private float x, y;

    void Start()
	{
        Player = GameObject.Find("Player");

        conf = MemberConfig.confInstance;

        position = transform.position;
        velocity = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);

        baseDist = Mathf.Infinity;

        foreach (GameObject squidSpawn in GameObject.FindGameObjectsWithTag("SquidSpawn"))
        {
            if (Vector3.Distance(this.transform.position, squidSpawn.transform.position) < baseDist)
            {
                baseDist = Vector3.Distance(this.transform.position, squidSpawn.transform.position);
                spawn = squidSpawn.GetComponent<SquidSpawn>();
            }
        }
        x = Random.Range(0, X);
        y = Random.Range(0, Y);
    }

    void Update()
    {
        if (!follow)
        {
            //wander

            float jitter = conf.wanderJitter * Time.deltaTime;
            wanderTarget += new Vector3(RandomBinomial() * jitter, RandomBinomial() * jitter, 0);
            wanderTarget = wanderTarget.normalized;
            wanderTarget *= conf.wanderRadius;
            Vector3 targetInLocalSpace = wanderTarget + new Vector3(0, conf.wanderDistance, 0);
            Vector3 targetInWorldSpace = transform.TransformPoint(targetInLocalSpace);
            targetInWorldSpace -= this.position;
            Vector3 wander = targetInWorldSpace.normalized;

            //Cohesion
            Vector3 cohesionVector = new Vector3();
            Vector3 alignVector = new Vector3();
            Vector3 seperateVector = new Vector3();

            int countMembers = 0;

            if (!findingNeighbours)
            {
                StartCoroutine(findNeighbours(testRadius));
            }

            if (neighbours.Count == 0)
            {
                cohesion = cohesionVector;
                alignment = alignVector;
                seperate = seperateVector;
            }

            foreach (var member in neighbours)
            {
                if (isInFOV(member.position))
                {
                    cohesionVector += member.position;
                    countMembers++;

                    Vector3 movingTowards = this.position - member.position;
                    seperateVector += movingTowards.normalized / movingTowards.magnitude;

                    alignVector += member.velocity;

                }
            }
            if (countMembers == 0)
            {
                cohesion = cohesionVector;
            }
            cohesionVector /= countMembers;
            cohesionVector = cohesionVector - this.position;
            cohesionVector = Vector3.Normalize(cohesionVector);
            cohesion = cohesionVector;

            alignment = alignVector.normalized;

            seperate = seperateVector.normalized;

            //Avoidance
            Vector3 avoidVector1 = new Vector3();

            if (!findingEnemies)
            {
                StartCoroutine(findEnemies(testRadius));
            }

            foreach (var enemyBoid in enemyList)
            {
                avoidVector1 += RunAway(enemyBoid.position);
            }
            if (Vector3.Distance(position, Player.transform.position) <= conf.avoidanceRadius)
            {
                avoidVector1 += RunAway(Player.transform.position);
            }
            avoidance = avoidVector1.normalized;

            Vector3 gather = new Vector3();
            Vector3 pullBackVector = new Vector3();

            if (!Leader)
            {
                //Gather
                gather = (spawn.target.position - position).normalized;
                LeaderDist = Mathf.Pow(Vector3.Distance(spawn.target.position, position), 2) / 2;
            }

            if (Leader)
            {
                if (LeaderTarget == new Vector3(0, 0, 0))
                {
                    LeaderTarget = spawn.NewPatrolDestination();
                }

                if (Vector3.Distance(LeaderTarget, position) < 3f)
                {
                    LeaderTarget = spawn.NewPatrolDestination();
                }

                wander = ((Vector3)LeaderTarget - position).normalized;

            }

            //Enemy chasing
            Vector3 enemyDir = new Vector3();
            dist = 10f;

            List<GameObject> destroyedAttackers = new List<GameObject>();

            foreach (GameObject attacker in hittable.Attackers)
            {
                if (attacker == null)
                {
                    //hittable.Attackers.Remove(attacker);
                    destroyedAttackers.Add(attacker);
                }
                else if (Vector3.Distance(attacker.transform.position, position) < dist)
                {
                    dist = Vector3.Distance(attacker.transform.position, position);
                    target = attacker;
                    enemyDir = (target.transform.position - position).normalized;
                    fearFactor = 0.2f;
                    sprite.color = colour;
                }
            }

            foreach (GameObject dead in destroyedAttackers)
            {
                hittable.Attackers.Remove(dead);
            }

            //Acceleration
            if (!Leader)
            {
                acceleration = conf.cohesionPriority * cohesion + conf.wanderPriority * wander
               + conf.alignmentPriority * alignment + conf.separationPriority * seperate
               + conf.avoidancePriority * avoidance * fearFactor + conf.pullBackPriority * pullBack + conf.gatherPriority * gather * LeaderDist + enemyDir * conf.enemyPriority;

                acceleration = Vector3.ClampMagnitude(acceleration, conf.maxAcceleration);
                velocity = velocity + acceleration * Time.deltaTime;
                velocity = Vector3.ClampMagnitude(velocity, conf.maxVelocity);
            }
            else
            {
                acceleration = conf.cohesionPriority * cohesion + conf.wanderPriority * wander * 5
               + conf.alignmentPriority * alignment + conf.separationPriority * seperate
               + conf.avoidancePriority * avoidance * fearFactor + conf.pullBackPriority * pullBack + conf.gatherPriority * gather * LeaderDist + enemyDir * conf.enemyPriority;

                acceleration = Vector3.ClampMagnitude(acceleration, conf.maxAcceleration);
                velocity = (velocity + acceleration * Time.deltaTime) * 0.95f;
                velocity = Vector3.ClampMagnitude(velocity, conf.maxVelocity);
            }
            position = position + velocity * Time.deltaTime;
            position.z = 0;
            transform.position = position;

            time -= Time.deltaTime;
        }
        else
        {
            //Debug.Log("working");
            acceleration = (Target.position - transform.position);
            acceleration = Vector3.ClampMagnitude(acceleration, conf.maxAcceleration);
            velocity = velocity + acceleration * Time.deltaTime;
            velocity = Vector3.ClampMagnitude(velocity, conf.maxVelocity);

            position = position + velocity * Time.deltaTime;
            position.z = 0;
            if(Vector3.Distance(transform.position, Target.position) < 12)
            {
                acceleration = Vector3.ClampMagnitude(acceleration, conf.maxAcceleration * 3);
                velocity = Vector3.ClampMagnitude(velocity, conf.maxVelocity / 2f);
            }
            if (Vector3.Distance(transform.position, Target.position) < 0.2f)// will need to tweak later for lower frame rates
            {
                //Debug.Log("latching");
                position = Target.position;
            }
            transform.position = position;
        }
    }

    Vector3 RunAway(Vector3 target)
	{
        Vector3 neededVelocity = (position - target).normalized * conf.maxVelocity;
        return neededVelocity - velocity;
	}
    float RandomBinomial()
	{
        return Random.Range(-1f, 1f) - Random.Range(-1f, 1f);
	}

    bool isInFOV(Vector3 vec)
	{
        return Vector3.Angle(this.velocity, vec - this.position) <= conf.maxFOV;
	}

    public void death()
    {
        spawn.Quantity--;
    }

    IEnumerator findNeighbours(float radius)
    {
        findingNeighbours = true;

        neighbours = spawn.getNeighbours(this, radius);

        yield return new WaitForSeconds(Time.deltaTime * x);
        x = X;
        findingNeighbours = false;
    }

    IEnumerator findEnemies(float radius)
    {
        findingEnemies = true;

        enemyList = spawn.GetEnemyBoids(this, conf.avoidanceRadius);

        yield return new WaitForSeconds(Time.deltaTime * y);
        y = Y;
        findingEnemies = false;
    }

}
