using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnergy : MonoBehaviour
{
    private float _targetTime = 5.0f;
    private List<Building> LinkBuilding = new List<Building>();
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
}
