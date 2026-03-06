using System.Text;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ScoreUINet : MonoBehaviour
{
    private TMP_Text text;

    void Awake() => text = GetComponent<TMP_Text>();

    void Update()
    {
        if (NetworkManager.Singleton == null || (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer))
        {
            text.text = "Not connected";
            return;
        }

        // Find all spawned PlayerScoreNet components and display their NetworkVariable scores.
        var scores = Object.FindObjectsByType<PlayerScoreNet>(FindObjectsSortMode.None);

        if (scores == null || scores.Length == 0)
        {
            text.text = "Waiting for players...";
            return;
        }

        // Optional: sort by OwnerClientId so order is stable
        System.Array.Sort(scores, (a, b) => a.OwnerClientId.CompareTo(b.OwnerClientId));

        var sb = new StringBuilder();
        foreach (var ps in scores)
        {
            // Label "You" vs "Other" to make it obvious in recordings
            string who = ps.IsOwner ? "You" : $"Player {ps.OwnerClientId}";
            sb.AppendLine($"{who}: {ps.Score.Value}");
        }

        text.text = sb.ToString().TrimEnd();
    }
}