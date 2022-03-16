using UnityEngine;

public class shipShield : MonoBehaviour
{
    public int damage;

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Hittable hittable = hitInfo.GetComponent<Hittable>();
        if(hittable != null)
        {
            hittable.TakeDamage(damage);
        }
    }
}
