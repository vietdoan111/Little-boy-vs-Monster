using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonumnentState
{
    idle,
    stagger,
    attack
}

public class Monument : Flock
{
    public int spawnNum;
    public int maxSpawnNum;
    public float timeBetweenSpawn = 10.0f;
    float nextSpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        for (int i = 0; i < startCount; i++)
        {
            FlockAgent agent = Instantiate(
                agentPrefab,
                (Vector2)transform.position + Random.insideUnitCircle * startCount * agentDensity,
                Quaternion.identity,
                transform
                );

            agents.Add(agent);
        }
    }

    // Update is called once per frame
    void Update()
    {
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

                agents.Add(agent);
            }
        }
    }
}
