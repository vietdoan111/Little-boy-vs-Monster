using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArrowState
{
    flying,
    grounded
}

public class Arrow : MonoBehaviour
{
    public Player player;
    public float maxFlyingTime;
    public float maxAppearingTime;
    public Rigidbody2D rb;
    public ArrowState state;

    float waitTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        if (state == ArrowState.grounded) rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void FixedUpdate()
    {
        waitTime += Time.deltaTime;
        if (waitTime > maxFlyingTime)
        {
            Debug.Log("arrow grounded");
            rb.velocity = Vector2.zero;
            state = ArrowState.grounded;
        }

        if (waitTime > maxAppearingTime)
        {
            Destroy(gameObject);
            player.arrowNum++;
            Debug.Log("arrow num: " + player.arrowNum);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            if (state == ArrowState.grounded) return;
            rb.velocity = Vector2.zero;
            state = ArrowState.grounded;
            Enemy enemy = collision.collider.GetComponent<Enemy>();
            enemy.TakeDamage(transform.position);
        }
    }
}
