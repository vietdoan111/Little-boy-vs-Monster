using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Avoidance")]
public class AvoidanceBehavior : FilterFlockBehavior
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        if (context.Count == 0) return Vector2.zero;

        //calculate average all point
        Vector2 avoidanceMove = Vector2.zero;
        int numAvoid = 0;
        List<Transform> filterContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filterContext)
        {
            if (Vector2.SqrMagnitude(item.position - agent.transform.position) < flock.SquareAvoidanceRadius)
            {
                numAvoid++;
                avoidanceMove += (Vector2)(agent.transform.position - item.position);
            }
        }
        if (numAvoid > 0) avoidanceMove /= numAvoid;

        return avoidanceMove;
    }
}
