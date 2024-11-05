using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class ConnectBuilding : MonoBehaviour
{
    [SerializeField] private GameObject _electricLink;
    private bool _updateSecondPos;
    private LineRenderer _lineRenderer;
    private GenerateEnergy _generateEnergy;
    private GameObject _currentLine;

    public Vector3 FirstPos;
    public Vector3 SecondPos;
    public void ConnectLink()
    {
        _generateEnergy = GetComponentInParent<GenerateEnergy>();

        _currentLine = Instantiate(_electricLink);
        _lineRenderer = _currentLine.GetComponent<LineRenderer>();

        FirstPos = this.transform.position;
        _lineRenderer.SetPosition(0, FirstPos);
        _lineRenderer.SetPosition(1, FirstPos); 

        _updateSecondPos = true;
    }

    private void Update()
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
                if(Input.GetMouseButtonDown(0))
                {                    
                    if(CheckTransformationBuilding(hit) && _generateEnergy.CanIncrementList())
                    {
                        print("building that needs electricity");
                        _updateSecondPos = false;
                    }
                    else
                    {
                        print("no building or building that doesn't need electricity");
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
        if (onTop != null && _generateEnergy != null)
        {
            _generateEnergy.IncrementList(onTop);
            return onTop.GetComponent<RessourceTransformer>() != null;
        }
        return false;
    }
}