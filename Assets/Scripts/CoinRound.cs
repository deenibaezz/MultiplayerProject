using UnityEngine;

public class CoinRound : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // find PlayerScoreLocal on that object (or its parent)
        var score = other.GetComponent<PlayerScoreLocal>() ?? other.GetComponentInParent<PlayerScoreLocal>();
        if (score == null) return;

        // find the RoundManager (we assume one exists in scene)
        var rm = FindFirstObjectByType<RoundManager>();
        if (rm != null)
        {
            // use the same GameObject names like "PlayerA" / "PlayerB"
            rm.RoundWon(score);
        }

        // destroy the coin (RoundManager also destroys, but safe)
        Destroy(gameObject);
    }
}