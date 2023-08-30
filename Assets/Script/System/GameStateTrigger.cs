using System;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameStateTrigger : MonoBehaviour
{
    [SerializeField]
    private bool deadZoneTrigger = true;

    public event EventHandler<bool> OnWin;
    public event EventHandler<bool> OnLoss;


    private void Start() {
        GameManager.Instance.RegisterWinCheck(this);
        GameManager.Instance.RegisterLossCheck(this);
    }
    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            if (deadZoneTrigger == false)
            {
                OnWin?.Invoke(this, true);               
            } else 
            {
                OnLoss?.Invoke(this, true);
            }
        }
    }
}
