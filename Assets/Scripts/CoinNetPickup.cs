using Unity.Netcode;
using UnityEngine;

public class CoinNetPickup : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsServer) return; // server decides coin pickup

        var score = other.GetComponent<PlayerScoreNet>();
        if (score == null) return;

        // update score on server
        score.AddPointServerRpc();

        // notify roundManager who won this round
        var rm = Object.FindFirstObjectByType<RoundManagerNet>();
        if (rm != null)
            rm.AnnounceRoundWinner(score.OwnerClientId);

        // remove coin for all clients
        NetworkObject.Despawn(true);
    }
}