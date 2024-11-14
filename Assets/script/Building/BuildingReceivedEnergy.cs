using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class BuildingReceivedEnergy : MonoBehaviour, ISubscrireEvent
{
    public List<GenerateEnergy> ConnectGenerator = new();
    public bool IsLinked;
    [SerializeField, ReadOnly(true)] private int _sumOfEnergyReceived;

    private BuildingManager instance;
    public void SubscrireEvent()
    {
        instance = BuildingManager.Instance;
        instance.UpdateBuildingEnergy += CalculateSumOfEnergy;
        CalculateSumOfEnergy();
    }
    private void CalculateSumOfEnergy()
    {
        _sumOfEnergyReceived = 0;
        if (ConnectGenerator.Count > 0)
        {
            _sumOfEnergyReceived = ConnectGenerator.Sum(gen => gen.Energy / gen.TransformBuildingConnected.Count);
            Debug.Log($"Total energy received by {gameObject.name} is : {_sumOfEnergyReceived}");
        }
        else { Debug.Log($"No Generator connect to {gameObject.name}"); }
    }
    private void DestroyTransformer()
    {
        foreach (var gen in ConnectGenerator)
        {
            if (gen.TransformBuildingConnected.Contains(this))
                gen.TransformBuildingConnected.Remove(this);
            foreach (GameObject link in gen._linkConnect)
            {
                if (link.GetComponent<LineRenderer>().GetPosition(1) + new Vector3(0, 0.5f, 0) == transform.position)
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
            Destroy(gameObject);
            DestroyTransformer();
        }
    }
}
