using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GenerateEnergy : MonoBehaviour
{
    private float _targetTime = 5.0f;
    public List<Building> LinkBuilding = new List<Building>();
    public int MaxConnection = 1;
    private bool isActive = false;
    private void ActivePanel()
    {
        isActive = !isActive;
        transform.GetChild(0).gameObject.SetActive(isActive);
    }
    public void Timer()
    {
        _targetTime -= Time.deltaTime;
        if(_targetTime <= 0.0f)
        {
            print("endTimer");
            _targetTime = 5.0f;
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

    public void IncrementList(GameObject go)
    {
        if(CanIncrementList())
            LinkBuilding.Add(go.GetComponent<Building>());
    }
    public bool CanIncrementList()
    {
        return LinkBuilding.Count < MaxConnection;
    }
}
