using System;
using System.Text;
using TMPro;
using UnityEngine;

public class ScoreUINet : MonoBehaviour
{
    private TMP_Text text;

    void Awake() => text = GetComponent<TMP_Text>();

    void Update()
    {
        var players = UnityEngine.Object.FindObjectsByType<PlayerScoreNet>(FindObjectsSortMode.None);

        if (players == null || players.Length == 0)
        {
            text.text = "";
            return;
        }

        Array.Sort(players, (a, b) => a.OwnerClientId.CompareTo(b.OwnerClientId));

        var sb = new StringBuilder();
        foreach (var ps in players)
        {
            int playerNum = (int)ps.OwnerClientId + 1;
            sb.AppendLine($"Player {playerNum}: {ps.Score.Value}");
        }

        text.text = sb.ToString().TrimEnd();
    }
}