using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VehicleMovement : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;
    private Inputdesu input = null;
    private Vector2 _moveVector = Vector2.zero;
    private Rigidbody _rb = null;

    // Settings
    [SerializeField] private float motorForce, breakForce, maxSteerAngle;

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    private void Awake() 
    {
        input = new Inputdesu();
        _rb = GetComponent<Rigidbody>();
    }
    
    private void FixedUpdate() {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput() {
        // Steering Input
        horizontalInput = _moveVector.x;

        // Acceleration Input
        verticalInput = _moveVector.y;

        // Breaking Input
        // isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor() {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        // currentbreakForce = isBreaking ? breakForce : 0f;
        // ApplyBreaking();
    }

    // private void ApplyBreaking() {
    //     frontRightWheelCollider.brakeTorque = currentbreakForce;
    //     frontLeftWheelCollider.brakeTorque = currentbreakForce;
    //     rearLeftWheelCollider.brakeTorque = currentbreakForce;
    //     rearRightWheelCollider.brakeTorque = currentbreakForce;
    // }

    private void HandleSteering() {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels() {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform) {
        Vector3 pos;
        Quaternion rot; 
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    private void OnEnable() 
    {
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;   
        input.Player.Movement.canceled += OnMovementCancelled; 
    }

    private void OnDisable() 
    {
        input.Disable();    
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCancelled;
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        _moveVector = value.ReadValue<Vector2>();
    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        _moveVector = Vector2.zero;
    }
}