using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockControll : MonoBehaviour
{
    [System.Serializable]
    public struct PlotData
    {
        public Transform PlotSpawnPoint;
    }

    [SerializeField] PlotData[] Plots = new PlotData[4];
    [SerializeField] GameObject StartInvisibleWall = null;
    [SerializeField] GameObject EndInvisibleWall = null;


    private bool PlayerInArea = false;

    private bool BlockReady = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            PlayerInArea = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (BlockReady)
            return;

        CheckPlotProgresses();
    }

    public bool IsPlayerInside()
    {
        return (PlayerInArea);
    }

    public void InvisibleStartWallActive(bool State)
    {
        StartInvisibleWall.SetActive(State);
    }

    public void InvisibleEndWallActive(bool State)
    {
        EndInvisibleWall.SetActive(State);
    }

    public Transform GetPlotParent(int Index)
    {
        Transform TempTransform = Plots[Index].PlotSpawnPoint;
        return (TempTransform);
    }

    public bool IsBlockReady()
    {
        return (BlockReady);
    }

    private void CheckPlotProgresses()
    {
        bool Ready = true;

        foreach (PlotData Plot in Plots)
        {
            if (Plot.PlotSpawnPoint == null)
            {
                Ready = false;
                break;
            }

            PlotControll TempPlotControll = Plot.PlotSpawnPoint.GetChild(0).GetComponent<PlotControll>();

            if (TempPlotControll == null)
            {
                Ready = false;
                break;
            }

            bool PlotReady = TempPlotControll.IsReady();
            if (PlotReady)
                continue;

            Ready = false;
            break;
        }
        BlockReady = Ready;
    }
}
