using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float health = 100.0f;
    public float speed = 10.0f;
    
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
        
    }

    protected virtual bool OnPlayerDetected()
    {
        return true;
    }
    
    protected abstract void Run();
}
