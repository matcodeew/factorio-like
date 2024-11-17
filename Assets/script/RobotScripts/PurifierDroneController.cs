using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using UnityEngine;

public class PurifierDroneController : MonoBehaviour
{
    [SerializeField] private DroneRechargeStation _rechargeStation;
    private Transform _rechargeStationTrans;
    private Transform _droneTrans;


    [Header("Drone Stats")]
    [SerializeField] private float _droneSpeed = 5.0f;
    [SerializeField] private int _droneManhattanRange = 0;
    [SerializeField] private float _interactionDist = 0.5f;

    [Header("Charge Stats")]
    private float _droneMaxCharge = 100.0f;
    public float CurrentDroneCharge = 100.0f;
    [SerializeField] private float _timeUntilEmpty = 0.0f;
    [SerializeField] private float _rechargeThreshold = 5.0f; // %
    private bool _isDroneDocked = false;
    private bool _lookForCharge = false;
    private bool _movingToNewTile = false;
    private bool _isPurifying = false;
    private bool _hasPurifiedAll = false;


    [Header("Purifier Variables")]
    [SerializeField] private List<TileData> _tiles = new List<TileData>();
    private Vector3 _targetPos = Vector3.zero;
    [SerializeField] private float _timeUntilPurified = 0.0f;
    private float _maxPurifiedTileVal = 1.0f;


    void Start()
    {
        _droneTrans = transform;
        _rechargeStationTrans = _rechargeStation.transform;

        InitTileList();
    }

    private void InitTileList()
    {
        for (int x = -_droneManhattanRange; x <= _droneManhattanRange; x++)
            for (int z = -_droneManhattanRange; z <= _droneManhattanRange; z++)
            {
                if ((x + z) < _droneManhattanRange)
                {
                    TileData newTile = MapManager.Instance.AccessTileByPos(new Vector3(x, 0, z));
                    if (newTile != null)
                        _tiles.Add(newTile);


                }
            }
        _hasPurifiedAll = _tiles.Count == 0;
    }



    void FixedUpdate()
    {
        // go to target


        // if drone is docked -> charging
        if (_isDroneDocked || _isPurifying || _hasPurifiedAll) return;


        // drone power consumption 
        CurrentDroneCharge = Mathf.Clamp
            (
                CurrentDroneCharge - ((_droneMaxCharge / _timeUntilEmpty) * Time.deltaTime),
                0.0f,
                _droneMaxCharge
            );

        if (!_lookForCharge)
        {
            if (CurrentDroneCharge <= _rechargeThreshold)
            {
                // if drone charge is under 5% and is not docked 
                //      -> target = recharge station
                _lookForCharge = true;
                _movingToNewTile = false;

                _targetPos = _rechargeStationTrans.position;
                _targetPos = new Vector3(_targetPos.x, transform.position.y, _targetPos.z);
            }
            else if (!_movingToNewTile)
            {
                // if drone has not set a new target
                //      sets a new target to random from tiles list
                _movingToNewTile = true;

                _targetPos = _tiles[Random.Range(0, _tiles.Count - 1)].transform.position;
                _targetPos = new Vector3(_targetPos.x, transform.position.y, _targetPos.z);
            }
        }
        if ((_droneTrans.position - _targetPos).magnitude <= _interactionDist)
        {
            if (_lookForCharge)
            {
                //      if drone dist to station is under maxDist 
                //          -> start charging up
                DockDrone();
            }
            else if (_movingToNewTile)
            {
                StartCoroutine(PurifyCycle());
            }
        }
        else
        {
            _droneTrans.position = Time.deltaTime * _droneSpeed * (_droneTrans.position - _targetPos).normalized;
        }
    }

    private IEnumerator PurifyCycle()
    {
        TileData targetTile = MapManager.Instance.AccessTileByPos(_targetPos);

        _isPurifying = true;
        while (!targetTile.IsPurified)
        {
            for (int x = -1; x <= 1; ++x)
                for (int z = -1; z <= 1; ++z)
                {
                    TileData tile = MapManager.Instance.AccessTileByPos(new Vector3(_targetPos.x + x, 0, _targetPos.z + z));
                    if (!_tiles.Contains(tile)) continue;

                    targetTile.AddPurify((_maxPurifiedTileVal / _timeUntilPurified) * Time.deltaTime);

                    if (tile.IsPurified)
                        _tiles.Remove(tile);
                }
            yield return null;
        }
        _hasPurifiedAll = _tiles.Count == 0;

        yield return null;
    }




    #region Charge

    public void DockDrone()
    {
        _isDroneDocked = true;
        _rechargeStation.DockDrone();
    }

    public void UndockDrone()
    {
        _isDroneDocked = false;
    }
    #endregion
}
