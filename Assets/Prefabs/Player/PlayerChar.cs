using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChar : MonoBehaviour
{
    [SerializeField] private Joystick moveStick;
    [SerializeField] private Joystick aimStick;

    CharacterController characterController;
    [SerializeField] float moveSpeed = 5f;

    // Start is called before the first frame update
    private void Awake()
    {
        moveStick.onInputValueChanged += MoveInputUpdated;
        aimStick.onInputValueChanged += AimInputUpdated;
        characterController = GetComponent<CharacterController>();
    }

    private void AimInputUpdated(Vector2 inputVal)
    {

    }

    private void MoveInputUpdated(Vector2 inputVal)
    {
        characterController.Move(new Vector3(inputVal.x, 0f, inputVal.y) * Time.deltaTime * moveSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
