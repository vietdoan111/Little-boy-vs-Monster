using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : Enemy
{
    Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        nextPatrolPos = startPos;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        enemyState = EnemyState.patrol;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        animator = GetComponent<Animator>();
        navMeshPath = new NavMeshPath();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyState == EnemyState.stagger) return;
        enemyState = EnemyState.patrol;
        if (!isMoving) waitTime += Time.deltaTime;
        if (CheckDis(lookRadius)) enemyState = EnemyState.chase;
        Patrol(nextPatrolPos);
        Chase();
        Animate();
    }

    void Animate()
    {
        if (!movement.Equals(Vector2.zero)) direction = movement; 
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        isMoving = false;
        if (agent.desiredVelocity.sqrMagnitude > 0.01f) isMoving = true;
        animator.SetBool("IsMoving", isMoving);
    }
}
