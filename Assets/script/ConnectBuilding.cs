using System.Collections;
using System.Collections.Generic;
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

    private void Update()
    {
        if(UpdateSecondPos)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                SecondPos = MapManager.Instance.AccessTileByPos(hit.point).transform.position + new Vector3(0,1,0);
                if(lineRenderer != null)
                {
                    lineRenderer.SetPosition(1, SecondPos);
                }
                if (MapManager.Instance.AccessTileByPos(SecondPos).OnTop != null && Input.GetMouseButtonDown(0))
                {
                    print("Someone on top");
                    UpdateSecondPos = false;
                }
            }
        }
    }
}
