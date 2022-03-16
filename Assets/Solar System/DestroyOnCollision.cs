using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    Hittable hittable;

    void OnTriggerEnter2D(Collider2D collider)
    {

        hittable = collider.gameObject.GetComponent<Hittable>();

        if(hittable != null)
        {
            hittable.Health = 0;
        }
    }
}
