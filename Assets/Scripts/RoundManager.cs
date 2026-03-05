using System.Collections;
using UnityEngine;
using TMPro;

public class RoundManager : MonoBehaviour
{
    [Header("Round Settings")]
    [SerializeField] private int totalRounds = 10;
    [SerializeField] private float interRoundDelay = 1.2f;
    [SerializeField] private Vector2 minPos = new(-7f, -4f);
    [SerializeField] private Vector2 maxPos = new(7f, 4f);
    [SerializeField] private float avoidRadius = 0.8f;
    [SerializeField] private LayerMask playerLayer;

    [Header("References")]
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private TMP_Text roundText;
    [SerializeField] private TMP_Text infoText; // shows "Player A wins round!" or final winner

    [Header("Players (assign)")]
    [SerializeField] private PlayerScoreLocal playerA;
    [SerializeField] private PlayerScoreLocal playerB;

    private int currentRound = 0;
    private GameObject activeCoin;
    private bool roundActive = false;
    private bool matchOver = false;

    void Start()
    {
        infoText.text = "";
        ResetScores();
        StartCoroutine(RunMatch());
    }

    private IEnumerator RunMatch()
    {
        yield return new WaitForSeconds(0.2f); // let objects initialize
        while (!matchOver && currentRound < totalRounds)
        {
            currentRound++;
            UpdateRoundUI();

            infoText.text = "";

            SpawnCoinForRound();
            roundActive = true;

            // wait until coin is collected (RoundWon will set roundActive = false)
            while (roundActive)
                yield return null;

            // short delay between rounds
            yield return new WaitForSeconds(interRoundDelay);

            // check early victory (first to majority)
            if (CheckEarlyWin())
                break;
        }

        EndMatch();
    }

    private void UpdateRoundUI()
    {
        roundText.text = $"Round {currentRound}/{totalRounds}";
    }

    private void SpawnCoinForRound()
    {
        // remove any existing coin just in case
        if (activeCoin != null) Destroy(activeCoin);

        // try to find a safe spot
        Vector2 pos = Vector2.zero;
        bool placed = false;
        int attempts = 0;
        while (!placed && attempts < 40)
        {
            attempts++;
            Vector2 p = new Vector2(Random.Range(minPos.x, maxPos.x), Random.Range(minPos.y, maxPos.y));
            Collider2D overlap = Physics2D.OverlapCircle(p, avoidRadius, playerLayer);
            if (overlap == null)
            {
                pos = p;
                placed = true;
            }
        }

        if (!placed)
        {
            pos = new Vector2(Random.Range(minPos.x, maxPos.x), Random.Range(minPos.y, maxPos.y));
        }

        activeCoin = Instantiate(coinPrefab, pos, Quaternion.identity);
        // Ensure coin has CoinRound.cs (see next script)
    }

    // called by coin when player picks it up
    public void RoundWon(PlayerScoreLocal winner)
    {
        if (!roundActive) return;
    roundActive = false;

    winner.AddPoint();

    infoText.text = $"{winner.gameObject.name} wins round {currentRound}!";

    if (activeCoin != null) Destroy(activeCoin);
    }

    private bool CheckEarlyWin()
    {
        int a = playerA.Score;
        int b = playerB.Score;
        int needed = (totalRounds / 2) + 1; // first to majority
        if (a >= needed || b >= needed)
            return true;
        return false;
    }

    private void EndMatch()
    {
        matchOver = true;
        int a = playerA.Score;
        int b = playerB.Score;
        string final;
        if (a == b) final = $"Match Tied!\nFinal Score: {a}-{b}";
        else if (a > b) final = $"Player A Wins!\nFinal Score: {a}-{b}";
        else final = $"Player B Wins!\nFinal Score: {b}-{a}";

        infoText.text = final;
        roundText.text = "Match Over";
    }

    public void ResetScores()
    {
        playerA.ResetScore();
        playerB.ResetScore();
        currentRound = 0;
        infoText.text = "";
        UpdateRoundUI();
    }

    // optional helpers to change round size from UI
    public void SetTotalRounds(int n) => totalRounds = n;
}