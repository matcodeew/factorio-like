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

    private int _furnaceId = 0;
    private int _solarPanelId = 0;
        
    private void Awake()
    {
        if(Instance == null) {
            Instance = this;
        }
    }

    public void CreateBuilding()
    {
        //create SolarPanel
        AllBuilding.Add(CreateSolarPanel(new Vector3(1, 1, 2)));
        AllBuilding.Add(CreateSolarPanel(new Vector3(5, 1, 2)));
        AllBuilding.Add(CreateSolarPanel(new Vector3(9, 1, 2)));

        //Create Furnace
        AllBuilding.Add(CreateFurnace(new Vector3(1, 1.5f, 10)));
        AllBuilding.Add(CreateFurnace(new Vector3(5, 1.5f, 10)));
        AllBuilding.Add(CreateFurnace(new Vector3(9, 1.5f, 10)));
    }

    public GameObject CreateSolarPanel(Vector3 _pos)
    {
        _buildingInProgress = Instantiate(SolarPanelPrefab, _pos, Quaternion.identity);
        _buildingInProgress.name = "SolarPanel " + _solarPanelId;
        MapManager.Instance.AccessTileByPos(_pos).OnTop = _buildingInProgress;
        _solarPanelId++;
        return _buildingInProgress;
    }
    public GameObject CreateFurnace(Vector3 _pos)
    {
        _buildingInProgress = Instantiate(FurnacePrefab, _pos, Quaternion.identity);
        _buildingInProgress.name = "Furnace " + _furnaceId;
        MapManager.Instance.AccessTileByPos(_pos).OnTop = _buildingInProgress;
        _furnaceId++;
        return _buildingInProgress;
    }
}
