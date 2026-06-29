using UnityEngine;

public class Boar : Enemy
{
    [SerializeField] private float runSpeedBonus = 5.0f;
    
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip boarChase;
    
    private Transform currentTarget;
    
    private bool isAttacking = false;
    public int touchDamage = 1;

    protected override void Start()
    {
        base.Start();
        
        if (pointA != null) 
        {
            currentTarget = pointA;
        }
    }

    private void FixedUpdate()
    {
        if (pointA != null && pointB != null)
        {
            Patrol(pointA, pointB, ref currentTarget);
        }
        else 
        {
            Patrol(); 
        }

        if (!isAttacking)
            GetComponent<AudioSource>().Stop();
        
        UpdateAnimations();
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isDead)
        {
            PlayerHealt playerHealth = collision.gameObject.GetComponent<PlayerHealt>();
            
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(touchDamage);
            }
        }
    }
    
    private void UpdateAnimations()
    {
        animator.SetFloat("Speed", speed);
    }

    protected override void Attack()
    {
        if (!isAttacking)
        {
            speed = normalSpeed + runSpeedBonus;
            GetComponent<AudioSource>().PlayOneShot(boarChase);
            isAttacking = true;
        }
    }

    protected override void ReturnToNormalSpeed()
    {
        speed = normalSpeed;
        isAttacking = false;
    }
}