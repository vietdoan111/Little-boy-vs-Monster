using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonumentState
{
    idle,
    stagger,
    attack
}

public class Monument : Flock
{
    public int health;
    public int spawnNum;
    public int maxSpawnNum;
    public float timeBetweenSpawn = 10.0f;
    public float lookRadius;
    public Transform target;
    public FlockBehavior[] behaviors;
    public MonumentState state;
    public Rigidbody2D rb;

    float nextSpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;
        target = GameObject.FindGameObjectWithTag("Player").transform;

        for (int i = 0; i < startCount; i++)
        {
            FlockAgent agent = Instantiate(
                agentPrefab,
                (Vector2)transform.position + Random.insideUnitCircle * startCount * agentDensity,
                Quaternion.identity,
                transform
                );
            agent.Initialized(this);
            agents.Add(agent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = Vector2.zero;

        foreach (FlockAgent agent in agents)
        {
            if (agent == null)
            {
                continue;
            }
            List<Transform> context = GetNearbyObject(agent);

            //move
            Vector2 move = behavior.CalculateMove(agent, context, this);
            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed) move = move.normalized * maxSpeed;
            agent.Move(move);
        }

        if (state != MonumentState.attack) return;
        //spawn new
        if (Time.time > nextSpawnTime)
        {
            nextSpawnTime = Time.time + timeBetweenSpawn;
            for (int i = 0; i < spawnNum; i++)
            {
                if (agents.Count > maxSpawnNum) break;
                FlockAgent agent = Instantiate(
                    agentPrefab,
                    (Vector2)transform.position + Random.insideUnitCircle * spawnNum * agentDensity,
                    Quaternion.identity,
                    transform
                    );
                agent.Initialized(this);
                agents.Add(agent);
            }
        }
    }

    public void TakeDamage()
    {
        if (state == MonumentState.stagger) return;
        StartCoroutine(TakeDmgCo());
    }

    public IEnumerator TakeDmgCo()
    {
        state = MonumentState.stagger;
        yield return new WaitForSeconds(0.1f);
        health--;
        rb.velocity = Vector2.zero;
        if (health <= 0) Destroy(gameObject);
        state = MonumentState.attack;
    }
}
