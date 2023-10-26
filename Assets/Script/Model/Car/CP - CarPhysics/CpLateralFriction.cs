using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Driver;

[System.Serializable]
public class CpLateralFriction : MonoBehaviour
{
    //TweakableVariables
    public AnimationCurve slideFrictionCurve;
    [Range(0, 1)]
    public float baseTireStickiness;
    [Space]
    public float currentTireStickiness;
    [Space]
    public float slidingFrictionRatio;
    public float slidingFrictionForceAmount;
    public float slidingFrictionToForwardSpeedAmount;

    private VehicleMovement cpMain;

    private void Awake()
    {
        cpMain = transform.parent.GetComponent<VehicleMovement>();
    }


    // Update is called once per frame
    void LateUpdate()
    {
        CalculateLateralFriction(cpMain.speedData);
    }

    private void FixedUpdate()
    {
        ApplyLateralFriction(cpMain.wheelData.grounded, cpMain.rb);
    }

    private void CalculateLateralFriction(VehicleSpeed speedData)
    {
        float slideFrictionRatio = 0;

        if (Math.Abs(speedData.sideSpeed + speedData.forwardSpeed) > 0.01f)
            slideFrictionRatio = Mathf.Clamp01(Mathf.Abs(speedData.sideSpeed) / (Mathf.Abs(speedData.sideSpeed) + speedData.forwardSpeed));

        slidingFrictionRatio = slideFrictionCurve.Evaluate(slideFrictionRatio);

        slidingFrictionForceAmount = slidingFrictionRatio * -speedData.sideSpeed * currentTireStickiness;
    }

    private void ApplyLateralFriction(bool grounded, Rigidbody rb)
    {
        if (!grounded)
            return;

        //Stops sideways sliding 
        rb.AddForce(slidingFrictionForceAmount * rb.transform.right, ForceMode.Impulse);
        currentTireStickiness = baseTireStickiness;
    }
}
