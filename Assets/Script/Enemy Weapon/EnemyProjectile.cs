using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float maxFlyingTime;
    public float maxAppearingTime;
    public Rigidbody2D rb;

    float waitTime = 0.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        waitTime += Time.fixedDeltaTime;
        if (waitTime > maxFlyingTime)
        {
            Debug.Log("fire grounded");
            rb.velocity = Vector2.zero;
        }

        if (waitTime > maxAppearingTime)
        {
            Destroy(gameObject);
            Debug.Log("Destroy fire");
        }
    }
}
