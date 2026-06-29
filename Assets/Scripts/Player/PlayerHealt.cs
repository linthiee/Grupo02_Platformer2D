using System.Collections;
using UnityEngine;

public class PlayerHealt : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    public float iFramesDuration = 1.5f;
    private bool isInvulnerable = false;

    public SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Color flashColor = new Color(1f, 1f, 1f, 0.7f);
    
    [SerializeField] private Transform PlayerSpawnPoint;

    private Animator animator;

    private IEventBus eventBus;

    private void Awake()
    {
        eventBus = ServiceLoader.GetService<IEventBus>();
    }
    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        PlayerSpawnPoint point = FindFirstObjectByType<PlayerSpawnPoint>();

        if (point != null)
        {
            PlayerSpawnPoint = point.transform;
        }
        if (spriteRenderer != null) 
        {
            originalColor = spriteRenderer.color;
        }
    }
    
    public void TakeDamage(int damage)
    {
        if (isInvulnerable) 
            return;

        currentHealth -= damage;
        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvulnerabilityRoutine());
        }
    }
    
    private IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;
        float elapsed = 0f;
        bool isFlashing = false;
        
        while (elapsed < iFramesDuration)
        {
            spriteRenderer.color = isFlashing ? originalColor : flashColor;
            isFlashing = !isFlashing;
            
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }

        spriteRenderer.color = originalColor;
        isInvulnerable = false;
    }
    
    public void Die()
    {
        animator.SetBool("Die", true);
       GetComponent<PlayerMovement>().enabled = false;

        eventBus.Publish(new EventBus.PlayerDeathEvent());

        animator.SetTrigger("Die");

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2f);
        currentHealth = maxHealth;
        transform.position = PlayerSpawnPoint.position;
        animator.SetBool("Die", false);
        GetComponent<PlayerMovement>().enabled = true;
        animator.Play("PlayerIdle");
    }   
}
