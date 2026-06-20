using System;
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

        animator.SetTrigger("walk");
    }

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        Vector2 collisionNormal = collision.GetContact(0).normal;

        if (Mathf.Abs(collisionNormal.x) > 0.5f && collision.gameObject.CompareTag("Ground"))
        {
                Debug.Log("vvvvvvvvvvvvvvvvvvvvvvv");
                direction = -direction;
        }
    }

    protected abstract void Run();
}