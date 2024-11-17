using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(BuildingReceivedEnergy))]
public class DroneRechargeStation : MonoBehaviour
{
    [SerializeField] private PurifierDroneController _connDrone;

    [Header("Charge parameters")]
    [SerializeField] private float _timeUntilFullyCharged = 0.0f;
    private float _droneMaxCharge = 100.0f;

    private BuildingReceivedEnergy _energyReceiver;
    private bool _isDroneDocked = false;


    void Start()
    {
        _energyReceiver = GetComponent<BuildingReceivedEnergy>();

        // For testing purposes:
        _energyReceiver.CalculateSumOfEnergy();



    }

    // NEEDS TO CHARGE WHEN BOT IS DOCKED
    // CHARGE THE BOT ONLY WHEN POWERED
    // SEND THE BOT AWAY WHEN IT IS FULLY CHARGED


    // Update is called once per frame
    void Update()
    {
        if (!_isDroneDocked || !_energyReceiver.IsPowered()) return;

        if (_connDrone.CurrentDroneCharge < _droneMaxCharge)
        {
            _connDrone.CurrentDroneCharge = Mathf.Clamp
            (
                _connDrone.CurrentDroneCharge + ((_droneMaxCharge / _timeUntilFullyCharged) * Time.deltaTime),
                0.0f,
                _droneMaxCharge
            );
            Debug.Log($"Charging... Current charge: {_connDrone.CurrentDroneCharge}");
        }
        else
        {
            UndockDrone();
        }
    }

    public void DockDrone()
    {
        _isDroneDocked = true;
    }

    public void UndockDrone()
    {
        _isDroneDocked = false;
        _connDrone.UndockDrone();
    }
}
