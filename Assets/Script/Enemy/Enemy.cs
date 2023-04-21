using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    patrol,
    chase,
    attack,
    stagger
}

public class Enemy : MonoBehaviour
{
    public int health;
    public float lookRadius;
    public EnemyState enemyState;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyState = EnemyState.patrol;
    }

    public void TakeDamage(Vector3 weaponPos)
    {
        StartCoroutine(TakeDmgCo(weaponPos));
    }

    public IEnumerator TakeDmgCo(Vector3 weaponPos)
    {
        enemyState = EnemyState.stagger;
        Vector2 direction = weaponPos - transform.position;
        rb.AddForce(-direction * 15f, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.1f);
        health--;
        if (health <= 0) Destroy(gameObject);
        rb.velocity = Vector2.zero;
        enemyState = EnemyState.patrol;
    }
}
