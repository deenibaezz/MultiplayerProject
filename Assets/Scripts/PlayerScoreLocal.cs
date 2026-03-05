using UnityEngine;

public class PlayerScoreLocal : MonoBehaviour
{
    public int Score { get; private set; }

    void Awake()
    {
        Score = 0;
    }

    public void ResetScore()
    {
        Score = 0;
    }

    public void AddPoint()
    {
        Score++;
        Debug.Log($"{gameObject.name} score: {Score}");
    }
}