using UnityEngine;

[RequireComponent(typeof(BuildingReceivedEnergy))]
public class DroneRechargeStation : MonoBehaviour
{
    [SerializeField] private DroneController _connDrone;

    [Header("Charge parameters")]
    [SerializeField] private float _timeUntilFullyCharged = 0.0f;
    private float _droneMaxCharge = 100.0f;

    private BuildingReceivedEnergy _energyReceiver;
    private bool _isDroneDocked = false;

    public void Start()
    {
        _connDrone.InitDrone();
    }


    // [ContextMenu("Test/Launch Production")]
    public void InitRechargeStation()
    {
        _energyReceiver = GetComponent<BuildingReceivedEnergy>();
    }

    void Update()
    {
        if (!_isDroneDocked || !_energyReceiver.IsPowered()) return;

        if (_connDrone.CurrentDroneCharge < _droneMaxCharge)
        {
            Debug.Log("Charging...");
            _connDrone.CurrentDroneCharge = Mathf.Clamp
            (
                _connDrone.CurrentDroneCharge + ((_droneMaxCharge / _timeUntilFullyCharged) * Time.deltaTime),
                0.0f,
                _droneMaxCharge
            );

        }
        else
        {
            Debug.Log("Undocking...");
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
