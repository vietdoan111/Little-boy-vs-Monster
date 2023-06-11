using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AgentState
{
    patrol,
    stagger,
    dead
}

[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour
{
    Collider2D agentCollider;
    public Collider2D AgentCollider { get { return agentCollider; } }
    public Animator animator;
    public Transform spriteTrans;
    public AgentState agentState;
    public Rigidbody2D rb;
    public int health;
    public float deathTime = 0.5f;

    bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        agentCollider = GetComponent<Collider2D>();
    }

    public void Move(Vector2 velocity)
    {
        if (agentState != AgentState.patrol) return;
        transform.up = velocity;
        transform.position += (Vector3)velocity * Time.deltaTime;
        spriteTrans.localEulerAngles = -transform.eulerAngles;


        if (animator != null)
        {
            animator.SetFloat("Horizontal", velocity.x);
            animator.SetFloat("Vertical", velocity.y);
            animator.SetFloat("Speed", velocity.sqrMagnitude);
        }
    }

    public void TakeDamage(Vector3 weaponPos)
    {
        if (agentState == AgentState.stagger) return;
        StartCoroutine(TakeDmgCo(weaponPos));
    }

    public IEnumerator TakeDmgCo(Vector3 weaponPos)
    {
        agentState = AgentState.stagger;
        Vector2 direction = weaponPos - transform.position;
        rb.velocity = -direction * 15f;
        yield return new WaitForSeconds(0.1f);
        health--;
        rb.velocity = Vector2.zero;
        if (health <= 0)
        {
            isDead = true;
            agentState = AgentState.dead;
            if (animator != null) animator.SetBool("IsDead", isDead);
            yield return new WaitForSeconds(deathTime);
            Destroy(gameObject);
        }
        agentState = AgentState.patrol;
    }
}
