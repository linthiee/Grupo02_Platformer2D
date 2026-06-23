using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private int score;

    private IEventBus _eventBus;

    private void Awake()
    {
        Instance= this;
        _eventBus = ServiceLoader.GetService<EventBus>();
        _eventBus.Subscribe<EventBus.CoinCollectedEvent>(OnCoinCollected);
    }

    private void OnDestroy()
    {
        _eventBus?.Unsubscribe<EventBus.CoinCollectedEvent>(OnCoinCollected);
    }

    private void OnCoinCollected(EventBus.CoinCollectedEvent eventData)
    {
        AddScore(eventData.CoinValue);
    }

    public void AddScore(int value)
    {
        score += value;
        Debug.Log("Score: " + score);
    }
}