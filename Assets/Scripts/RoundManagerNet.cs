using System.Collections;
using Unity.Netcode;
using UnityEngine;
using Unity.Collections;
using System.Collections.Generic;
using System.Text;


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

    public NetworkVariable<int> CurrentRound = new NetworkVariable<int>(
    0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public NetworkVariable<FixedString64Bytes> InfoMessage = new NetworkVariable<FixedString64Bytes>(
    default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public NetworkVariable<bool> MatchOver = new NetworkVariable<bool>(
    false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

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
            CurrentRound.Value = currentRound;

            InfoMessage.Value = "";     // clear center text at start of round
            MatchOver.Value = false;

            SpawnCoin();

            // wait until coin is picked up (despawned)
            while (activeCoin != null && activeCoin.IsSpawned)
                yield return null;

            yield return new WaitForSeconds(interRoundDelay);

            if (CheckEarlyWin())
                break;
        }
        MatchOver.Value = true;

        EndMatchNet();
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
    public void AnnounceRoundWinner(ulong winnerClientId)
    {
    // Runs on server because CoinNetPickup only calls this on server
    InfoMessage.Value = $"Player {winnerClientId} won the round!";
    }
    private void EndMatchNet()
{
    // Build score list and determine winner(s)
    int bestScore = int.MinValue;
    List<ulong> winners = new List<ulong>();
    var sb = new StringBuilder();

    // Iterate connected clients (works on server)
    foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
    {
        var playerObj = client.PlayerObject;
        if (playerObj == null) continue;

        var ps = playerObj.GetComponent<PlayerScoreNet>();
        if (ps == null) continue;

        int sc = ps.Score.Value;
        sb.AppendLine($"Player {ps.OwnerClientId}: {sc}");

        if (sc > bestScore)
        {
            bestScore = sc;
            winners.Clear();
            winners.Add(ps.OwnerClientId);
        }
        else if (sc == bestScore)
        {
            winners.Add(ps.OwnerClientId);
        }
    }

    string scoresText = sb.ToString().TrimEnd();

    string final;
    if (winners.Count > 1)
    {
        final = $"Match Tied!\nFinal Scores:\n{scoresText}";
    }
    else if (winners.Count == 1)
    {
        final = $"Player {winners[0]} wins!\nFinal Scores:\n{scoresText}";
    }
    else
    {
        // safety fallback
        final = $"Match Over!\nFinal Scores:\n{scoresText}";
    }

    InfoMessage.Value = final;
}
}