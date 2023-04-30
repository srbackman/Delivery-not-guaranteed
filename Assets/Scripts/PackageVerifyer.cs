using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageVerifyer : MonoBehaviour
{
    /*Editor settings.*/
    [SerializeField] private int[] PackagePointValues = new int[]
    {
        10,
        30,
        50
    };

    /*Private values.*/
    private string[] NamesPackageArray = new string[]
    {
        "PackageLight",
        "PackageMedium",
        "PackageHeavy"
    };

    public bool VerifyPackage(Transform Target)
    {
        foreach (string Name in NamesPackageArray)
        {
            if (Target.CompareTag(Name))
                return (true);
        }
        return (false);
    }

    public int GetPackagePointValue(Transform Target)
    {
        int i = 0;
        while (i < 3)
        {
            if (Target.CompareTag(NamesPackageArray[i]))
                break;
            i++;
        }

        if (i >= 3)
            return (0);

        int Points = PackagePointValues[i];

        return (Points);
    }

    public string GetRandomPackageName()
    {
        int Index = Random.Range(0, 3);
        string PackageName = NamesPackageArray[Index];
        return (PackageName);
    }

    public int GetAmountOfCertainTags(List<Transform> Packages, string WantedTag)
    {
        int Count = 0;
        foreach (Transform T in Packages)
        {
            if (T.CompareTag(WantedTag))
                Count++;
        }
        return (Count);
    }

    public int GetAmountOfCertainTags(List<string> Packages, string WantedTag)
    {
        int Count = 0;
        foreach (string T in Packages)
        {
            if (T == WantedTag)
                Count++;
        }
        return (Count);
    }
}
