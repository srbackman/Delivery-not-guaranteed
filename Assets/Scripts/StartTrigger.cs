using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTrigger : MonoBehaviour
{
    [SerializeField] private Button StartButton = null;

    private LevelManager LM = null;

    private void Start()
    {
        LM = FindObjectOfType<LevelManager>();
    }

    private void Update()
    {
        if (ButtonPressed())
            LM.ForceTriggerNextBlock();
    }

    private bool ButtonPressed()
    {
        return (StartButton.IsTriggered());
    }
}
