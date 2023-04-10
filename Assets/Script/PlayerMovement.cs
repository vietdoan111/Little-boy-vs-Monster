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
    public PlayerState state;
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float arrowForce = 20f;


    float horizontalDir, verticalDir;
    Vector2 movement;
    Vector3 shootUp = new Vector3(0, 0, 0);
    Vector3 shootDown = new Vector3(0, 0, 180);
    Vector3 shootLeft = new Vector3(0, 0, 90);
    Vector3 shootRight = new Vector3(0, 0, -90);
    bool isShooting;
    Vector3 firePointStart;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Vertical", 1);
    }

    // Update is called once per frame
    void Update()
    {
        movement = Vector2.zero;
        ProcessInputs();
    }

    private void FixedUpdate()
    {
        if (isShooting && state != PlayerState.shootArrow)
        {
            StartCoroutine(ShootingCo());
        }
        else if (state == PlayerState.walk)
        {
            Animate();
        }
    }

    void ProcessInputs()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        isShooting = Input.GetButton("Fire1");
        if (isShooting) RotateArrow();
    }

    void RotateArrow()
    {
        firePointStart = firePoint.position;
        if (verticalDir > 0.5)
        {
            firePoint.eulerAngles = shootUp;
            firePointStart.y += 1.5f;
        }
        else if (verticalDir < -0.5)
        {
            firePoint.eulerAngles = shootDown;
            firePointStart.y--;
        }
        else if (verticalDir == 0)
        {
            if (horizontalDir > 0.5)
            {
                firePoint.eulerAngles = shootRight;
                firePointStart.x++;
            }
            else if (horizontalDir < -0.5)
            {
                firePoint.eulerAngles = shootLeft;
                firePointStart.x--;
            }
        }
    }

    void Animate()
    {
        if (movement != Vector2.zero)
        {
            Move();
            horizontalDir = movement.x;
            verticalDir = movement.y;
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    void Move()
    {
        movement.Normalize();
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void Shoot()
    {
        GameObject arrow = Instantiate(bulletPrefab, firePointStart, firePoint.rotation);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * arrowForce, ForceMode2D.Impulse);
    }

    private IEnumerator ShootingCo()
    {
        animator.SetBool("Shoot", true);
        state = PlayerState.shootArrow;
        yield return new WaitForSeconds(0.25f);
        Shoot();
        animator.SetBool("Shoot", false);
        state = PlayerState.walk;
    }
}
