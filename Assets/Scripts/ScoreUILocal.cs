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

        text.text = $"Player A: {a}\nPlayer B: {b}";
    }
}