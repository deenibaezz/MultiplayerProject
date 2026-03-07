using TMPro;
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

        // Round text
        if (rm.MatchOver.Value)
            roundText.text = "Match Over";
        else if (rm.CurrentRound.Value == 0)
            roundText.text = "Waiting for players...";
        else
            roundText.text = $"Round {rm.CurrentRound.Value}/{totalRounds}";

        // Center info text (instructions / round winner / final result)
        infoText.text = rm.InfoMessage.Value.ToString();
    }
}