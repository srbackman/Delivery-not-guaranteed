using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int TotalScore = 0;

    public void AddToTotalScore(int Amount)
    {
        TotalScore += Amount;
    }

    public int GetTotalScore()
    {
        return (TotalScore);
    }
}
