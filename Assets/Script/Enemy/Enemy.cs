using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    patrol,
    chase,
    attack,
    stagger
}

public class Enemy : MonoBehaviour
{
    public Transform target;
    public int health;
    public float lookRadius;
    public EnemyState enemyState;
    public Rigidbody2D rb;
    public Animator animator;
    public NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void TakeDamage(Vector3 weaponPos)
    {
        if (enemyState == EnemyState.stagger) return;
        StartCoroutine(TakeDmgCo(weaponPos));
    }

    public IEnumerator TakeDmgCo(Vector3 weaponPos)
    {
        enemyState = EnemyState.stagger;
        Vector2 direction = weaponPos - transform.position;
        rb.velocity = -direction * 15f;
        yield return new WaitForSeconds(0.1f);
        health--;
        if (health <= 0) Destroy(gameObject);
        rb.velocity = Vector2.zero;
        enemyState = EnemyState.patrol;
    }
}
