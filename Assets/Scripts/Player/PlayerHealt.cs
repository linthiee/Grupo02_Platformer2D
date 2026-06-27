using System.Collections;
using UnityEngine;

public class PlayerHealt : MonoBehaviour
{
    [SerializeField] private int maxHealth = 2;
    [SerializeField] private Transform PlayerSpawnPoint;

    private int currentHealth;

    private Animator animator;

    private IEventBus eventBus;

    private void Awake()
    {
        eventBus = ServiceLoader.GetService<IEventBus>();

        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        PlayerSpawnPoint point = FindFirstObjectByType<PlayerSpawnPoint>();

        if (point != null)
        {
            PlayerSpawnPoint = point.transform;
        }
    }
    
    private void Update()
    {
        //testyeo muerte player
        if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetTrigger("Die");
            TakeDamage(maxHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hurt");
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetBool("Die", true);
        // Disable player movement and other components here
       GetComponent<PlayerMovement>().enabled = false;
        // You can also disable other components like colliders, etc.

        eventBus.Publish(new EventBus.PlayerDeathEvent());

        animator.SetTrigger("Die");

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds before respawning
        currentHealth = maxHealth;
        transform.position = PlayerSpawnPoint.position;
        animator.SetBool("Die", false);
        GetComponent<PlayerMovement>().enabled = true;
        animator.Play("PlayerIdle");
        // Reset player position or other necessary states here
    }   
}
