using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    walk,
    attack,
    stagger
}

public class Player : MonoBehaviour
{
    public int health;
    public int arrowNum = 5;
    public Animator animator;
    public Rigidbody2D rb;
    public PlayerState state;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Vertical", 1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Arrow"))
        {
            Debug.Log("Pick up arrow");
            Destroy(collision.gameObject);
            arrowNum++;
        }

        if (collision.collider.CompareTag("Enemy"))
        {
            if (state == PlayerState.stagger) return;
            Vector3 enemyWeapPos = collision.transform.position;
            StartCoroutine(PlayerTakeDmgCo(enemyWeapPos));
        }
    }

    public IEnumerator PlayerTakeDmgCo(Vector3 enemyWeapPos)
    {
        state = PlayerState.stagger;
        Vector2 direction = enemyWeapPos - transform.position;
        rb.velocity = -direction * 15f;
        yield return new WaitForSeconds(0.1f);
        health--;
        if (health <= 0) Debug.Log("Lose Game");
        rb.velocity = Vector2.zero;
        state = PlayerState.walk;
    }
}
