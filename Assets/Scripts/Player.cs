using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Runnable
{

    [Range(1.0f, 5.0f)] public float baseGravity = 1.0f;
    [Range(1.0f, 5.0f)] public float fallGravityMultiplier = 1.0f;
    [Range(1.0f, 5.0f)] public float shortJumpGravityMultiplier = 1.0f;
    [Range(1.0f, 5.0f)] public float stompGravityMultiplier = 1.0f;
    public float jumpVelocity = 0.0f;
    public float maxYVelocity = 0.0f;
    public GameManager gameManager = null;

    private float gravity = 1.0f;
    private float yVelocity = 0.0f;
    private bool jumpPressed = false;
    private bool stompPressed = false;
    private bool _isGrounded = false;
    private bool IsGrounded
    {
        get => _isGrounded;
        set
        {
            _isGrounded = value;
            GetComponent<Animator>().SetBool("IsGrounded", value);
        }
    }

    protected override void RunComponent()
    {
        GetComponent<Animator>().SetBool("GameRunning", true);
    }

    protected override void StopComponent()
    {
        GetComponent<Animator>().SetBool("GameRunning", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameRunning)
        {
            return;
        }
        if (Input.GetButtonDown("Jump") && IsGrounded)
        {
            jump();
            jumpPressed = true;
        }
        if (Input.GetButtonUp("Jump"))
        {
            jumpPressed = false;
        }
        if (Input.GetAxisRaw("Vertical") < 0.0f)
        {
            stompPressed = true;
        } else
        {
            stompPressed = false;
        }
    }

    private void FixedUpdate()
    {
        if (!GameRunning)
        {
            return;
        }
        setGravity();
        yVelocity += Mathf.Clamp(gravity * Physics2D.gravity.y * Time.fixedDeltaTime, -maxYVelocity, maxYVelocity);
        Rigidbody2D rb2D = GetComponent<Rigidbody2D>();
        Vector2 newPosition = rb2D.position + (Vector2.up * yVelocity * Time.fixedDeltaTime);
        if (newPosition.y <= 0.0f)
        {
            newPosition.y = 0.0f;
            yVelocity = 0.0f;
            IsGrounded = true;
        }
        else
        {
            IsGrounded = false;
        }
        rb2D.MovePosition(newPosition);
    }

    private void setGravity()
    {
        if (yVelocity > 0.0f)
        {
            if (jumpPressed)
            {
                gravity = baseGravity;
            } else
            {
                gravity = baseGravity * shortJumpGravityMultiplier;
            }
        } else if (stompPressed)
        {
            gravity = baseGravity * stompGravityMultiplier;
        } else
        {
            gravity = baseGravity * fallGravityMultiplier;
        }
    }

    private void jump()
    {
        yVelocity = jumpVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            gameManager.GameOver();
        }
    }
}
