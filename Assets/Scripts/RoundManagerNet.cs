using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class RoundManagerNet : NetworkBehaviour
{
    [Header("Round Settings")]
    [SerializeField] private int totalRounds = 10;          // first to 6 when totalRounds=10
    [SerializeField] private float interRoundDelay = 1.0f;

    [Header("Spawn Area")]
    [SerializeField] private Vector2 minPos = new(-7f, -4f);
    [SerializeField] private Vector2 maxPos = new(7f, 4f);
    [SerializeField] private float avoidRadius = 0.8f;
    [SerializeField] private LayerMask playerLayer;

    [Header("Prefabs")]
    [SerializeField] private NetworkObject coinPrefab;

    private int currentRound = 0;
    private NetworkObject activeCoin;

    public override void OnNetworkSpawn()
    {
        // Only server runs the match loop
        if (IsServer)
            StartCoroutine(RunMatch());
    }

    private IEnumerator RunMatch()
    {
        yield return new WaitForSeconds(0.3f);

        while (currentRound < totalRounds)
        {
            currentRound++;

            SpawnCoin();

            // wait until coin is picked up (despawned)
            while (activeCoin != null && activeCoin.IsSpawned)
                yield return null;

            yield return new WaitForSeconds(interRoundDelay);

            if (CheckEarlyWin())
                break;
        }
    }

    private void SpawnCoin()
    {
        if (activeCoin != null && activeCoin.IsSpawned)
            activeCoin.Despawn(true);

        Vector2 pos = FindSafeSpawn();
        activeCoin = Instantiate(coinPrefab, pos, Quaternion.identity);
        activeCoin.Spawn(true);
    }

    private Vector2 FindSafeSpawn()
    {
        for (int attempts = 0; attempts < 40; attempts++)
        {
            Vector2 p = new(Random.Range(minPos.x, maxPos.x), Random.Range(minPos.y, maxPos.y));
            if (Physics2D.OverlapCircle(p, avoidRadius, playerLayer) == null)
                return p;
        }

        return new(Random.Range(minPos.x, maxPos.x), Random.Range(minPos.y, maxPos.y));
    }

    private bool CheckEarlyWin()
    {
        int needed = (totalRounds / 2) + 1;

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            var playerObj = client.PlayerObject;
            if (playerObj == null) continue;

            var score = playerObj.GetComponent<PlayerScoreNet>();
            if (score != null && score.Score.Value >= needed)
                return true;
        }

        return false;
    }
}