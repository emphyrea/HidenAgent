using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChar : MonoBehaviour
{
    [SerializeField] private Joystick moveStick;
    [SerializeField] private Joystick aimStick;

    CharacterController characterController;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float turnSpeed = 30f;

    Vector2 moveInput;
    Vector2 aimInput;

    Camera cam;
    [SerializeField] CameraRig camRig;

    Vector3 moveDir;
    Vector3 aimDir;

    Animator animator;

    // Start is called before the first frame update
    private void Awake()
    {
        moveStick.onInputValueChanged += MoveInputUpdated;
        aimStick.onInputValueChanged += AimInputUpdated;
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        cam = Camera.main;
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
        float leftSpeed = Vector3.Dot(moveDir, transform.right);
        float fwdSpeed = Vector3.Dot(moveDir, transform.forward);

        animator.SetFloat("LeftSpeed", leftSpeed);
        animator.SetFloat("FwdSpeed", fwdSpeed);

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
        if (lookDir.magnitude != 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookDir, Vector3.up), Time.deltaTime * turnSpeed);
        }
    }

    private void ProcessMoveInput()
    {
        characterController.Move(moveDir * moveSpeed * Time.deltaTime);
    }
}
