using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraControll : MonoBehaviour
{

    [SerializeField] private Transform GimbalXAxis = null;
    float XRotate = 0.0f;
    float YRotate = 0.0f;

    public void RotateCamera(Vector2 Direction, float LookStepSpeed, float UpperLimit, float LowerLimit)
    {
        float DirectionX = Direction.x * LookStepSpeed * Time.deltaTime;
        float DirectionY = Direction.y * LookStepSpeed * Time.deltaTime;

        XRotate -= DirectionY;
        XRotate = Mathf.Clamp(XRotate, LowerLimit, UpperLimit);

        GimbalXAxis.localRotation = Quaternion.Euler(XRotate, 0f, 0f);
        transform.Rotate(Vector3.up, DirectionX);
    }

    private bool OverLimit(float Value, float Limit)
    {
        bool Result = false;
        if (Value < 0)
            Result = (Value < -Limit);
        else
            Result = (Value > Limit);
        return (Result);
    }
}
