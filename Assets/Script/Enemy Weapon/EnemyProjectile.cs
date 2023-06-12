using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float maxFlyingTime;
    public float maxAppearingTime;
    public Vector2 targetPos;

    float waitTime = 0.0f;

    private void FixedUpdate()
    {
        waitTime += Time.fixedDeltaTime;
        if (waitTime < maxFlyingTime)
            transform.position = Vector2.MoveTowards(transform.position, targetPos, 7.0f * Time.fixedDeltaTime);

        if (waitTime > maxAppearingTime)
        {
            Destroy(gameObject);
            Debug.Log("Destroy fire");
        }
    }
}
