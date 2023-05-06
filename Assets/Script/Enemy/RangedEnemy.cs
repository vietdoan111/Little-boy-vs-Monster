using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemy : Enemy
{
    public GameObject projectile;
    public float timeBetweenShot;
    public float attackRange;
    public float retreatRange;
    public float projectileSpeed;

    float nextShotTime;

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
        CheckState();
        Patrol(nextPatrolPos);
        Chase();
        ShootFire();
        Retreat();
        Animate();
    }

    void Animate()
    {
        isMoving = false;
        if (agent.desiredVelocity.sqrMagnitude > 0.01f) isMoving = true;
        if (health > 0) return;
        animator.SetBool("IsDead", isDead);
    }

    void ShootFire()
    {
        if (enemyState != EnemyState.attack) return;
        agent.isStopped = true;
        if (Time.time > nextShotTime)
        {
            nextShotTime = Time.time + timeBetweenShot;
            MoveProjectile();
        }
    }

    void MoveProjectile()
    {
        GameObject fire = Instantiate(projectile, transform.position, Quaternion.identity);
        EnemyProjectile prjTile = fire.GetComponent<EnemyProjectile>();
        prjTile.targetPos = target.position;
    }

    public void Retreat()
    {
        if (enemyState != EnemyState.retreat) return;
        agent.isStopped = true;
        transform.position = Vector2.MoveTowards(transform.position, target.position, -2.0f * Time.deltaTime);

    }

    void CheckState()
    {
        if (CheckDis(lookRadius)) enemyState = EnemyState.chase;
        if (CheckDis(attackRange)) enemyState = EnemyState.attack;
        if (CheckDis(retreatRange)) enemyState = EnemyState.retreat;
    }
}
