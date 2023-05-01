using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : Enemy
{
    public Vector3 nextPatrolPos;
    public float maxPatrolDis;
    public float maxWaitTime;

    float waitTime;
    Vector2 startPos;
    Vector3 movement;
    bool isMoving = false;
    NavMeshPath navMeshPath;

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
        if (CheckDis()) enemyState = EnemyState.chase;
        Patrol(nextPatrolPos);
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

    public void Patrol(Vector3 desiredPos)
    {
        if (enemyState != EnemyState.patrol) return;
        FindNextPatrolSpot();
        if (agent.CalculatePath(desiredPos, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
        {
            agent.SetPath(navMeshPath);
        }
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
        isMoving = false;
        if (agent.desiredVelocity.sqrMagnitude > 0.01f) isMoving = true;
        animator.SetBool("IsMoving", isMoving);
    }

    void FindNextPatrolSpot()
    {
        if (waitTime < maxWaitTime) return;
        nextPatrolPos.x = startPos.x + Random.Range(-maxPatrolDis, maxPatrolDis);
        nextPatrolPos.y = startPos.y + Random.Range(-maxPatrolDis, maxPatrolDis);
        waitTime = 0.0f;
    }
}
