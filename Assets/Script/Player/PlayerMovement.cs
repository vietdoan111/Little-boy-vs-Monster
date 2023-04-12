using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    walk,
    attack
}

public class PlayerMovement : Player
{
    public PlayerState state;
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject sword;
    public float arrowForce = 20f;


    float horizontalDir, verticalDir;
    Vector2 movement;
    Vector3 turnUp = new Vector3(0, 0, 0);
    Vector3 turnDown = new Vector3(0, 0, 180);
    Vector3 turnLeft = new Vector3(0, 0, 90);
    Vector3 turnRight = new Vector3(0, 0, -90);
    bool isShooting;
    bool isMelee;
    Vector3 firePointStart;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
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
        if (state != PlayerState.attack)
        {
            if (isShooting && arrowNum > 0) StartCoroutine(ShootingCo());
            if (isMelee) sword.SetActive(true);
        }
        
        if (state == PlayerState.walk)
        {
            Animate();
        }
    }

    void ProcessInputs()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        isShooting = Input.GetButton("Fire1");
        isMelee = Input.GetButton("Fire2");
        if (isShooting || isMelee) RotateWeapon();
    }

    void RotateWeapon()
    {
        firePointStart = firePoint.position;
        if (horizontalDir > 0.5)
        {
            sword.transform.eulerAngles = turnRight;
            firePoint.eulerAngles = turnRight;
            firePointStart.x += 1.5f;
        }
        else if (horizontalDir < -0.5)
        {
            sword.transform.eulerAngles = turnLeft;
            firePoint.eulerAngles = turnLeft;
            firePointStart.x -= 1.5f;
        }
        else
        {
            if (verticalDir > 0.5)
            {
                sword.transform.eulerAngles = turnUp;
                firePoint.eulerAngles = turnUp;
                firePointStart.y += 2.5f;
            }
            else if (verticalDir < -0.5)
            {
                sword.transform.eulerAngles = turnDown;
                firePoint.eulerAngles = turnDown;
                firePointStart.y -= 2.0f;
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
        arrowNum--;
        Debug.Log("arrow num when shoot: " + arrowNum);
        GameObject arrow = Instantiate(bulletPrefab, firePointStart, firePoint.rotation);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * arrowForce, ForceMode2D.Impulse);
    }

    private IEnumerator ShootingCo()
    {
        animator.SetBool("Shoot", true);
        state = PlayerState.attack;
        yield return new WaitForSeconds(0.25f);
        Shoot();
        animator.SetBool("Shoot", false);
        state = PlayerState.walk;
    }
}
