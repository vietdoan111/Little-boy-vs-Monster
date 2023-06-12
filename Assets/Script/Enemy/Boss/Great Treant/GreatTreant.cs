using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GreatTreantState
{
    sleep,
    normal,
    enraged,
    stop,
    retreat,
    stagger
}

public class GreatTreant : MonoBehaviour
{
    public int health;
    public GreatTreantState state;
    public Rigidbody2D rb;

    public void TakeDamage()
    {
        if (state == GreatTreantState.stagger) return;
        StartCoroutine(TakeDmgCo());
    }

    public IEnumerator TakeDmgCo()
    {
        state = GreatTreantState.stagger;
        yield return new WaitForSeconds(0.1f);
        health--;
        rb.velocity = Vector2.zero;
        if (health <= 0) Destroy(gameObject);
        state = GreatTreantState.normal;
    }
}
