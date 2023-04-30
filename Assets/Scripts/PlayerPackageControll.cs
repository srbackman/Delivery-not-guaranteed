using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPackageControll : MonoBehaviour
{
    private Transform CurrentPackage = null;
    private PackageVerifyer Verifyer = null;
    private Camera PlayerCamera = null;


    private void Start()
    {
        Verifyer = FindObjectOfType<PackageVerifyer>();
        PlayerCamera = FindObjectOfType<Camera>();
    }

    public void TryToInteract(float Distance, Transform PackageHolder)
    {
        if (CurrentPackage)
        {
            DropPackage();
            return;
        }

        RaycastHit OutHit;
        Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out OutHit, Distance);
        
        if (OutHit.transform == null)
            return;

        /*Try to Interact with the object.*/
        Button GoodButton = OutHit.transform.GetComponent<Button>();
        if (GoodButton)
        {
            GoodButton.Trigger();
            return;
        }

        /*Try to pick up the object.*/
        bool IsAPackage = Verifyer.VerifyPackage(OutHit.transform);
        if (IsAPackage)
            PickUpPackage(OutHit.transform);
    }

    public bool CheckHeldPackage()
    {
        return (CurrentPackage);
    }

    public void ThrowPackage(float ThrowForce)
    {
        //Vector3 Direction = (CurrentPackage.position - PlayerCamera.transform.position).normalized;
        Vector3 Direction = PlayerCamera.transform.forward;
        Rigidbody Rb = CurrentPackage.GetComponent<Rigidbody>();
        Rb.velocity = new Vector3(0f, 0f, 0f);
        Rb.AddForce(Direction * ThrowForce, ForceMode.Impulse);
        CurrentPackage = null;
    }

    public void TryPunchPackage(float PunchDistance, float PunchForce)
    {
        RaycastHit OutHit;
        Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out OutHit, PunchDistance);

        if (!OutHit.transform)
            return;

        Rigidbody Rb = OutHit.transform.GetComponent<Rigidbody>();

        if (!Rb)
            return;

        Vector3 Direction = (OutHit.transform.position - PlayerCamera.transform.position).normalized;
        Rb.AddForce(Direction * PunchForce, ForceMode.Impulse);
    }

    public bool PullPackage(float PullForce, float MinimumDistance, Transform PackageHolder)
    {
        bool AtDestination = false;

        Vector3 Direction = (PackageHolder.position - CurrentPackage.position).normalized;

        CurrentPackage.GetComponent<Rigidbody>().AddForce(Direction * PullForce, ForceMode.Acceleration);

        return (AtDestination);
    }
    
    private void PickUpPackage(Transform Target)
    {
        CurrentPackage = Target;
    }

    private void DropPackage()
    {


        print("package dropped");
        CurrentPackage = null;
    }

    private void ActivateTarget()
    {

    }
}
