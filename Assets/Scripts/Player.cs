using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public static Player playerInstance;

    //Player Movement
    public Transform transform;
    private Rigidbody2D rb;
    public float speed, minSpeed, maxSpeed;
    public float rotationSpeed, minRotation, maxRotation;
    public float k = 2;
    public float moveVelocity = 0f;
    public float accelerationLinear, accelerationRotational;
    private float X, Y;
    public float RollDist, RollTotal, RollVelocity;
    public float DashDist, DashTotal, DashVelocity;
    public float energyRecoveryRate;

    // boost
    public Boost boost;
    public Slider boostSlider;

    //Animation
    public Animator animator;

    //Player Stats
    public Hittable hittable;
    public float HealthMax;
    public float Health, HealthPct;
    public float Energy, EnergyMax;
    private float EnergyPct;
    public Text healthtxt;
    public Text energytxt;
    public GameObject firePoint;
    public GameObject stats;
    public GameObject DeathScreen;
    public GameObject AmmoButton;
    public GameObject Hologram;
    public GameObject minimap;

    //Other
    public GameObject PlayerShield;
    public AudioSource audio;
    private bool windOn = false;

    //Stats
    public Stats StatScript;
    public StatsBar statsBar;

    //Snake
    public bool spawned = false;
    public GameObject spaceSnake;
    public float SnakeDist;


    void Awake()
    {
        playerInstance = this;
    }

    void Start()
    {
        //Obtain RigidBody
        rb = GetComponent<Rigidbody2D>();

        //Stats
        StatScript = GameObject.Find("Inventory").transform.GetChild(0).GetComponent<Stats>();
        StatScript.SetUp();

        statsBar.SetHealth(Health / HealthMax);
        statsBar.SetEnergy(Energy / EnergyMax);

        //Update initial Stats
        healthtxt.text = Health + "/" + HealthMax;
        energytxt.text = (int)Energy + "/" + EnergyMax;
    }

    void FixedUpdate()
    {
        //check if dead
        if (Health <= 0)
        {
            Die();
        }

        //recover energy
        Energy += energyRecoveryRate * Time.fixedDeltaTime;
        if(Energy > EnergyMax)
        {
            Energy = EnergyMax;
        }

        //Update Shield
        if (Energy < 1)
        {
            PlayerShield.SetActive(false);
        }
        else
        {
            PlayerShield.SetActive(true);
        }

        //Update Stats
        Health = hittable.Health;
        statsBar.SetHealth(Health / HealthMax);
        statsBar.SetEnergy(Energy / EnergyMax);

        healthtxt.text = Health + "/" + HealthMax;
        energytxt.text = (int)Energy + "/" + EnergyMax;

        //Obtain Inputs
        X = Input.GetAxisRaw("Horizontal");
        Y = Input.GetAxisRaw("Vertical");

        //Update Velocity
        if (Y > 0)
        {
            AccelerateLinear();
            AccelerateRotational();
        }
        else
        {
            DecelerateLinear();
            DecelerateRotational();
        }
        this.moveVelocity = this.speed;

        //Calculate new position
        float roll = RollDist * Time.fixedDeltaTime * RollVelocity;

        float dash = 0;
        if (DashDist != 0)
        {
            dash = Time.fixedDeltaTime * DashVelocity;
        }

        float newX = moveVelocity * Mathf.Cos(transform.eulerAngles.z * Mathf.PI / 180) - roll * Mathf.Sin(transform.eulerAngles.z * Mathf.PI / 180) + dash * Mathf.Cos(transform.eulerAngles.z * Mathf.PI / 180);
        float newY = -moveVelocity * Mathf.Sin(transform.eulerAngles.z * Mathf.PI / 180) - roll * Mathf.Cos(transform.eulerAngles.z * Mathf.PI / 180) - dash * Mathf.Sin(transform.eulerAngles.z * Mathf.PI / 180);

        //Drifting
        if (Input.GetKey(KeyCode.LeftShift))
        {
            //Drift
            Debug.Log(rb.velocity);
            rb.velocity *= 0.9975f;
            rb.velocity += (Vector2)WindForce();
            moveVelocity = rb.velocity.magnitude;

            rb.velocity = Vector3.RotateTowards(rb.velocity, transform.up, Time.fixedDeltaTime * rotationSpeed * Mathf.PI / 360, 0);

            if (moveVelocity > speed)
            {
                moveVelocity = speed;
            }
        }
        else
        {
            rb.velocity = new Vector3(newY, newX, 0) + (Vector3)WindForce();
            //Debug.Log(WindForce());
        }
        //WindNoise
        if(WindForce() != new Vector2())
        {
            ScreenShakeController.instance.StartShake(0.1f, 0.1f);
            if (!windOn)
            {
                audio.Play();
                windOn = true;
            }
        }
        else
        {
            audio.Stop();
            windOn = false;
        }

        if (RollDist != 0)
        {
            if (RollDist < -0.05)
            {
                RollDist += Mathf.Abs(roll);
                if (RollDist > -0.05)
                {
                    RollDist = 0;
                }
            }
            else if (RollDist > 0.05)
            {
                RollDist -= Mathf.Abs(roll);
                if (RollDist < 0.05)
                {
                    RollDist = 0;
                }
            }
        }
        if (DashDist != 0)
        {
            if (DashDist > 0.05)
            {
                DashDist -= Mathf.Abs(dash);
                if (DashDist < 0.05)
                {
                    DashDist = 0;
                    boost.endBoost();
                }
            }
        }

        //Update Rotation
        if(X > 0)
        {
            animator.SetBool("TurnRight", true);
        }
        else
        {
            animator.SetBool("TurnRight", false);
        }
        if (X < 0)
        {
            animator.SetBool("TurnLeft", true);
        }
        else
        {
            animator.SetBool("TurnLeft", false);
        }

        rb.rotation -= X * (rotationSpeed - rotationSpeed *  Mathf.Sqrt((speed - moveVelocity)) * k) * Time.fixedDeltaTime;

        if (!spawned)
        {
            Snake();
        }

        if(Mathf.Abs(RollDist) >  DashDist)
        {
            boostSlider.value = 2 * Mathf.Log(Mathf.Abs(RollDist), 0.5f) / Mathf.Log(RollTotal, 0.5f) -1.25f;
        }
        else if (DashDist != 0)
        {
            boostSlider.value = 2.5f * Mathf.Log(DashDist, 0.5f) / Mathf.Log(DashTotal, 0.5f) - 1.5f;
        }
        else
        {
            boostSlider.value = 0;
        }
    }

    /*
    Increases the linear speed of the player.

    The rate of increase in determined by the linear acceleration parameter.
    */
    public void AccelerateLinear() {
        this.speed += this.accelerationLinear * Time.fixedDeltaTime;
        if (this.speed > this.maxSpeed)
        {
            this.speed = this.maxSpeed;
        }
    }

    /*
    Increases the rotational speed of the player.

    The rate of increase in determined by the rotational acceleration parameter.
    */
    public void AccelerateRotational() {
        this.rotationSpeed += this.accelerationRotational * Time.fixedDeltaTime;
        if(this.rotationSpeed > this.maxRotation)
        {
            this.rotationSpeed = this.maxRotation;
        }
    }

    /*
    Decreases the linear speed of the player.

    The rate of decrease in determined by the linear acceleration parameter.
    */
    public void DecelerateLinear() {
        this.speed -= this.accelerationLinear * Time.fixedDeltaTime;
        if (this.speed < this.minSpeed)
        {
            this.speed = this.minSpeed;
        }
    }

    /*
    Decreases the rotational speed of the player.

    The rate of decrease in determined by the rotational acceleration parameter.
    */
    public void DecelerateRotational() {
        this.rotationSpeed -= this.accelerationRotational * Time.fixedDeltaTime;
        if (this.rotationSpeed < this.minRotation)
        {
            this.rotationSpeed = this.minRotation;
        }
    }

    public void TakeDamage(float damage)
	{
        Health -= damage;
        if(Health <= 0)
		{
            Die();
		}
	}

    public void Heal(int heal)
    {
        if(Health + heal > HealthMax)
        {
            hittable.Health = HealthMax;
        }
        else
        {
            hittable.Health = Health + heal;
        }
    }

    public void LoseEnergy(float energy)
	{
        Energy -= energy;
        if (Energy < 0)
		{
            Energy = 0;
		}
    }

    public void Die()
	{
        stats.SetActive(false);
        firePoint.SetActive(false);
        minimap.SetActive(false);
        AmmoButton.SetActive(false);
        Hologram.SetActive(false);
        DeathScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public Trail trail;
    public void BarrelRoll(int dir)
    {
        RollDist = RollTotal * dir;
        trail.startRoutine(2, 0.15f);
    }

    public void Dash()
    {
        DashDist = DashTotal;
    }

    void Snake()
    {
        if(Vector3.Distance(transform.position, new Vector3(0, 0, 0)) > SnakeDist && !StatScript.tutorial)
        {
            Debug.Log("Spawning Snake");
            Instantiate(spaceSnake, transform.position + new Vector3(-100, 50, 0), Quaternion.identity);
            spawned = true;
        }
    }

    private PerlinVectorFlowField Flowfield;
    public Vector2 WindForce()
    {
        Flowfield = PerlinVectorFlowField.flowField;

        if((int)(transform.position.x - Flowfield.transform.position.x + Flowfield.Lifetime + Flowfield.size / 2) > 0 && (int)(transform.position.x - Flowfield.transform.position.x + Flowfield.Lifetime + Flowfield.size / 2) < Flowfield.length)
        {
            if ((int)(transform.position.y - Flowfield.transform.position.y + Flowfield.Lifetime + Flowfield.size / 2) > 0 && (int)(transform.position.y - Flowfield.transform.position.y + Flowfield.Lifetime + Flowfield.size / 2) < Flowfield.length)
            {
                return Flowfield.Forces[(int)(transform.position.x - Flowfield.transform.position.x + Flowfield.Lifetime + Flowfield.size / 2), (int)(transform.position.y - Flowfield.transform.position.y + Flowfield.Lifetime + Flowfield.size / 2)] * Time.deltaTime * 100;
            }
        }

        return new Vector2(0, 0);
    }

}