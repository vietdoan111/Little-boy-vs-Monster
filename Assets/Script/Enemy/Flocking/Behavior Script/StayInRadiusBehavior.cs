using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/StayInRadius")]
public class StayInRadiusBehavior : FlockBehavior
{
    public Vector2 center;
    public float radius;

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        Vector2 centerOffset = center - (Vector2)agent.transform.position;
        float radiusDrag = centerOffset.magnitude / radius;

        if (radiusDrag < 0.9f) return Vector2.zero;

        return centerOffset * radiusDrag * radiusDrag;
    }
}
