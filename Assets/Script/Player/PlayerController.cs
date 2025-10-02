using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Horizontal Movement Settings")]
    private Rigidbody2D rb;
    [SerializeField] private float walkSpeed = 1;
    [SerializeField] private float jumpPeakSpeedMultiplier = 1.5f;
    [SerializeField] private float speedTransitionSmoothness = 5f;
    private float xAxis;
    private float currentSpeed;
    private int facingDirection = 1;

    [Header("Ground Check Settings")]
    [SerializeField] private float jumpForce = 45;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckY = 0.2f;
    [SerializeField] private float groundCheckX = 0.5f;
    [SerializeField] private LayerMask whatisGround;
    
    [Header("Jump Settings")]
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;
    [SerializeField] private float maxFallSpeed = 20f;
    [SerializeField] private float jumpPeakThreshold = 0.5f;
    
    [Header("Ability Settings")]
    [SerializeField] private float dashSpeed = 25f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float diveForce = 30f;
    [SerializeField] private float wallStickTime = 0.5f;
    [SerializeField] private float wallCheckDistance = 0.6f;
    
    [Header("Water Settings")]
    [SerializeField] private float waterSurfaceSpeed = 3f;
    [SerializeField] private float underwaterSpeed = 2f;
    [SerializeField] private float buoyancyForce = 5f;
    [SerializeField] private float waterDrag = 2f;
    [SerializeField] private LayerMask whatIsWater;
    
    [Header("Layer Settings")]
    [SerializeField] private LayerMask whatIsWall;
    
    // Ability state variables
    private bool isDashing = false;
    private bool isDiving = false;
    private bool isWallSticking = false;
    private bool canDoubleJump = true;
    private bool hasDoubleJumped = false;
    private bool isInWater = false;
    private bool isOnWaterSurface = false;
    private float dashTimeLeft = 0f;
    private float wallStickTimer = 0f;
    
    public static PlayerController Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = walkSpeed;
        facingDirection = (int)transform.localScale.x;
    }

    void Update()
    {
        getInputs();
        CheckWater();
        CheckWall();
        HandleAbilities();
        jump();
        flip();
    }

    void FixedUpdate()
    {
        if (!isDashing && !isWallSticking)
        {
            move();
        }
        applyJumpPhysics();
        clampFallSpeed();
        HandleDash();
        HandleDive();
        HandleWaterPhysics();
    }

    void getInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
    }

    void flip()
    {
        if (xAxis < 0 && !isDiving)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
            facingDirection = -1;
        }
        else if (xAxis > 0 && !isDiving)
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
            facingDirection = 1;
        }
    }

    private void move()
    {
        if (isDiving && isOnWaterSurface)
        {
            // Linear movement on water surface
            rb.velocity = new Vector2(xAxis * waterSurfaceSpeed, 0);
            return;
        }
        
        float targetSpeed = walkSpeed;
        
        if (!Grounded() && Mathf.Abs(rb.velocity.y) < jumpPeakThreshold && !isInWater)
        {
            targetSpeed = walkSpeed * jumpPeakSpeedMultiplier;
        }
        
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, speedTransitionSmoothness * Time.fixedDeltaTime);
        rb.velocity = new Vector2(currentSpeed * xAxis, rb.velocity.y);
    }

    public bool Grounded()
    {
        return Physics2D.BoxCast(groundCheckPoint.position, 
                               new Vector2(groundCheckX, groundCheckY), 
                               0f, Vector2.down, 0f, whatisGround);
    }

    void CheckWater()
    {
        // Check if player is in water using the water layer mask
        RaycastHit2D waterCheck = Physics2D.BoxCast(transform.position, 
            new Vector2(0.8f, 0.8f), 0f, Vector2.zero, 0f, whatIsWater);
            
        isInWater = waterCheck.collider != null;
        
        if (isInWater)
        {
            float waterTop = waterCheck.collider.bounds.max.y;
            isOnWaterSurface = transform.position.y >= waterTop - 0.3f;
        }
        else
        {
            isOnWaterSurface = false;
            // Exit diving state when leaving water
            if (isDiving)
            {
                isDiving = false;
            }
        }
    }

    void jump()
    {
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0 && !isInWater)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        
        if (Input.GetButtonDown("Jump"))
        {
            if (isOnWaterSurface && isDiving)
            {
                // Jump out of water from diving state
                isDiving = false;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * 0.7f);
            }
            else if (Grounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                canDoubleJump = true;
                hasDoubleJumped = false;
                isDiving = false;
            }
            else if (canDoubleJump && AbilityManager.Instance.IsAbilityUnlocked("DoubleJump") && !hasDoubleJumped && !isInWater)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * 0.8f);
                canDoubleJump = false;
                hasDoubleJumped = true;
                isDiving = false;
            }
            else if (isWallSticking && AbilityManager.Instance.IsAbilityUnlocked("WallStick"))
            {
                
                rb.velocity = new Vector2(-1 * facingDirection * walkSpeed * 1.2f, jumpForce);
                isWallSticking = false;
                wallStickTimer = 0f;
                isDiving = false;
            }
            else if (isInWater && !isOnWaterSurface)
            {
                // Swim upward in water
                rb.velocity = new Vector2(rb.velocity.x, underwaterSpeed);
            }
        }
    }
    
    void CheckWall()
    {
        if (isInWater) return;
        
        RaycastHit2D wallHit = Physics2D.Raycast(transform.position, 
            Vector2.right * facingDirection, wallCheckDistance, whatIsWall);
            
        if (wallHit.collider != null && !Grounded() && AbilityManager.Instance.IsAbilityUnlocked("WallStick"))
        {
            if (wallStickTimer < wallStickTime)
            {
                isWallSticking = true;
                wallStickTimer += Time.deltaTime;
                rb.velocity = new Vector2(0, Mathf.Clamp(rb.velocity.y, -2f, 0f));
            }
            else
            {
                isWallSticking = false;
            }
        }
        else
        {
            isWallSticking = false;
            wallStickTimer = 0f;
        }
    }
    
    void HandleAbilities()
    {
        // Dash input - only outside water
        if (Input.GetKeyDown(KeyCode.LeftShift) && AbilityManager.Instance.IsAbilityUnlocked("Dash") && !isInWater)
        {
            TryDash();
        }
        
        // Dive input - only allowed when not grounded and ability unlocked
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && 
            AbilityManager.Instance.IsAbilityUnlocked("Diving") && !Grounded())
        {
            TryDive();
        }
        
        // Cancel dive when hitting ground
        if (isDiving && Grounded())
        {
            isDiving = false;
        }
    }
    
    void TryDash()
    {
        if (!isDashing && (Grounded() || Mathf.Abs(rb.velocity.x) > 0.1f))
        {
            isDashing = true;
            dashTimeLeft = dashDuration;
        }
    }
    
    void HandleDash()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                rb.velocity = new Vector2(dashSpeed * facingDirection, 0);
                dashTimeLeft -= Time.fixedDeltaTime;
            }
            else
            {
                isDashing = false;
            }
        }
    }
    
    void TryDive()
    {
        // Only allow diving into water from air, or diving within water
        if (!isDiving && !Grounded() && !isInWater)
        {
            // Check if we're about to enter water
            RaycastHit2D waterBelow = Physics2D.Raycast(transform.position, Vector2.down, 2f, whatIsWater);
            if (waterBelow.collider != null)
            {
                isDiving = true;
                rb.velocity = new Vector2(0, -diveForce);
            }
        }
        else if (isInWater && !isOnWaterSurface)
        {
            // Dive downward in water
            rb.velocity = new Vector2(0, -underwaterSpeed);
        }
    }
    
    void HandleDive()
    {
        // Only allow diving in water or when entering water
        if (isDiving && !isInWater)
        {
            // Check if we're still above water
            RaycastHit2D waterBelow = Physics2D.Raycast(transform.position, Vector2.down, 1f, whatIsWater);
            if (waterBelow.collider == null && Grounded())
            {
                isDiving = false;
            }
        }
    }
    
    void HandleWaterPhysics()
    {
        if (isInWater)
        {
            // Apply water drag
            rb.drag = waterDrag;
            
            // Apply buoyancy when not diving
            if (!isDiving && rb.velocity.y < 0)
            {
                rb.AddForce(Vector2.up * buoyancyForce);
            }
            
            // Allow WASD movement underwater
            if (!isOnWaterSurface && !isDiving)
            {
                rb.velocity = new Vector2(xAxis * underwaterSpeed, rb.velocity.y);
            }
            
            // Allow swimming up/down with W/S keys
            if (!isOnWaterSurface)
            {
                float verticalInput = 0f;
                if (Input.GetKey(KeyCode.W)) verticalInput = 1f;
                if (Input.GetKey(KeyCode.S)) verticalInput = -1f;
                
                if (verticalInput != 0 && !isDiving)
                {
                    rb.velocity = new Vector2(rb.velocity.x, verticalInput * underwaterSpeed);
                }
            }
        }
        else
        {
            rb.drag = 0;
        }
    }
    
    void applyJumpPhysics()
    {
        if (isWallSticking || isInWater) return;
        
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }
    
    void clampFallSpeed()
    {
        if (rb.velocity.y < -maxFallSpeed && !isInWater)
        {
            rb.velocity = new Vector2(rb.velocity.x, -maxFallSpeed);
        }
    }
    
    // Visual debug
    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawLine(transform.position, transform.position + (Vector3.right * facingDirection * wallCheckDistance));
        
    //     if (isInWater)
    //     {
    //         Gizmos.color = Color.blue;
    //         Gizmos.DrawWireCube(transform.position, new Vector3(0.8f, 0.8f, 0));
    //     }
    // }
}