using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public List<GameObject> AllBuilding = new();
    public static BuildingManager Instance;



    public GameObject SolarPanelPrefab;
    public GameObject FurnacePrefab;
    private GameObject _buildingInProgress;

        
    private void Awake()
    {
        if(Instance == null) {
            Instance = this;
        }
    }

    private void Start()
    {
        AllBuilding.Add(CreateSolarPanel(new Vector3(5, 1, 5)));
        AllBuilding.Add(CreateFurnace(new Vector3(7, 1.5f, 5)));
        AllBuilding.Add(CreateFurnace(new Vector3(5, 1.5f, 7)));
    }

    public GameObject CreateSolarPanel(Vector3 _pos)
    {
        return _buildingInProgress = Instantiate(SolarPanelPrefab, _pos, Quaternion.identity);
    }
    public GameObject CreateFurnace(Vector3 _pos)
    {
        return _buildingInProgress = Instantiate(FurnacePrefab, _pos, Quaternion.identity);
    }
}
