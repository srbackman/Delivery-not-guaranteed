using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryTruck : MonoBehaviour
{
    /*Editor*/
    [SerializeField] private Button[] PackageButtons = new Button[3];
    [SerializeField] private GameObject[] PackageObjects = new GameObject[3];
    [SerializeField] private Transform PackagesParent = null;
    [SerializeField] private Transform PackageSpawnPoint = null;
    [SerializeField] private float PackageForceSpeed = 20;
    [SerializeField] private int MaxPackagesAtTime = 20;
    [SerializeField] private float DeleteDepth = -5f;

    /*Private*/
    private List<Transform> Packages = new List<Transform>();
    private ScoreManager SM = null;
    private PackageVerifyer Verifyer = null;

    private void Start()
    {
        SM = FindObjectOfType<ScoreManager>();
        Verifyer = FindObjectOfType<PackageVerifyer>();
    }

    void Update()
    {
        /*Remove null slots.*/
        RemoveNullSlots();

        /*Remove packages that are belove DeleteDepth.*/
        CheckPackageHeights();

        /*Try to spawn a package.*/
        int Index = CheckButtons();
        if (Index != -1)
            SpawnPackage(Index);
    }

    private int CheckButtons()
    {
        if (PackageButtons[0].IsTriggered()) //Light
            return (0);

        if (PackageButtons[1].IsTriggered()) //Medium
            return (1);

        if (PackageButtons[2].IsTriggered()) //Heavy
            return (2);

        return (-1);
    }

    private void SpawnPackage(int Index)
    {
        if (Packages.Count >= MaxPackagesAtTime)
            return;

        GameObject Temp = Instantiate<GameObject>(PackageObjects[Index], PackageSpawnPoint.position, PackageSpawnPoint.rotation, PackagesParent);
        Packages.Add(Temp.transform);

        Rigidbody Rb = Temp.GetComponent<Rigidbody>();
        Rb.AddForce(PackageSpawnPoint.forward * PackageForceSpeed, ForceMode.VelocityChange);
    }

    private void CheckPackageHeights()
    {
        for (int i = 0; i < Packages.Count; i++)
        {
            if (Packages[i].position.y < DeleteDepth)
                RemovePackage(i);
        }
    }

    private void RemovePackage(int Index)
    {
        int PointsLost = Verifyer.GetPackagePointValue(Packages[Index]);
        SM.AddToTotalScore(-PointsLost);

        Destroy(Packages[Index].gameObject);
        Packages.RemoveAt(Index);
    }

    private void RemoveNullSlots()
    {
        for (int i = 0; i < Packages.Count; i++)
        {
            if (Packages[i] == null)
                Packages.RemoveAt(i);
        }
    }
}
