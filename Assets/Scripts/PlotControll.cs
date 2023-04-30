using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlotControll : MonoBehaviour
{
    /*Editor settings.*/
    [SerializeField] private Transform DropOffZonesParent = null;

    [SerializeField] private Color UnReadyColor = Color.red;
    [SerializeField] private Color ReadyColor = Color.green;

    [SerializeField] private Transform OkSign = null;

    [SerializeField] private TMP_Text NeedsText = null;

    /*Private values.*/
    private List<DropOffZone> DropOffZoneScripts = new List<DropOffZone>();
    private List<string> RequierdPackageList = null;
    private bool TaskReady = false;

    private ScoreManager SM = null;
    private PackageVerifyer Verifyer = null;

    int LightCount = 0;
    int MediumCount = 0;
    int HeavyCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (DropOffZonesParent == null)
            return;

        Verifyer = FindObjectOfType<PackageVerifyer>();
        SM = FindObjectOfType<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TaskReady)
            return;

        if (DropOffZoneScripts.Count == 0 || RequierdPackageList.Count == 0)
        {
            TaskReady = true;
            return;
        }    
        
        CheckDropOffZones();
    }

    public void SetData(List<string> Targets, int MaxDropOffZones)
    {
        RequierdPackageList = Targets;

        if (Verifyer == null)
            Verifyer = FindObjectOfType<PackageVerifyer>();

        LightCount = Verifyer.GetAmountOfCertainTags(Targets, "PackageLight");
        MediumCount = Verifyer.GetAmountOfCertainTags(Targets, "PackageMedium");
        HeavyCount = Verifyer.GetAmountOfCertainTags(Targets, "PackageHeavy");
        ManageNeedsBoard(LightCount, MediumCount, HeavyCount);

        SetZonesOn(MaxDropOffZones);
    }

    private void SetZonesOn(int MaxDropOffZones)
    {
        /*Get DropOffZones.*/
        foreach (Transform Child in DropOffZonesParent)
        {
            DropOffZone Temp = Child.GetComponent<DropOffZone>();
            DropOffZoneScripts.Add(Temp);
            Child.gameObject.SetActive(false);
        }

        int DropOffZonesActivated = 0;

        for (int i = 0; i < DropOffZoneScripts.Count; i++)
        {
                print("fones" + DropOffZonesActivated + " " + MaxDropOffZones);
            if (!(DropOffZonesActivated < MaxDropOffZones))
                break;

            int RandomValue = Random.Range(MaxDropOffZones + i, DropOffZoneScripts.Count + 1);
            bool SpawnBuilding = (1f <= (float)(RandomValue / DropOffZoneScripts.Count));

            if (SpawnBuilding)
            {
                print("zones");
                DropOffZoneScripts[i].gameObject.SetActive(true);
                DropOffZonesActivated++;
            }
        }
    }
    
    public bool IsReady()
    {
        if (OkSign && TaskReady)
            OkSign.gameObject.SetActive(true);
        return (TaskReady);
    }
    
    private bool CheckDropOffZones()
    {
        bool Ready = false;

        foreach (DropOffZone Zone in DropOffZoneScripts)
        {
            if (!Zone.gameObject.activeSelf)
                continue;

            List<Transform> TempPackages = Zone.GetPackagesInZone();

            for (int i = 0; i < TempPackages.Count; i++)
            {
                if (TempPackages[i] == null)
                {
                    TempPackages.RemoveAt(i);
                    continue;
                }

                for (int a = 0; a < RequierdPackageList.Count; a++)
                {
                    if (!TempPackages[i].CompareTag(RequierdPackageList[a]))
                        continue;

                    switch (TempPackages[i].tag)
                    {
                        case "PackageLight": LightCount--; break;
                        case "PackageMedium": MediumCount--; break;
                        case "PackageHeavy": HeavyCount--; break;
                        default: break;
                    }

                    SM.AddToTotalScore(Verifyer.GetPackagePointValue(TempPackages[i]));

                    Destroy(TempPackages[i].gameObject);
                    TempPackages.RemoveAt(i);
                    RequierdPackageList.RemoveAt(a);

                    break;
                }
            }
        }
        ManageNeedsBoard(LightCount, MediumCount, HeavyCount);

        return (Ready);
    }

    private void ManageNeedsBoard(int LightCount, int MediumCount, int HeavyCount)
    {
        string BoardNumers = LightCount + "\n" + MediumCount + "\n" + HeavyCount;
        NeedsText.SetText(BoardNumers);
    }
}
