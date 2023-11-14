using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChar : MonoBehaviour, ITeamInterface, IMovementInterface
{
    HealthComponent healthComp;

    [SerializeField] private Joystick moveStick;
    [SerializeField] private Joystick aimStick;
    [SerializeField] UIManager uiManager;

    CharacterController characterController;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float turnSpeed = 30f;

    [SerializeField] int teamID = 1;

    Vector2 moveInput;
    Vector2 aimInput;

    Camera cam;
    [SerializeField] CameraRig camRig;

    Vector3 moveDir;
    Vector3 aimDir;

    Animator animator;
    float animTurnSpeed = 0f;
    public int GetTeamID()
    {
        return teamID;
    }
    [SerializeField] MovementComponent movementComponent;

    InventoryComponent inventoryComponent;
    public void SwitchWeapon()
    {
        inventoryComponent.NextWeapon();
    }

    // Start is called before the first frame update
    private void Awake()
    {
        moveStick.onInputValueChanged += MoveInputUpdated;
        aimStick.onInputValueChanged += AimInputUpdated;
        aimStick.onStickTapped += AimStickTapped;
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        inventoryComponent = GetComponent<InventoryComponent>();
        movementComponent = GetComponent<MovementComponent>();
        healthComp = GetComponent<HealthComponent>();
        healthComp.onHealthEmpty += StartDeath;

        cam = Camera.main;
    }

    private void StartDeath(float delta, float maxHealth)
    {
        animator.SetTrigger("death");
        uiManager.SetGameplayControlEnabled(false);
    }

    private void AimStickTapped()
    {
        animator.SetTrigger("switchWeapon");
    }

    private void AimInputUpdated(Vector2 inputVal)
    {
        aimInput = inputVal;
        aimDir = ConvertInputToWorldDir(aimInput);
    }

    private void MoveInputUpdated(Vector2 inputVal)
    {
        moveInput = inputVal;
        moveDir = ConvertInputToWorldDir(moveInput);
    }

    // Update is called once per frame
    void Update()
    {
        ProcessMoveInput();
        ProcessAimInput();
        UpdateAnim();
    }

    private void UpdateAnim()
    {
        float rightSpeed = Vector3.Dot(moveDir, transform.right);
        float fwdSpeed = Vector3.Dot(moveDir, transform.forward);


        animator.SetFloat("LeftSpeed", -rightSpeed);
        animator.SetFloat("FwdSpeed", fwdSpeed);

        animator.SetBool("firing", aimInput.magnitude > 0);
    }

    private void LateUpdate()
    {
        UpdateCamera();    
    }

    private void UpdateCamera()
    {
        if(aimDir.magnitude == 0)
        {
            camRig.AddYawInput(moveInput.x);
        }
    }

    Vector3 ConvertInputToWorldDir(Vector2 inputVal)
    {
        Vector3 rightDir = cam.transform.right;
        Vector3 upDir = cam.transform.forward;
        upDir.y = 0;
        upDir = upDir.normalized;

        return (rightDir * inputVal.x + upDir * inputVal.y).normalized;
    }

    private void ProcessAimInput()
    {
        Vector3 lookDir = aimDir.magnitude != 0 ? aimDir : moveDir; //if aim has input use aimdir, otherwise use movedir
        float goalAnimTurnSpeed = movementComponent.RotateTowards(lookDir);

        animTurnSpeed = Mathf.Lerp(animTurnSpeed, goalAnimTurnSpeed, Time.deltaTime * 10);
        if(animTurnSpeed < 0.01f)
        {
            animTurnSpeed = 0f;
        }
        animator.SetFloat("turnSpeed", animTurnSpeed);
    }

    private void ProcessMoveInput()
    {
        characterController.Move(moveDir * moveSpeed * Time.deltaTime);
    }

    public void DamagePoint()
    {
        inventoryComponent.DamagePoint();
    }

    public void RotateTowards(Vector3 direction)
    {
        movementComponent.RotateTowards(direction);
    }

    public void RotateTowards(GameObject target)
    {
        movementComponent.RotateTowards(target.transform.position - transform.position);
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }
}
