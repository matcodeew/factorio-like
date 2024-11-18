using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class GenerateEnergy : MonoBehaviour, ISubscrireEvent
{
    public List<BuildingReceivedEnergy> TransformBuildingConnected = new List<BuildingReceivedEnergy>();
    public List<GameObject> _linkConnect = new List<GameObject>();
    [SerializeField, Min(1.0f)] public int MaxConnection = 2;
    public int Energy;
    private BuildingManager instance;
    private bool _isActive;
    public bool CanConnectBuilding() { return TransformBuildingConnected.Count < MaxConnection; }
    public void SubscrireEvent()
    {
        instance = BuildingManager.Instance;
        instance.UpdateSharedEnergy += UpdateEnergy;
    }
    private void UpdateEnergy()
    {
        //if(CompareTag("SolarPanel"))
        //{
        //    Energy = instance.GenerateSolarEnergy();
        //}
        //else
        //{
        //    Energy = instance.GenerateWindEnergy();
        //}
        Energy = instance.GenerateSolarEnergy();
        //Debug.Log($"energy send by {gameObject.name} is : {Energy}");   
        instance.UpdateBuildingEnergy?.Invoke();
    }
    public void AddGenToList()
    {
        foreach(BuildingReceivedEnergy energy in TransformBuildingConnected)
        {
            if(!energy.ConnectGenerator.Contains(this))
            {
                energy.ConnectGenerator.Add(this);
                //instance.UpdateSharedEnergy?.Invoke();
            }
        }
    }
    public void OnMouseDown()
    {
        //ActivePanel();
        if(instance.CanDestroyBuilding)
        {
            InventoryBuildManager.Instance.AddOnDestroy(this.gameObject);
            Destroy(gameObject);
            DestroyGenerator();
        }
    }
    public void ActivePanel()
    {
        _isActive = !_isActive;
        transform.GetChild(0).gameObject.SetActive(_isActive);
    }

    private void DestroyGenerator()
    {
        foreach(var transformer in TransformBuildingConnected)
        {
            if(transformer.ConnectGenerator.Contains(this))
            {
                transformer.ConnectGenerator.Remove(this);
            }

            foreach(GameObject link in _linkConnect)
            {
                if(_linkConnect.Contains(link))
                {
                    _linkConnect.Remove(link);
                    Destroy(link);
                }
                break;
            }
        }
        instance.UpdateBuildingEnergy?.Invoke();
        instance.UpdateSharedEnergy -= UpdateEnergy;
    }
}
