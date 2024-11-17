using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DroneController : MonoBehaviour
{
    [Header("Charge Stats")]
    protected float _droneMaxCharge = 100.0f;
    public float CurrentDroneCharge = 0.0f;
    [SerializeField] protected float _timeUntilEmpty = 0.0f;
    [SerializeField] protected float _rechargeThreshold = 5.0f; // %
    [SerializeField] protected DroneRechargeStation _rechargeStation;



    protected Transform _rechargeStationTrans;
    protected bool _isDroneDocked = false;
    protected bool _lookForCharge = false;
    protected bool _isDroneInit = false;

    public void InitDrone()
    {
        if (_isDroneInit) return;

        _rechargeStationTrans = _rechargeStation.transform;
        _rechargeStation.InitRechargeStation();
        InitDroneSpecialized();
        _isDroneInit = true;
        Debug.Log("DRONE IS INIT");
    }

    protected abstract void InitDroneSpecialized();

    public void DockDrone()
    {
        _isDroneDocked = true;
        _rechargeStation.DockDrone();
    }

    public void UndockDrone()
    {
        _lookForCharge = false;
        _isDroneDocked = false;
    }
}
