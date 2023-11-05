using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Driver;

[System.Serializable]
public class CpWheels : MonoBehaviour
{
    private VehicleMovement cpMain;
    private Dictionary<Transform, WheelHitData> _mapWheelToLastHitCache =
        new Dictionary<Transform, WheelHitData>();

    private class WheelHitData
    {
        public bool IsGrounded;
        public RaycastHit GroundData;
    }

    //Wheel Variables
    public float wheelHeight;
    public LayerMask groundCheckLayer;


    [Space]
    public CpWheelData cpWheelData;

    private void Awake()
    {
        cpMain = transform.parent.GetComponent<VehicleMovement>();
    }

    private void Start()
    {
        foreach (var wheel in cpWheelData.physicsWheelPoints)
        {
            _mapWheelToLastHitCache[wheel] = new WheelHitData();
        }
    }

    private void FixedUpdate()
    {
        UpdateWheelStates();
        cpMain.wheelData = cpWheelData;
    }

    private void UpdateWheelStates()
    {
        Vector3 surfaceNormal = Vector3.zero;

        cpWheelData.numberOfGroundedWheels = 0;
        cpWheelData.grounded = false;
        RaycastHit hit;

        foreach (Transform wheel in cpWheelData.physicsWheelPoints)
        {
            bool didhit = Physics.Raycast(
                wheel.position,
                -wheel.transform.up,
                out hit,
                wheelHeight,
                groundCheckLayer
            );
            Debug.DrawRay(wheel.position, -wheel.transform.up * wheelHeight, Color.red);

            WheelHitData wheelHitData = _mapWheelToLastHitCache[wheel];

            wheelHitData.IsGrounded = didhit;
            wheelHitData.GroundData = hit;

            if (!didhit)
                continue;

            if (hit.transform.gameObject.layer == 4)
            {
                cpWheelData.numberOfWateredWheels += 1;
            }
            else
            {
                cpWheelData.grounded = true;
                cpWheelData.numberOfGroundedWheels += 1;

                surfaceNormal += hit.normal;
            }
        }

        cpWheelData.averageWheelSurfaceNormal = surfaceNormal.normalized;
    }
}

[System.Serializable]
public class CpWheelData
{
    public Transform[] physicsWheelPoints;
    public bool grounded;
    public int numberOfGroundedWheels;
    public Vector3 averageWheelSurfaceNormal;
    public int numberOfWateredWheels;
}
