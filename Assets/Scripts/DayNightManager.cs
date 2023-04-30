using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightManager : MonoBehaviour
{
    /*Editor settings*/
    [SerializeField] private Light DirectionalLight = null;
    [SerializeField] private int CycleDuration = 180;
    [SerializeField] private int LightsOn = 70;
    [SerializeField] private int LightsOff = 160;
    [SerializeField] private float SunStartPos = 50;
    [SerializeField] private float SunEndPos = 410;

    /*Private values*/
    private float DayNightTimer = 0f;

    void Start()
    {
        DayNightTimer = CycleDuration;
    }

    // Update is called once per frame
    void Update()
    {
        DayNightTimer -= Time.deltaTime;

        Vector3 SunPosition = new Vector3(Mathf.Lerp(SunStartPos, SunEndPos, DayNightTimer / CycleDuration), -30f, 0f);
        DirectionalLight.transform.eulerAngles = SunPosition;

        if (DayNightTimer <= 0)
            DayNightTimer = CycleDuration;
    }
}
