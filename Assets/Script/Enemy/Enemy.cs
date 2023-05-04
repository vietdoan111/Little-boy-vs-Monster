using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    patrol,
    chase,
    attack,
    retreat,
    dodge,
    stagger,
    dead
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
    public Vector3 nextPatrolPos;
    public float maxPatrolDis;
    public float maxWaitTime;
    public NavMeshPath navMeshPath;
    public float waitTime;
    public Vector2 startPos;
    public Vector3 movement;
    public bool isMoving = false;
    public float deathTime;
    public bool isDead;

    public void TakeDamage(Vector3 weaponPos)
    {
        if (enemyState == EnemyState.stagger) return;
        StartCoroutine(TakeDmgCo(weaponPos));
    }

    public IEnumerator TakeDmgCo(Vector3 weaponPos)
    {
        enemyState = EnemyState.stagger;
        agent.isStopped = true;
        Vector2 direction = weaponPos - transform.position;
        rb.velocity = -direction * 15f;
        yield return new WaitForSeconds(0.1f);
        health--;
        rb.velocity = Vector2.zero;
        if (health <= 0)
        {
            isDead = true;
            enemyState = EnemyState.dead;
            yield return new WaitForSeconds(deathTime);
            Destroy(gameObject);
        }
        else agent.isStopped = false;
        enemyState = EnemyState.patrol;
    }

    public void FindFacingDirection()
    {
        Vector3 normalizedMovement = agent.desiredVelocity.normalized;
        Vector3 upVector = Vector3.Project(normalizedMovement, transform.up);
        Vector3 rightVector = Vector3.Project(normalizedMovement, transform.right);

        movement.y = upVector.magnitude * Vector3.Dot(upVector, transform.up);
        movement.x = rightVector.magnitude * Vector3.Dot(rightVector, transform.right);
    }

    public void Chase()
    {
        if (enemyState == EnemyState.dead) return;
        if (enemyState != EnemyState.chase) return;
        agent.isStopped = false;
        agent.SetDestination(target.position);
        FindFacingDirection();
    }

    public void Patrol(Vector3 desiredPos)
    {
        if (enemyState == EnemyState.dead) return;
        if (enemyState != EnemyState.patrol) return;
        agent.isStopped = false;
        FindNextPatrolSpot();
        if (agent.CalculatePath(desiredPos, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
        {
            agent.SetPath(navMeshPath);
        }
        FindFacingDirection();
    }

    public bool CheckDis(float actionRange)
    {
        if (enemyState == EnemyState.stagger) return false;
        if (Vector2.Distance(transform.position, target.position) > actionRange) return false;
        return true;
    }

    void FindNextPatrolSpot()
    {
        if (waitTime < maxWaitTime) return;
        nextPatrolPos.x = startPos.x + Random.Range(-maxPatrolDis, maxPatrolDis);
        nextPatrolPos.y = startPos.y + Random.Range(-maxPatrolDis, maxPatrolDis);
        waitTime = 0.0f;
    }
}
