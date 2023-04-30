using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    public Vector3 startPos;

    Vector3 movement;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        enemyState = EnemyState.patrol;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyState = EnemyState.patrol;
        if (CheckDis()) enemyState = EnemyState.chase;
        Patrol(startPos);
        Chase();
        Animate();
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
        if (enemyState != EnemyState.chase) return;
        agent.SetDestination(target.position);
        FindFacingDirection();
    }

    public void Patrol(Vector3 startPos)
    {
        if (enemyState != EnemyState.patrol) return;
        enemyState = EnemyState.patrol;
        agent.SetDestination(startPos);
        FindFacingDirection();
    }

    public bool CheckDis()
    {
        if (enemyState == EnemyState.stagger) return false;
        if (Vector2.Distance(transform.position, target.position) > lookRadius) return false;
        return true;
    }

    void Animate()
    {
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetBool("IsMoving", false);
        if (agent.desiredVelocity.sqrMagnitude > 0.01f) animator.SetBool("IsMoving", true);
    }
}
