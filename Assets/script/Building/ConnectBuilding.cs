using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectBuilding : MonoBehaviour
{
    [SerializeField] private GameObject _electricLink;
    private bool _updateSecondPos;
    private LineRenderer _lineRenderer;
    private GenerateEnergy _generateEnergy;
    private GameObject _currentLine;

    private Vector3 FirstPos, SecondPos;

    private void Awake()
    {
        _generateEnergy = GetComponent<GenerateEnergy>();
    }
    private void Update()
    {
        UpdateLineRendererSecondPos();
    }
    public void ConnectLink()
    {
        _generateEnergy.ActivePanel();
        _currentLine = Instantiate(_electricLink);
        _lineRenderer = _currentLine.GetComponent<LineRenderer>();

        FirstPos = this.transform.position;
        _lineRenderer.SetPosition(0, FirstPos);

        _updateSecondPos = true;
    }
    private void UpdateLineRendererSecondPos()
    {
        if(_updateSecondPos)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                SecondPos = SnapToGrid(hit.point);
                if(_lineRenderer != null)
                {
                    _lineRenderer.SetPosition(1, SecondPos);
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
        GameObject onTop = MapManager.Instance.AccessTileByPos(hit.point).OnTop;
        if(onTop != null && _generateEnergy != null)
        {
            RessourceTransformer ressourceTransformer = onTop.GetComponent<RessourceTransformer>();
            if(_generateEnergy.CanConnectBuilding() && ressourceTransformer != null)
            {
                _generateEnergy.TransformBuildingConnected.Add(onTop.GetComponent<BuildingReceivedEnergy>());
                _generateEnergy.AddGenToList();
                _generateEnergy._linkConnect.Add(_currentLine);
                return true; 
            }
            else { print("the building has too many connections"); }
        }
        return false;
    }
}