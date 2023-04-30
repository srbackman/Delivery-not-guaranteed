using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [Header("FILL THIS!")]
    [SerializeField] Transform PackageHolder = null;

    /*Editor settings.*/
    [Header("Movement X & Z")]
    [SerializeField] private float MaxSpeed = 5f;
    [SerializeField] private float SpeedStepForce = 2f;

    [Header("Jumping")]
    [SerializeField] private float JumpForce = 4f;
    [SerializeField] private int MaxJumps = 1;

    [Header("Looking")]
    [SerializeField] private float LookStepSpeed = 100f;
    [SerializeField] private float MaxUpAngle = 80f;
    [SerializeField] private float MaxDownAngle = -50f;

    [Header("Interacting")]
    [SerializeField] private float InteractionDistance = 5f;
    [SerializeField] private float ForcePullForce = 20f;
    [SerializeField] private float ForcePullStopDistance = 2f;

    [Header("Punch/Throw")]
    [SerializeField] private float PunchForce = 20f;
    [SerializeField] private float PunchDistance = 5f;
    [SerializeField] private float PunchThrowCooldown = 1f;
    [SerializeField] private float ThrowForce = 5f;

    /*Private values and data.*/
    private PlayerMovement CurrentPlayerMovement = null;
    private PlayerCameraControll CurrentPlayerCameraControll = null;
    private PlayerPackageControll CurrentPackageControll = null;
    private PlayerInputActions Input = null;

    private Vector2 MovementDirection = Vector2.zero;
    private Vector2 LookDirection = Vector2.zero;
    private bool InteractionState = false;
    private bool PunchThrowState = false;

    private bool JumpState = false;
    private bool JumpButtonUp = true;
    private int JumpsDone = 0;

    private bool InteractionButtonUp = true;

    private bool PunchThrowButtonUp = true;


    // Start is called before the first frame update
    private void Awake()
    {
        Input = new PlayerInputActions();

        CurrentPlayerMovement = FindObjectOfType<PlayerMovement>();
        CurrentPlayerCameraControll = CurrentPlayerMovement.GetComponent<PlayerCameraControll>();
        CurrentPackageControll = CurrentPlayerMovement.GetComponent<PlayerPackageControll>();

        Input.Player.Reset.started += ResetGame;
        Input.Player.LockToggle.started += LockToggle;
    }

    private void ResetGame(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    private void LockToggle(InputAction.CallbackContext context)
    {
        Cursor.visible = !Cursor.visible;
        if (Cursor.visible)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        Input.Player.Enable();
    }

    private void OnDisable()
    {
        Input.Player.Disable();
    }

    private void Update()
    {
        /*Move*/
        if (MovementDirection != Vector2.zero)
            CurrentPlayerMovement.ApplyMovementForce(MovementDirection, MaxSpeed, SpeedStepForce);

        /*Look*/
        if (LookDirection != Vector2.zero)
            CurrentPlayerCameraControll.RotateCamera(LookDirection,LookStepSpeed, MaxUpAngle, MaxDownAngle);
    }

    private void FixedUpdate()
    {
        ReadInputs();

        /*Reset*/
        //if ()

        /*Jump*/
        if (JumpState && (JumpsDone < MaxJumps))
        {
            CurrentPlayerMovement.ApplyJumpForce(JumpForce);
            JumpsDone++;
            print("jump" + JumpsDone);
            JumpState = false;
        }

        bool CurrentPackage = CurrentPackageControll.CheckHeldPackage();

        /*Punch/Throw*/
        if (PunchThrowState)
        {
            if (CurrentPackage)
                CurrentPackageControll.ThrowPackage(ThrowForce);
            else
                CurrentPackageControll.TryPunchPackage(PunchDistance, PunchForce);
            PunchThrowState = false;
            return;
        }

        /*Interact with something, pickup or drop a package.*/
        if (InteractionState)
        {
            CurrentPackageControll.TryToInteract(InteractionDistance, PackageHolder);
            InteractionState = false;
            return;
        }

        /*Pull package towards package holder.*/
        if (CurrentPackage)
            CurrentPackageControll.PullPackage(ForcePullForce, ForcePullStopDistance, PackageHolder);
    }

    public void Reset()
    {
        
    }

    private void ReadInputs()
    {
        //Grounding
        if (CurrentPlayerMovement.IsPlayerGrounded())
            JumpsDone = 0;

        //Movement
        MovementDirection = Input.Player.Move.ReadValue<Vector2>();

        //Jumping
        if (IsFloatOne(Input.Player.Jump.ReadValue<float>()))
        {
            if (JumpButtonUp)
            {
                JumpState = true;
                JumpButtonUp = false;
            }
        }
        else if (!JumpButtonUp)
        {
            JumpButtonUp = true;
        }

        //Looking
        LookDirection = Input.Player.Look.ReadValue<Vector2>();

        //Interacting
        if (IsFloatOne(Input.Player.Interact.ReadValue<float>()))
        {
            if (InteractionButtonUp)
            {
                print("interacted");
                InteractionState = true;
                InteractionButtonUp = false;
            }
        }
        else if (!InteractionButtonUp)
        {
            InteractionButtonUp = true;
        }

        //Punching/Throwing
        if (IsFloatOne(Input.Player.PunchThrow.ReadValue<float>()))
        {
            if (PunchThrowButtonUp)
            {
                PunchThrowState = true;
                PunchThrowButtonUp = false;
            }
        }
        else if (!PunchThrowButtonUp)
        {
            PunchThrowButtonUp = true;
        }
    }

    private bool IsFloatOne(float Value)
    {
        bool Result = (Value == 1.0f);
        return (Result);
    }
}
