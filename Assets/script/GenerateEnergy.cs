using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GenerateEnergy : MonoBehaviour
{
    private float _targetTime = 5.0f;
    public List<Building> LinkBuilding = new List<Building>();
    private bool isActive = false;
    private bool GetComponentForFirstTime = true;
    private EnergyGenerator _energyGenerator;

    private void GetComponentOneTime()
    {
        if(GetComponentForFirstTime)
        {
            _energyGenerator = GetComponent<Building>().GetBehaviour<EnergyGenerator>();
            GetComponentForFirstTime = false;
        }
    }
    public void ActivePanel()
    {
        GetComponentOneTime();
        isActive = !isActive;
        transform.GetChild(0).gameObject.SetActive(isActive);
    }
    public void Timer()
    {
        _targetTime -= Time.deltaTime;
        if(_targetTime <= 0.0f)
        {
            TransferEnergy();
            _targetTime = 5.0f;
        }
    }
    public void IncrementList(Building building)
    {
        if(CanConnectBuilding() && !LinkBuilding.Contains(building))
        { 
            LinkBuilding.Add(building);
        } 
    }
    public bool CanConnectBuilding() { return LinkBuilding.Count < _energyGenerator.MaxConnection; }
    public void TransferEnergy()
    {
        int EnergyTranfered = _energyGenerator.GenerateRandomEnergy();
        foreach(Building link in LinkBuilding)
        {
            TransformRessources transformRessource = link.GetBehaviour<TransformRessources>();
            transformRessource.ReceivedEnergy = EnergyTranfered / LinkBuilding.Count;
            print("the energy transferred is : " + transformRessource.ReceivedEnergy);
        }
    }
    public void OnMouseDown()
    {
        ActivePanel();
    }
}