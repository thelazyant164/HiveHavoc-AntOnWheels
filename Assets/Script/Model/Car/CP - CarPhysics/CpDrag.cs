using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Driver;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Driver
{
[System.Serializable]
public class CpDrag : MonoBehaviour
{
    [Header("Drag")]
    public float linearDrag;
    public float freeWheelDrag;
    public float brakingDrag;
    public float angularDrag;

    public bool linearDragCheck;
    public bool brakingDragCheck;
    public bool freeWheelDragCheck;

    private VehicleMovement cpMain;

    private void Awake()
    {
        cpMain = transform.parent.GetComponent<VehicleMovement>();
        cpMain.rb.angularDrag = angularDrag;
    }
    private void Start()
    {

    }
    // Update is called once per frame
    private void Update()
    {
        UpdateDrag(
            cpMain.rb,
            cpMain.wheelData.grounded,
            cpMain.input,
            cpMain.speedData
        );

    }

    private void UpdateDrag(Rigidbody rb, bool grounded, PlayerInputs input, VehicleSpeed speedData)
    {
        linearDragCheck = Mathf.Abs(input.accelInput) < 0.05 || grounded;
        float linearDragToAdd = linearDragCheck ? linearDrag : 0;

        brakingDragCheck = input.accelInput < 0 && speedData.forwardSpeed > 0;
        float brakingDragToAdd = brakingDragCheck ? brakingDrag : 0;
        
        freeWheelDragCheck = Math.Abs(input.accelInput) < 0.02f && grounded;
        float freeWheelDragToAdd = freeWheelDragCheck ? freeWheelDrag : 0;

        rb.drag = linearDragToAdd + brakingDragToAdd + freeWheelDragToAdd;
    }
}
}