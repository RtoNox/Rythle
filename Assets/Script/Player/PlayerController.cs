using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Horizontal Movement Settings")]
    private Rigidbody2D rb;
    [SerializeField] private float walkSpeed = 1;
    private float xAxis;

    [Header("Ground Check Settings")]
    [SerializeField] private float jumpForce = 45;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckY = 0.2f;
    [SerializeField] private float groundCheckX = 0.5f;
    [SerializeField] private LayerMask whatisGround;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        getInputs();
        jump();
    }

    void FixedUpdate()
    {
        move();
    }

    void getInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
    }

    private void move()
    {
        rb.velocity = new Vector2(walkSpeed * xAxis, rb.velocity.y);
    }

    public bool Grounded()
    {
        // More efficient ground check
        return Physics2D.BoxCast(groundCheckPoint.position, 
                               new Vector2(groundCheckX, groundCheckY), 
                               0f, Vector2.down, 0f, whatisGround);
    }

    void jump()
    {
        if (Input.GetButtonDown("Jump") && Grounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}