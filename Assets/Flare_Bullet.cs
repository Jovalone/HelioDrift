using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flare_Bullet : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb;
    public Vector2 acceleration;
    public float maxAcc;
    private float angleOffset;
    private Vector2 temp;
    public float lifetime;
    public float maxAngle;
    public float startAngle;
    public float freq;
    public int dir;

    void Start()
    {
        float angle = Random.Range(-startAngle, startAngle);
        rb.velocity = Quaternion.AngleAxis(angle, Vector3.forward) * transform.up * speed * dir;
        StartCoroutine(selfDestruction());
        angleOffset = Random.Range(0f, 2 * Mathf.PI);
        speed = Random.Range(speed * 0.8f, speed * 1.2f);

    }

    void Update()
    {
        acceleration += new Vector2(Random.Range(-1f, 1f) * Time.deltaTime * maxAcc, Random.Range(-1f, 1f) * Time.deltaTime * maxAcc);
        
        if (acceleration.magnitude > maxAcc)
        {
            temp = acceleration.normalized;
            acceleration = temp * acceleration;
        }

        rb.velocity += acceleration * Time.deltaTime;

        Quaternion rotation = Quaternion.AngleAxis(maxAngle * Mathf.Sin(Time.time * freq + angleOffset) * Time.deltaTime, Vector3.back);
        rb.velocity = rotation * rb.velocity;

        if (rb.velocity.magnitude > speed)
        {
            temp = rb.velocity.normalized;
            rb.velocity = temp * speed;
        }
    }

    IEnumerator selfDestruction()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(this.gameObject);
    }
}
