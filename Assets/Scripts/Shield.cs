using UnityEngine;
using UnityEngine.UI;

public class Shield : MonoBehaviour
{
    public GameObject Player;
    private Player PlayerScript;
    public GameObject shield;
    public float ShieldStrength;
    public Transform transform;
    private float Angle;
    private Vector3 dir;
    CircleCollider2D collider;
    public Text Shieldtxt;


    void Start()
	{
        Player = GameObject.Find("Player");
        PlayerScript = Player.GetComponent<Player>();
        collider = GetComponent<CircleCollider2D>();

        collider.enabled = false;
        Shieldtxt.enabled = false;
    }

    void Update()
	{
        if (Input.GetKeyDown("f"))
        {
            collider.enabled = !collider.enabled;
            Shieldtxt.enabled = collider.enabled;
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        dir = hitInfo.transform.position - transform.position;

        Angle = Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg;

        GameObject TempShield = Instantiate(shield, transform.position, Quaternion.Euler(new Vector3(0, 0, Angle)), transform);

        Bullet bullet = hitInfo.GetComponent<Bullet>();
        if (bullet != null)
        {
            ScreenShakeController.instance.StartShake(0.2f, 0.075f * bullet.damage);
            PlayerScript.LoseEnergy(bullet.damage / ShieldStrength);
        }

    }
}
