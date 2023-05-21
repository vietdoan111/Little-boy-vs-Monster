using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour
{
    Collider2D agentCollider;
    Transform target;
    public Collider2D AgentCollider { get { return agentCollider; } }

    // Start is called before the first frame update
    void Start()
    {
        agentCollider = GetComponent<Collider2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Move(Vector2 velocity)
    {
        transform.up = velocity;
        Vector3 direction = target.position - transform.position;
        transform.Translate(((Vector3)velocity + direction.normalized) * Time.deltaTime);
    }
}
