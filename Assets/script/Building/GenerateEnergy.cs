using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateEnergy : MonoBehaviour
{
    public List<BuildingReceivedEnergy> TransformBuildingConnected = new List<BuildingReceivedEnergy>();
    public int MaxConnection;
    private bool _isActive;

    public bool CanConnectBuilding() { return TransformBuildingConnected.Count < MaxConnection; }
    public void AddGenToList()
    {
        foreach (BuildingReceivedEnergy energy in TransformBuildingConnected)
        {
            if (!energy.ConnectGenerator.Contains(this))
            {
                energy.ConnectGenerator.Add(this);
            }
        }
    }
    public void ActivePanel()
    {
        _isActive = !_isActive;
        transform.GetChild(0).gameObject.SetActive(_isActive);
    }
    private void OnMouseDown()
    {
        ActivePanel();
    }
}
