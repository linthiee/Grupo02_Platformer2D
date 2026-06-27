using UnityEngine;

public class CoinSpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;

    private void Start()
    {
        CoinSpawnPoint[] spawnPoints = FindObjectsByType<CoinSpawnPoint>(FindObjectsSortMode.None);
        foreach (var spawnPoint in spawnPoints)
        {
            Instantiate(coinPrefab, spawnPoint.transform.position, Quaternion.identity);
        }
    }
}