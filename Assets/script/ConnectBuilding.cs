using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class ConnectBuilding : MonoBehaviour
{
    [SerializeField] private GameObject ElectricLink;
    public Vector3 FirstPos;
    public Vector3 SecondPos;
    private bool UpdateSecondPos;
    private LineRenderer lineRenderer;

    public void ConnectLink()
    {
        GameObject go = Instantiate(ElectricLink);
        lineRenderer = go.GetComponent<LineRenderer>();

        FirstPos = this.transform.position;
        lineRenderer.SetPosition(0, FirstPos);
        lineRenderer.SetPosition(1, FirstPos); 

        UpdateSecondPos = true;
    }

    private void OnMouseDown()
    {

    }

    private void Update()
    {
        if(UpdateSecondPos)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                SecondPos = SnapToGrid(hit.point);
                if(lineRenderer != null)
                {
                    lineRenderer.SetPosition(1, SecondPos);
                }
                if(Input.GetMouseButtonDown(0))
                {                    
                    if(CheckTransformationBuilding(hit))
                    {
                        print("Someone on top");
                        UpdateSecondPos = false;
                    }
                    else
                    {
                        print("nothing on top");
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
        if(MapManager.Instance.AccessTileByPos(hit.point).OnTop != null)
        {
            RessourceTransformer? transformer = MapManager.Instance.AccessTileByPos(hit.point).OnTop.GetComponent<RessourceTransformer>();
            bool RightBuilding = (transformer != null) ? true : false;
            return RightBuilding;
        }
        return false;
    }
}
