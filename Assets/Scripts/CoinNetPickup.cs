using Unity.Netcode;
using UnityEngine;

public class CoinNetPickup : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsServer) return;

        var score = other.GetComponent<PlayerScoreNet>();
        if (score == null) return;

        // Award point
        score.AddPointServerRpc();

        // Notify RoundManager who won this round
        var rm = Object.FindFirstObjectByType<RoundManagerNet>();
        if (rm != null)
            rm.AnnounceRoundWinner(score.OwnerClientId);

        // Despawn coin for everyone
        NetworkObject.Despawn(true);
    }
}