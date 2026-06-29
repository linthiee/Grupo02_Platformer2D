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

    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float jumpForce;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    [SerializeField] private float coyoteTime;
    [SerializeField] private float jumpBufferTime;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip swordSwing;
    
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    
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
        PlayerSpawnPoint playerSpawnpoint = FindFirstObjectByType<PlayerSpawnPoint>();

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
        
        CheckGrounded(); 

        ApplyMovement();
        ApplyJump();
        ApplyJumpPhysics();
    }

    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
    
    private void GatherInputs()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && currentState != PlayerState.Attacking)
        {
            Debug.Log($"{this.currentState}");
            HandleAttackInput();
        }
    }

    private void ApplyJumpPhysics()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
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
        if (currentState == PlayerState.Attacking)
        {
            if (!isGrounded && rb.linearVelocity.y < -0.1f)
            {
                OnAttackComplete(); 
            }
            return;
        }

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

    private void HandleAttackInput()
    {
        currentState = PlayerState.Attacking;
        animator.SetTrigger("Attack");
        GetComponent<AudioSource>().PlayOneShot(swordSwing);

        Debug.Log($"{this.currentState}");
    }

    public void OnAttackComplete()
    {
        currentState = PlayerState.Idle;
        Debug.Log($"{this.currentState}");
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
}