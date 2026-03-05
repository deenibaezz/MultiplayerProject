using UnityEngine;

public class PlayerScoreLocal : MonoBehaviour
{
    public int Score { get; private set; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Coin")) return;

        Destroy(other.gameObject);
        Score++;
        Debug.Log($"{gameObject.name} score: {Score}");
    }
}