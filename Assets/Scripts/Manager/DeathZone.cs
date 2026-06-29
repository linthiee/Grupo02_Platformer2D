using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealt playerHealth = collision.GetComponent<PlayerHealt>();
            
            if (playerHealth != null)
            {
                playerHealth.Die(); 
            }
        }
        else if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
    }
}