using UnityEngine;

public class CoinSpawnerLocal : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int coinCount = 20;
    [SerializeField] private Vector2 minPos = new(-8, -4);
    [SerializeField] private Vector2 maxPos = new(8, 4);

    void Start()
    {
        for (int i = 0; i < coinCount; i++)
        {
            Vector2 p = new(
                Random.Range(minPos.x, maxPos.x),
                Random.Range(minPos.y, maxPos.y)
            );

            Instantiate(coinPrefab, p, Quaternion.identity);
        }
    }
}