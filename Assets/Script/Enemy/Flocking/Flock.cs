using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab;
    [HideInInspector]
    public List<FlockAgent> agents = new List<FlockAgent>();
    public FlockBehavior behavior;

    [Range(1, 500)]
    public int startCount = 250;
    public float agentDensity = 0.3f;

    [Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(0f, 100f)]
    public float maxSpeed = 5.0f;
    [Range(0f, 10f)]
    public float neighborRadius = 1.5f;
    [Range(0f, 5f)]
    public float avoidanceRadiusMultiplier = 0.5f;

    [HideInInspector]
    public float squareMaxSpeed;
    [HideInInspector]
    public float squareNeighborRadius;
    [HideInInspector]
    public float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

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
                Random.insideUnitCircle * startCount * agentDensity,
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
            List<Transform> context = GetNearbyObject(agent);

            //debug check neighbor
            //agent.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.blue, context.Count / 6f);

            //move
            Vector2 move = behavior.CalculateMove(agent, context, this);
            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed) move = move.normalized * maxSpeed;
            agent.Move(move);
        }
    }
    
    public List<Transform> GetNearbyObject(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextCollider = Physics2D.OverlapCircleAll(agent.transform.position, neighborRadius);
        foreach(Collider2D collider in contextCollider)
        {
            if (collider != agent.AgentCollider) context.Add(collider.transform);
        }

        return context;
    }
}
