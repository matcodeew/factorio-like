using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [HideInInspector] public List<GameObject> AllBuilding = new();
    [HideInInspector] public static BuildingManager Instance;

    public bool CanDestroyBuilding = false;

    [Header("Energy Value")]
    [Range(0.0f, 100.0f)] public int SolarForcePercent;
    [SerializeField] private int SolarPanelEnergy;
    [Range(0.0f, 100.0f)] public int WindForcePercent;
    [SerializeField] private int WindTurbineEnergy;
    private GameObject _buildingInProgress;


    private int _furnaceId = 0;
    private int _solarPanelId = 0;
    private bool _isUpdatingEnergy;
    //Event
    public Action UpdateSharedEnergy;
    public Action UpdateBuildingEnergy;
    private void Awake()
    {
        if(Instance == null) {
            Instance = this;
        }
    }
    private void Start()
    {
        StartCoroutine(PeriodicEnergyUpdate(15));
        UpdateSharedEnergy?.Invoke();
    }

    public int GenerateSolarEnergy()
    {
        float RandEnergy = UnityEngine.Random.Range(SolarPanelEnergy/* - 5*/, SolarPanelEnergy);
        int EnergyGenerated = (int)Mathf.Round(RandEnergy * SolarForcePercent / 100);
        return EnergyGenerated;
    }
    public int GenerateWindEnergy()
    {
        float RandEnergy = UnityEngine.Random.Range(WindTurbineEnergy/* - 5*/, WindTurbineEnergy);
        int EnergyGenerated = (int)Mathf.Round(RandEnergy * WindForcePercent / 100);
        return EnergyGenerated;
    }
    //public void CreateBuilding()
    //{
    //    //create SolarPanel
    //    AllBuilding.Add(CreateSolarPanel(new Vector3(0, 1, 2)));
    //    AllBuilding.Add(CreateSolarPanel(new Vector3(5, 1, 2)));
    //    AllBuilding.Add(CreateSolarPanel(new Vector3(10, 1, 2)));

    //    //Create Furnace
    //    AllBuilding.Add(CreateFurnace(new Vector3(0, 1f, 10)));
    //    AllBuilding.Add(CreateFurnace(new Vector3(5, 1f, 10)));
    //    AllBuilding.Add(CreateFurnace(new Vector3(10, 1f, 10)));
    //}
    //public GameObject CreateSolarPanel(Vector3 _pos)
    //{
    //    _buildingInProgress = Instantiate(SolarPanelPrefab, _pos, Quaternion.identity);
    //    _buildingInProgress.name = "SolarPanel " + _solarPanelId;
    //    MapManager.Instance.AccessTileByPos(_pos).OnTop = _buildingInProgress;
    //    _solarPanelId++;
    //    return _buildingInProgress;
    //}
    //public GameObject CreateFurnace(Vector3 _pos)
    //{
    //    _buildingInProgress = Instantiate(FurnacePrefab, _pos, Quaternion.identity);
    //    _buildingInProgress.name = "Furnace " + _furnaceId;
    //    MapManager.Instance.AccessTileByPos(_pos).OnTop = _buildingInProgress;
    //    _furnaceId++;
    //    return _buildingInProgress;
    //}

    private IEnumerator PeriodicEnergyUpdate(float _time)
    {
        _isUpdatingEnergy = true;
        while (_isUpdatingEnergy)
        {
            yield return new WaitForSeconds(_time);
            UpdateSharedEnergy?.Invoke();
        }
        yield return null;
    }
    public void CanDestroyOBject()
    {
        CanDestroyBuilding = !CanDestroyBuilding;
    }
}
