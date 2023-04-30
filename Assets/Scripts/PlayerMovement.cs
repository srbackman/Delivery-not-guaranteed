using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody Rb = null;
    private CapsuleCollider Collider = null;

    // Start is called before the first frame update
    void Start()
    {
        Rb = this.GetComponent<Rigidbody>();
        Collider = this.GetComponent<CapsuleCollider>();
    }

    public bool IsPlayerGrounded()
    {
        float ColliderHeight = Collider.height;
        RaycastHit OutHit;
        Physics.Raycast(this.transform.position, Vector3.down, out OutHit, ColliderHeight / 2);

        bool Result = OutHit.transform;
        return (Result);
    }

    public void ApplyMovementForce(Vector2 Direction, float MaxForce, float ForceStep)
    {
        Vector3 Force = new Vector3(Direction.x , 0.0f, Direction.y) * ForceStep * Time.deltaTime;

        //if (!WithinLimitsXZ(Rb.velocity, Force, MaxForce))
        //    Force = (Force.normalized * (MaxForce / 2));

        if (WithinLimitsXZ(Rb.velocity, Force, MaxForce * (Direction.magnitude)))
        {
            Rb.AddRelativeForce(Force, ForceMode.Impulse);
            //print("adding force...");
        }
    }

    private bool WithinLimitsXZ(Vector3 BaseForce, Vector3 AddativeForce, float MaxForce)
    {
        /*Check X axis.*/
        float XDiff = Mathf.Abs(BaseForce.x + AddativeForce.x);
        if (XDiff > MaxForce)
        {
            //print("over x speed" + XDiff + "<-diff | maxforce->" + MaxForce);
            return (false);
        }

        /*Check Z axis.*/
        float ZDiff = Mathf.Abs(BaseForce.z + AddativeForce.z);
        if (ZDiff > MaxForce)
        {
            //print("over z speed");
            return (false);
        }

        /*Check total difference.*/
        //bool Result = (XDiff + ZDiff) < MaxForce;
        //print("result " + Result);
        return (true);
    }

    public void ApplyJumpForce(float Force)
    {
        Rb.AddRelativeForce((Vector3.up * Force), ForceMode.Impulse);
    }

}
