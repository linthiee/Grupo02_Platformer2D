using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float health = 100.0f;
    public float speed = 100.0f;

    public Animator animator;

    private int direction = -1;

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    protected abstract void Attack();

    protected virtual void Patrol()
    {
        transform.position = new Vector3(transform.position.x + (speed * Time.deltaTime * direction),
            transform.position.y, transform.position.z);

        Debug.Log("patroling");
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
            Debug.Log("changing target");
        }
        animator.SetTrigger("walk");

        Debug.Log($"current target: {currentTarget.position.x}, position: {transform.position.x}, distance: {Mathf.Abs(currentTarget.position.x - transform.position.x)}");
    }

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("collision");

        Vector2 collisionNormal = collision.GetContact(0).normal;

        if (Mathf.Abs(collisionNormal.x) > 0.5f && collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("vvvvvvvvvvvvvvvvvvvvvvv");
            direction = -direction;
            Flip();
        }
    }

    protected abstract void Run();

    private void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
    }
}
