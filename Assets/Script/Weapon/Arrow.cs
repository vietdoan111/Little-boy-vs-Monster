using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void FixedUpdate()
    {
        waitTime += Time.fixedDeltaTime;
        if (waitTime > maxFlyingTime && waitTime < maxAppearingTime)
        {
            Debug.Log("arrow grounded");
            rb.velocity = Vector2.zero;
            state = ArrowState.grounded;
        }

        if (waitTime > maxAppearingTime)
        {
            Destroy(gameObject);
            player.AddArrow();
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

        if (collision.collider.CompareTag("Arrow"))
        {
            if (state == ArrowState.grounded) return;
            Debug.Log("arrow grounded");
            rb.velocity = Vector2.zero;
            state = ArrowState.grounded;
            Destroy(gameObject);
            player.AddArrow();
            Debug.Log("arrow num: " + player.arrowNum);
        }

        if (collision.collider.CompareTag("Fire"))
        {
            Destroy(collision.gameObject);
        }

        if (collision.collider.CompareTag("Flock"))
        {
            if (state == ArrowState.grounded) return;
            rb.velocity = Vector2.zero;
            state = ArrowState.grounded;
            FlockAgent agent = collision.collider.GetComponent<FlockAgent>();
            agent.TakeDamage(transform.position);
        }

        if (collision.collider.CompareTag("FlockHead"))
        {
            if (state == ArrowState.grounded) return;
            rb.velocity = Vector2.zero;
            state = ArrowState.grounded;
            Monument monument = collision.collider.GetComponent<Monument>();
            monument.TakeDamage();
        }

        if (collision.collider.CompareTag("Boss"))
        {
            if (state == ArrowState.grounded) return;
            rb.velocity = Vector2.zero;
            state = ArrowState.grounded;
            GreatTreant GreatTreant = collision.collider.GetComponent<GreatTreant>();
            GreatTreant.TakeDamage();
        }
    }
}
