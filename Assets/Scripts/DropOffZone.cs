using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOffZone : MonoBehaviour
{

    private List<Transform> Packages = new List<Transform>();
    private PackageVerifyer Verifyer = null;

    void Start()
    {
        Verifyer = FindObjectOfType<PackageVerifyer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //print("trigger triggered");
        if (Verifyer.VerifyPackage(other.transform) && !IsAlreadyRegistered(other.transform))
            Packages.Add(other.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (Verifyer.VerifyPackage(other.transform) && IsAlreadyRegistered(other.transform))
            Packages.Remove(other.transform);
    }

    private bool IsAlreadyRegistered(Transform Target)
    {
        bool Result = Packages.Contains(Target);
        return (Result);
    }

    public List<Transform> GetPackagesInZone()
    {
        return (Packages);
    }

    public void RemovePackage(int Index)
    {
        Destroy(Packages[Index].gameObject);
        Packages.RemoveAt(Index);
    }

    public void CleanUpPackageList()
    {
        Packages.Clear();
    }

}
