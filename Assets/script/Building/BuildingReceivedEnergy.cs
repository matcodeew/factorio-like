using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BuildingData))]
public class BuildingReceivedEnergy : MonoBehaviour, ISubscrireEvent
{
    public List<GenerateEnergy> ConnectGenerator = new();
    public bool IsLinked;
    [SerializeField] private float _sumOfEnergyReceived;
    private bool _isPowered = false;
    private BuildingData _buildingData;

    private BuildingManager instance;

    [ContextMenu("Test/Setup Energy")]
    public void SubscrireEvent()
    {
        RobotStation? _station = GetComponent<RobotStation>();
        if(_station != null)
        {
            _station.Init();
        }
        _buildingData = GetComponent<BuildingData>();
        instance = BuildingManager.Instance;
        instance.UpdateBuildingEnergy += CalculateSumOfEnergy;
        CalculateSumOfEnergy();
    }
    public void CalculateSumOfEnergy()
    {
        _sumOfEnergyReceived = 0;
        if (ConnectGenerator.Count > 0)
        {
            _sumOfEnergyReceived = ConnectGenerator.Sum(gen => gen.Energy / gen.TransformBuildingConnected.Count);
            Debug.Log($"Total energy received by {gameObject.name} is : {_sumOfEnergyReceived}");
        }
        else { Debug.Log($"No Generator connect to {gameObject.name}"); }

        _isPowered = _sumOfEnergyReceived >= _buildingData.NeededEnergy;
    }

    public bool IsPowered()
    {
        return _isPowered;
    }



    private void DestroyTransformer()
    {
        foreach (var gen in ConnectGenerator)
        {
            if (gen.TransformBuildingConnected.Contains(this))
                gen.TransformBuildingConnected.Remove(this);
            foreach (GameObject link in gen._linkConnect)
            {
                Vector3 LinkNormalizePos = new Vector3(link.GetComponent<LineRenderer>().GetPosition(1).x,
                    0, (link.GetComponent<LineRenderer>().GetPosition(1).z));
                Vector3 NormalizePos = new Vector3(transform.position.x, 0, transform.position.z);

                if (LinkNormalizePos == NormalizePos)
                {
                    gen._linkConnect.Remove(link);
                    Destroy(link);
                    break;
                }
            }
        }
        instance.UpdateBuildingEnergy -= CalculateSumOfEnergy;
        instance.UpdateSharedEnergy?.Invoke();
    }
    private void OnMouseDown()
    {
        if (instance != null) { 
            if (instance.CanDestroyBuilding)
            {
                InventoryBuildManager.Instance.AddOnDestroy(this.gameObject);
                Destroy(gameObject);
                DestroyTransformer();
            } }
    }
}
