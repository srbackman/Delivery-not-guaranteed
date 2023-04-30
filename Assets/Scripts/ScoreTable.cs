using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTable : MonoBehaviour
{
    [SerializeField] private TMP_Text ScoreText = null;

    private ScoreManager SM = null;

    void Start()
    {
        SM = FindObjectOfType<ScoreManager>();
        SetScoreText();
    }

    private void SetScoreText()
    {
        int TotalScore = SM.GetTotalScore();
        string ScoreString = "Game Over\nFinal Score\n" + TotalScore;

        ScoreText.SetText(ScoreString);
    }
}
