using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float health = 100.0f;
    
    public float speed = 5.0f; 
    protected float normalSpeed;
    
    public Animator animator;
    public SpriteRenderer spriteRenderer; 

    public Transform wallCheck;
    public float wallCheckDistance = 0.5f;
    public LayerMask groundLayer;
    protected int direction = -1;

    public SpriteRenderer visualFov;
    private Color normalColor = new Color(1f, 1f, 1f, 0.5f);
    private Color alertColor = new Color(1f, 0f, 0f, 0.5f);
    
    protected bool isDead = false;
    private Color originalColor = Color.white;

    protected virtual void Start()
    {
        normalSpeed = speed;
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    protected virtual void Patrol()
    {
        transform.position = new Vector3(transform.position.x + (speed * Time.deltaTime * direction),
            transform.position.y, transform.position.z);

        if (IsHittingWall())
        {
            direction = -direction;
            Flip();
        }
    }

    protected virtual void Patrol(Transform pointA, Transform pointB, ref Transform currentTarget)
    {
        transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);

        if (currentTarget.position.x > transform.position.x && direction == -1)
        {
            direction = 1;
            Flip();
        }
        else if (currentTarget.position.x < transform.position.x && direction == 1)
        {
            direction = -1;
            Flip();
        }

        if (Mathf.Abs(currentTarget.position.x - transform.position.x) <= 0.5f)
        {
            if (currentTarget == pointA)
            {
                currentTarget = pointB;
            }
            else
            {
                currentTarget = pointA;
            }
        }
    }
    
    public void TakeDamage(int amount)
    {
        if (isDead) return;

        health -= amount;
        
        animator.SetTrigger("hit"); 
        
        StartCoroutine(DamageFlash());

        if (health <= 0)
        {
            isDead = true;
            Die();
        }
    }

    private IEnumerator DamageFlash()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.15f);
            spriteRenderer.color = originalColor; 
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isDead)
        {
            visualFov.color = alertColor;
            Attack();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isDead)
        {
            visualFov.color = normalColor;
            ReturnToNormalSpeed();
        }
    }

    protected abstract void Attack();
    protected abstract void ReturnToNormalSpeed();

    protected bool IsHittingWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, groundLayer);
        Debug.DrawRay(wallCheck.position, transform.right * wallCheckDistance, Color.red);
        return (hit.collider != null);
    }

    protected void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
    }
}