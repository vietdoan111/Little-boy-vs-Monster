using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GreatTreantHead : GreatTreant
{
    public float lookRadius = 10f;

    public Transform target;

    public GameObject projectile;

    public float TimeBetweenShot;

    public float nextShotTime;

    public float TimeBetweenShotEnraged;

    Vector2 secondndShotDir;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        state = GreatTreantState.sleep;
        secondndShotDir.x = transform.position.x - 1;
        secondndShotDir.y = transform.position.y;
    }
    void FixedUpdate()
    {
        if (state == GreatTreantState.stagger) return;
        if (Vector2.Distance(transform.position, target.position) < lookRadius && state != GreatTreantState.enraged)
        {
            state = GreatTreantState.normal;
        }

        if (health <= 10)
        {
            state = GreatTreantState.enraged;
            if (Time.time > nextShotTime)
            {
                nextShotTime = Time.time + TimeBetweenShotEnraged;
                EnemyProjectile fire = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<EnemyProjectile>();
                fire.targetPos = target.position;
                EnemyProjectile fire2 = Instantiate(projectile, secondndShotDir, Quaternion.identity).GetComponent<EnemyProjectile>();
                fire2.targetPos = target.position;
            }
        }
        
        if (state != GreatTreantState.sleep)
        {
            state = GreatTreantState.normal;
            if (Time.time > nextShotTime)
            {
                nextShotTime = Time.time + TimeBetweenShot;
                EnemyProjectile fire = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<EnemyProjectile>();
                fire.targetPos = target.position;
            }
        }
    }

    private void OnDestroy()
    {
        if(health < 1) SceneManager.LoadScene("StartMenu");
    }

}
