using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnergy : MonoBehaviour
{
    public List<BuildingReceivedEnergy> TransformBuildingConnected = new List<BuildingReceivedEnergy>();
    public int MaxConnection;
    public int Energy;

    private BuildingManager instance;
    private bool _isActive;
    public bool CanConnectBuilding() { return TransformBuildingConnected.Count < MaxConnection; }

    private void Start()
    {
        instance = BuildingManager.Instance;
        instance.UpdateSharedEnergy += UpdateEnergy;
    }

    private void UpdateEnergy()
    {
        Energy = instance.GenerateEnergy();
        Debug.Log($"energy send by {gameObject.name} is : {Energy}");
        instance.UpdateBuildingEnergy?.Invoke();
    }
    public void AddGenToList()
    {
        foreach(BuildingReceivedEnergy energy in TransformBuildingConnected)
        {
            if(!energy.ConnectGenerator.Contains(this))
            {
                energy.ConnectGenerator.Add(this);
                instance.UpdateSharedEnergy?.Invoke();
            }
        }
    }
    public void OnMouseDown()
    {
        ActivePanel();
    }
    public void ActivePanel()
    {
        _isActive = !_isActive;
        transform.GetChild(0).gameObject.SetActive(_isActive);
    }
}
