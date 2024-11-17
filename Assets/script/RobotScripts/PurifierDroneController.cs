using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurifierDroneController : DroneController
{
    private Transform _droneTrans;


    [Header("Drone Stats")]
    [SerializeField] private float _droneSpeed = 5.0f;
    [SerializeField] private int _droneManhattanRange = 0;
    [SerializeField] private float _interactionDist = 0.5f;
    private bool _movingToNewTile = false;


    [Header("Purifier Variables")]
    [SerializeField] private List<TileData> _tiles = new List<TileData>();
    [SerializeField] private Vector3 _targetPos = Vector3.zero;
    [SerializeField] private float _timeUntilPurified = 0.0f;
    private float _maxPurifiedTileVal = 1.0f;
    private bool _isPurifying = false;
    private bool _hasPurifiedAll = false;

    protected override void InitDroneSpecialized()
    {
        _droneTrans = transform;
        InitTileList();
    }

    private void InitTileList()
    {
        for (int x = -_droneManhattanRange; x <= _droneManhattanRange; x++)
            for (int z = -_droneManhattanRange; z <= _droneManhattanRange; z++)
            {
                if ((x + z) <= _droneManhattanRange)
                {
                    Vector3 test = new Vector3(_rechargeStationTrans.position.x + x, 0, _rechargeStationTrans.position.z + z);
                    TileData newTile = MapManager.Instance.AccessTileByPos(test);
                    if (newTile != null && !_tiles.Contains(newTile) && !newTile.IsPurified)
                        _tiles.Add(newTile);
                }
            }
        _hasPurifiedAll = _tiles.Count == 0;
    }



    void FixedUpdate()
    {
        // if drone is docked -> charging
        if (_isDroneDocked || _hasPurifiedAll || !_isDroneInit) return;

        // drone power consumption 
        CurrentDroneCharge = Mathf.Clamp
            (
                CurrentDroneCharge - ((_droneMaxCharge / _timeUntilEmpty) * Time.deltaTime),
                0.0f,
                _droneMaxCharge
            );

        if (_isPurifying) return;

        if (!_lookForCharge)
        {
            if (CurrentDroneCharge <= _rechargeThreshold)
            {
                // if drone charge is under 5% and is not docked 
                //      -> target = recharge station
                _lookForCharge = true;
                _movingToNewTile = false;

                _targetPos = _rechargeStationTrans.position;
                _targetPos = new Vector3(_targetPos.x, _droneTrans.position.y, _targetPos.z);
            }
            else if (!_movingToNewTile)
            {
                // if drone has not set a new target
                //      sets a new target to random from tiles list
                _movingToNewTile = true;

                _targetPos = _tiles[Random.Range(0, _tiles.Count - 1)].transform.position;
                _targetPos = new Vector3(_targetPos.x, _droneTrans.position.y, _targetPos.z);
            }
        }


        if ((_targetPos - _droneTrans.position).magnitude <= _interactionDist)
        {
            if (_lookForCharge)
            {
                // if drone dist to station is under maxDist 
                //     -> start charging up
                DockDrone();
            }
            else if (_movingToNewTile)
            {
                _movingToNewTile = false;
                StartCoroutine(PurifyCycle());
            }
        }
        else
        {
            // go to target
            _droneTrans.position = Time.deltaTime * _droneSpeed * (_targetPos - _droneTrans.position).normalized + _droneTrans.position;
        }
    }

    private IEnumerator PurifyCycle()
    {
        TileData targetTile = MapManager.Instance.AccessTileByPos(_targetPos);

        _isPurifying = true;
        while (!targetTile.IsPurified)
        {
            if (CurrentDroneCharge <= _rechargeThreshold)
            {
                break;
            }
            for (int x = -1; x <= 1; ++x)
                for (int z = -1; z <= 1; ++z)
                {
                    TileData tile = MapManager.Instance.AccessTileByPos(new Vector3(_targetPos.x + x, 0, _targetPos.z + z));
                    if (tile == null) continue;

                    tile.AddPurify((_maxPurifiedTileVal / _timeUntilPurified) * Time.deltaTime);

                    if (tile.IsPurified)
                        _tiles.Remove(tile);
                }
            yield return null;
        }
        _hasPurifiedAll = _tiles.Count == 0;

        _isPurifying = false;
        yield return null;
    }
}
