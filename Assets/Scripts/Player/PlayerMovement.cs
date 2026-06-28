using UnityEngine;

public enum PlayerState
{
    Idle,
    Moving,
    Jumping,
    Falling,
    Attacking
}

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    
    [SerializeField] private float maxSpeed = 8f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 10f;
    [SerializeField] private float jumpForce = 12f;

    [SerializeField] private float coyoteTime = 0.15f;
    [SerializeField] private float jumpBufferTime = 0.15f;

    private PlayerState currentState = PlayerState.Idle;
    private bool facingRight = true;
    private bool isGrounded = false;
    private bool wasGrounded = false; 

    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    private float moveInput;
    private bool jumpRequest;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        PlayerSpawnPoint playerSpawnpoint = FindObjectOfType<PlayerSpawnPoint>();
        if (playerSpawnpoint != null)
        {
            transform.position = playerSpawnpoint.transform.position;
        }

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        GatherInputs();
        UpdateTimers();
        UpdateState();
        UpdateAnimations();
        HandleSpriteFlip();
    }

    private void FixedUpdate()
    {
        wasGrounded = isGrounded;
        isGrounded = false; 

        ApplyMovement();
        ApplyJump();
    }

    private void GatherInputs()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.W))
        {
            jumpBufferCounter = jumpBufferTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && currentState != PlayerState.Attacking)
        {
            Attack();
        }
    }

    private void UpdateTimers()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        jumpBufferCounter -= Time.deltaTime;

        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            jumpRequest = true;
            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
        }
    }

    private void UpdateState()
    {
        if (currentState == PlayerState.Attacking) return;

        if (isGrounded)
        {
            currentState = (Mathf.Abs(rb.linearVelocity.x) > 0.1f) ? PlayerState.Moving : PlayerState.Idle;
        }
        else
        {
            currentState = (rb.linearVelocity.y > 0) ? PlayerState.Jumping : PlayerState.Falling;
        }
    }

    private void UpdateAnimations()
    {
        float currentSpeed = Mathf.Abs(rb.linearVelocity.x);
        animator.SetFloat("Speed", currentSpeed);
        animator.SetFloat("VerticalSpeed", rb.linearVelocity.y);
        animator.SetBool("IsGround", isGrounded);

        float animationSpeedRatio = Mathf.Max(0.1f, currentSpeed / maxSpeed);
        animator.SetFloat("RunMultiplier", animationSpeedRatio);
    }

    private void HandleSpriteFlip()
    {
        if (moveInput > 0 && !facingRight)
        {
            Flip(true);
        }
        else if (moveInput < 0 && facingRight)
        {
            Flip(false);
        }
    }

    private void Flip(bool right)
    {
        facingRight = right;
        spriteRenderer.flipX = !right;
        
        float attackPointX = right ? 0.8f : -0.8f;
        attackPoint.localPosition = new Vector3(attackPointX, 0.0f, 0.0f);
    }

    private void Attack()
    {
        currentState = PlayerState.Attacking;
        animator.SetTrigger("Attack");
    }

    public void OnAttackComplete() 
    {
        currentState = PlayerState.Idle;
    }

    private void ApplyMovement()
    {
        float targetSpeed = moveInput * maxSpeed;
        float speedDif = targetSpeed - rb.linearVelocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        
        float movementForce = speedDif * accelRate;
        rb.AddForce(movementForce * Vector2.right, ForceMode2D.Force);
    }

    private void ApplyJump()
    {
        if (jumpRequest)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpRequest = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            for (int i = 0; i < collision.contactCount; i++)
            {
                Vector2 normal = collision.GetContact(i).normal;
                
                if (normal.y > 0.5f)
                {
                    isGrounded = true;
                    
                    if (!wasGrounded)
                    {
                        wasGrounded = true; 
                    }

                    return;
                }
            }
        }
    }
}