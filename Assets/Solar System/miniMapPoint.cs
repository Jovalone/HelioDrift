using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miniMapPoint : MonoBehaviour
{
    public Transform pointer;
    public Transform transform;
    public SpriteRenderer sprite;

    void Update()
    {
        if(pointer != null)
        {
            sprite.enabled = true;

            //transform.LookAt(pointer);
            //find wanted vector
            Vector2 Direction = ((Vector2)pointer.position - (Vector2)transform.position).normalized;
            float angle = Mathf.Atan2(Direction.x, Direction.y) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.back);
            transform.rotation = rotation;
        }
        else
        {
            sprite.enabled = false;
        }
    }
}
