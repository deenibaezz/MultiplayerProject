using TMPro;
using UnityEngine;

public class ScoreUILocal : MonoBehaviour
{
    [SerializeField] private PlayerScoreLocal playerA;
    [SerializeField] private PlayerScoreLocal playerB;

    private TMP_Text text;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        int a = playerA.Score;
        int b = playerB.Score;

        string leader =
            a == b ? "Tied" :
            a > b ? "Player A winning" :
                    "Player B winning";

        text.text = $"Player A: {a}\nPlayer B: {b}\n{leader}";
    }
}