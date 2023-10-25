using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Driver;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Driver
{
    public class CvWheelsBase : MonoBehaviour
    {

        [SerializeField] private GameObject wheelPrefab;
        [SerializeField] private Transform[] wheelAttachPoints;    //Different to the placement of physics wheels
        [SerializeField] private List<CvWheels> wheelVisuals = new List<CvWheels>();

        [SerializeField]
        private VehicleMovement cpMain;

        private void Awake()
        {
            cpMain = transform.parent.parent.GetComponent<VehicleMovement>();
        }

        // Start is called before the first frame update
        void Start()
        {
            InitWheels();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            foreach (CvWheels wheelVisual in wheelVisuals)
            {
                wheelVisual.ProcessWheelVisuals(cpMain.input, cpMain.speedData);
            }
        }

        private void InitWheels()
        {
            foreach (Transform wheelAttachPoint in wheelAttachPoints)
            {
                CvWheels wheelVisual = Instantiate(wheelPrefab, wheelAttachPoint.position, wheelAttachPoint.rotation, wheelAttachPoint).GetComponent<CvWheels>();
                wheelVisual.SetUpWheel(cpMain.rb);
                wheelVisuals.Add(wheelVisual);
                Debug.Log($"wheelVisual transform: {wheelVisual.transform.position}");
            }
        }
    }
}
