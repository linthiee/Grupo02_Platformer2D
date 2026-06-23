using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class CoinLogic : MonoBehaviour
{
    [SerializeField] private int coinValue = 1;

    private IEventBus _eventBus;

    private void Start()
    {
        gameObject.SetActive(true);
    }

    private void Awake()
    {
        _eventBus = ServiceLoader.GetService<EventBus>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           _eventBus.Publish(new EventBus.CoinCollectedEvent(coinValue));

            Destroy(gameObject);
        }
    }
}