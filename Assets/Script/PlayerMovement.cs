using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    walk,
    shootArrow
}

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    public bool isShooting;
    public Transform FirePoint;
    public GameObject bulletPrefab;
    public float arrowForce = 20f;


    float HorizontalDir, VerticalDir;
    Vector2 movement;
    Vector3 shootUp = new Vector3(0, 0, 0);
    Vector3 shootDown = new Vector3(0, 0, 180);
    Vector3 shootLeft = new Vector3(0, 0, 90);
    Vector3 shootRight = new Vector3(0, 0, -90);

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Vertical", -1);
    }

    // Update is called once per frame
    void Update()
    {
        movement = Vector2.zero;
        ProcessInputs();
    }

    private void FixedUpdate()
    {
        Animate();
    }

    void ProcessInputs()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }
    void Animate()
    {
        if (movement != Vector2.zero)
        {
            Move();
            HorizontalDir = movement.x;
            VerticalDir = movement.y;
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    void Move()
    {
        movement.Normalize(); // ?i chéo s? ko b? nhanh h?n d?c và ngang
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
