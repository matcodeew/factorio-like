using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class ConnectBuilding : MonoBehaviour
{
    [SerializeField] private GameObject _electricLink;
    private bool _updateSecondPos;
    private LineRenderer _lineRenderer;
    private GameObject _currentLine;
    private Vector3 _firstPos, _secondPos;
    private bool _isActive = false;
    private bool _getComponentForFirstTime = true;
    private EnergyGenerator _energyGenerator;

    private float _targetTime = 10.0f;
    private float _currentTime;

    public List<Building> LinkTransformationBuilding = new List<Building>();

    public void Timer()
    {
        _currentTime -= Time.deltaTime;
        if (_currentTime <= 0.0f)
        {
            if(CanStartTransferEnergy())
            {
                _energyGenerator.GenerateRandomEnergy();
                _currentTime = _targetTime;
            }
        }
    }
    public void ConnectLink()
    {
        ActivePanel();
        _currentLine = Instantiate(_electricLink);
        _lineRenderer = _currentLine.GetComponent<LineRenderer>();

        _firstPos = this.transform.position;
        _lineRenderer.SetPosition(0, _firstPos);

        _updateSecondPos = true;
    }
    private void Update()
    {
        if (_updateSecondPos)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                _secondPos = SnapToGrid(hit.point);
                if (_lineRenderer != null)
                {
                    _lineRenderer.SetPosition(1, _secondPos);
                }
                if (Input.GetMouseButtonDown(0))
                {
                    if(CheckTransformationBuilding(hit))
                    {
                        _updateSecondPos = false;
                    }
                    else
                    {
                        _updateSecondPos = false;
                        Destroy(_currentLine);
                    }
                }
            }
        }
    }

    private Vector3 SnapToGrid(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x);
        int z = Mathf.FloorToInt(position.z);
        return new Vector3(x, 1, z);
    }
    private bool CheckTransformationBuilding(RaycastHit hit)
    {
        GameObject _onTop = MapManager.Instance.AccessTileByPos(hit.point).OnTop;
        if (_onTop != null)
        {
            Building _buildOnTop = _onTop.GetComponent<Building>();
            if(CanConnectBuilding())
            {
                if (!LinkTransformationBuilding.Contains(_buildOnTop))
                {
                    LinkTransformationBuilding.Add(_buildOnTop);
                    _buildOnTop.GetBehaviour<TransformRessources>().LinkedBuilding.Add(this.GetComponent<Building>().GetBehaviour<EnergyGenerator>());
                    return _onTop.GetComponent<RessourceTransformer>() != null;
                }
                else { print("the building already has this connection"); }
            }
            else { print("the building has too many connections"); }
        }
        return false;
    }
    public bool CanConnectBuilding() { return LinkTransformationBuilding.Count < _energyGenerator.MaxConnection; }
    public bool CanStartTransferEnergy() { return LinkTransformationBuilding.Count > 0; }

    public void OnMouseDown()
    {
        ActivePanel();
    }
    public void ActivePanel()
    {
        if (_getComponentForFirstTime)
        {
            _energyGenerator = GetComponent<Building>().GetBehaviour<EnergyGenerator>();
            _getComponentForFirstTime = false;
        }
        _isActive = !_isActive;
        transform.GetChild(0).gameObject.SetActive(_isActive);
    }
}