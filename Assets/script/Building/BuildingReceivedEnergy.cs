using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class BuildingReceivedEnergy : MonoBehaviour
{
    public List<GenerateEnergy> ConnectGenerator = new();
    public bool IsLinked;
    [SerializeField, ReadOnly(true)] private int _sumOfEnergyReceived;

    private void Start()
    {
        BuildingManager.Instance.UpdateBuildingEnergy += CalculateSumOfEnergy;
        CalculateSumOfEnergy();
    }

    private void CalculateSumOfEnergy()
    {
        if(ConnectGenerator.Count > 0)
        {
            _sumOfEnergyReceived = ConnectGenerator.Sum(gen => gen.Energy / gen.TransformBuildingConnected.Count);
            Debug.Log($"Total energy received by {gameObject.name} is : {_sumOfEnergyReceived}");
        }
        else { Debug.Log($"No Generator connect to {gameObject.name}"); }
    }
}
