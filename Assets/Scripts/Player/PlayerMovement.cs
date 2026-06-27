using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;

    private bool facingRight = true;

    private Rigidbody2D rb;

    private bool isGrounded = true;

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
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        float move = Input.GetAxisRaw("Horizontal");

        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));

        animator.SetBool("IsGround", isGrounded);

        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        animator.SetFloat("VerticalSpeed", rb.linearVelocity.y);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Attack");
        }
        
        if (move > 0)
        {
            spriteRenderer.flipX = false;
            facingRight = true;

            attackPoint.localPosition = new Vector3(0.8f, 0.0f, 0.0f);
        }
        else if (move < 0)
        {
            spriteRenderer.flipX = true;
            facingRight = false;

            attackPoint.localPosition = new Vector3(-0.8f, 0.0f, 0.0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Choqué con: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            Debug.Log("Player is grounded.");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}