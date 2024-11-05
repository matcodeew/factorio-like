using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GenerateEnergy : MonoBehaviour
{
    private float _targetTime = 5.0f;
    public List<GameObject> LinkBuilding = new List<GameObject>();
    private bool isActive = false;
    private bool GetComponentForFirstTime = true;
    private EnergyGenerator _energyGenerator;

    private void GetComponentOneTime()
    {
        if(GetComponentForFirstTime)
        {
            _energyGenerator = GetComponent<Building>().GetBehaviour<EnergyGenerator>(); ////// descente a 5 fps.
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
    public void IncrementList(GameObject go)
    {
        if (CanIncrementList())
            LinkBuilding.Add(go);
    }
    public bool CanIncrementList()
    {
        return LinkBuilding.Count < _energyGenerator.MaxConnection;
    }
    public void TransferEnergy()
    {
        foreach(GameObject link in LinkBuilding)
        {
            TransformRessources transformRessource = link.GetComponent<Building>().GetBehaviour<TransformRessources>();
            transformRessource.ReceivedEnergy = _energyGenerator.GenerateRandomEnergy() / LinkBuilding.Count;
            print("the energy transferred is : " + transformRessource.ReceivedEnergy);
        }
    }


    public void OnMouseDown()
    {
        ActivePanel();
    }
    private void Update()
    {
        Timer();
    }
}