using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Targeting")]
public class TargetingBehavior : FlockBehavior
{
    Transform target;
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        if (target == null) return Vector2.zero;

        Vector2 move = target.position - agent.transform.position;
        move.Normalize();
        return move;
    }
}
