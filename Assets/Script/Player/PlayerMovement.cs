using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Player
{
    public float moveSpeed = 5f;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public Sword sword;
    public float arrowForce = 20f;
    public VectorValue startPos;
    public ArrayValue listChest;
    public ArrayValue playerStats;


    float horizontalDir, verticalDir;
    Vector2 movement;
    Vector3 turnUp = new Vector3(0, 0, 0);
    Vector3 turnDown = new Vector3(0, 0, 180);
    Vector3 turnLeft = new Vector3(0, 0, 90);
    Vector3 turnRight = new Vector3(0, 0, -90);
    bool isShooting;
    bool isMelee;
    Vector3 firePointStart;

    private void OnEnable()
    {
        LoadGame();
        openedChests = listChest.initialValue;
        maxHealth = playerStats.initialValue[0];
        maxArrowNum = playerStats.initialValue[1];
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Vertical", 1);
        health = maxHealth;
        arrowNum = maxArrowNum;
        transform.position = startPos.initialValue;
        heathText.text = health.ToString();
        arrowText.text = arrowNum.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        movement = Vector2.zero;
        ProcessInputs();
    }

    private void FixedUpdate()
    {
        if (state == PlayerState.stagger) return;
        if (state != PlayerState.attack)
        {
            RotateWeapon();
            if (isShooting && arrowNum > 0) StartCoroutine(ShootingCo());
            if (isMelee) StartCoroutine(MeleeCo());
        }

        if (state == PlayerState.walk) Animate();
    }

    void ProcessInputs()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        isShooting = Input.GetButton("Fire1");
        isMelee = Input.GetButton("Fire2");
    }

    void RotateWeapon()
    {
        firePointStart = firePoint.position;
        sword.isUp = false;
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
                sword.isUp = true;
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
        arrowText.text = arrowNum.ToString();
        Debug.Log("arrow num when shoot: " + arrowNum);
        GameObject arrow = Instantiate(bulletPrefab, firePointStart, firePoint.rotation);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.velocity = arrowForce * firePoint.up;
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

    private IEnumerator MeleeCo()
    {
        state = PlayerState.attack;
        rb.velocity = Vector2.zero;
        sword.gameObject.SetActive(true);
        yield return new WaitForSeconds(sword.swingTime);
        sword.gameObject.SetActive(false);
        state = PlayerState.walk;
    }

    void LoadGame()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        if (data == null) return;

        startPos.defaultValue.x = data.position[0];
        startPos.defaultValue.y = data.position[1];
        startPos.initialValue = startPos.defaultValue;

        listChest.defaultValue = data.openedChests;
        listChest.initialValue = listChest.defaultValue;

        playerStats.defaultValue = data.playerStats;
        playerStats.initialValue = playerStats.defaultValue;
    }
}
