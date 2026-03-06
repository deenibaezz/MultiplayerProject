using Unity.Netcode;
using UnityEngine;

public class CoinNetPickup : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsServer) return; // server decides pickups

        var score = other.GetComponent<PlayerScoreNet>();
        if (score == null) return;

        score.AddPointServerRpc();

        NetworkObject.Despawn(true); // remove coin for everyone
    }
}