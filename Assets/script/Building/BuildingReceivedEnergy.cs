using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BuildingData))]
public class BuildingReceivedEnergy : MonoBehaviour, ISubscrireEvent
{
    public List<GenerateEnergy> ConnectGenerator = new();
    public bool IsLinked;
    private float _sumOfEnergyReceived;

    private bool _isPowered = false;
    private BuildingData _buildingData;

    private BuildingManager instance;

    [ContextMenu("Test/Setup Energy")]
    public void SubscrireEvent()
    {
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
                if (link.GetComponent<LineRenderer>().GetPosition(1) == transform.position)
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
        if (instance.CanDestroyBuilding)
        {
            InventoryBuildManager.Instance.AddOnDestroy(this.gameObject);
            Destroy(gameObject);
            DestroyTransformer();
        }
    }
}
