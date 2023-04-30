using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

    [SerializeField] private TMP_Text LiveScoreText = null;
    [SerializeField] private TMP_Text TimeLeftText = null;


    private ScoreManager SM = null;

    // Start is called before the first frame update
    void Start()
    {
        SM = FindObjectOfType<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        int ScoreValue = SM.GetTotalScore();
        string ScoreText = "Score: " + ScoreValue;
        LiveScoreText.SetText(ScoreText);


        //string TimerText = "Time Left: 99.9s";
        //TimeLeftText.SetText(TimerText);

    }
}
