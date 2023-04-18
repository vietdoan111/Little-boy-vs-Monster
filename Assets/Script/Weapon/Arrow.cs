using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Player player;
    public float maxFlyingTime;
    public float maxAppearingTime;
    public Rigidbody2D rb;

    float waitTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>();
    }

    void FixedUpdate()
    {
        waitTime += Time.deltaTime;
        if (waitTime > maxFlyingTime && waitTime < maxAppearingTime)
        {
            Debug.Log("arrow grounded");
            rb.velocity = Vector2.zero; ;
        }

        if (waitTime > maxAppearingTime)
        {
            Destroy(gameObject);
            player.arrowNum++;
            Debug.Log("arrow num: " + player.arrowNum);
        }
    }
}
