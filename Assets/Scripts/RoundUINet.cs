using TMPro;
using Unity.Netcode;
using UnityEngine;

public class RoundUINet : MonoBehaviour
{
    [SerializeField] private TMP_Text roundText;
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private int totalRounds = 10;

    void Update()
    {
        var rm = Object.FindFirstObjectByType<RoundManagerNet>();
        if (rm == null)
        {
            roundText.text = "";
            infoText.text = "";
            return;
        }

        int r = rm.CurrentRound.Value;

        roundText.text = rm.MatchOver.Value ? "Match Over" : $"Round {r}/{totalRounds}";
        infoText.text = rm.InfoMessage.Value.ToString();
    }
}