using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IComportement
{
    void Execute();
    void AddRightComponent(GameObject _object);
}

public class EnergyGenerator : IComportement
{
    public int GeneratedEnergy = 30;
    public GenerateEnergy GenerateEnergy;
    public EnergyGenerator(int _energy)
    {
        GeneratedEnergy = _energy;
    }
    public void Execute()
    {
        //GeneratedEnergy.UpdateEnergyValue();
    }
    public void AddRightComponent(GameObject _object)
    {
        GenerateEnergy = _object.AddComponent<GenerateEnergy>();
    }
}
public class TransformRessources : IComportement
{
    public int ReceivedEnergy;
    public int NeededEnergy;
    public RessourceTransformer RessourceTransformer;
    public TransformRessources(int receivedEnergy, int neededEnergy)
    {
        ReceivedEnergy = receivedEnergy;
        NeededEnergy = neededEnergy;
    }
    public void AddRightComponent(GameObject _object)
    {
        RessourceTransformer = _object.AddComponent<RessourceTransformer>();
    }
    public void Execute()
    {
        if(ReceivedEnergy >= NeededEnergy)
        {
            RessourceTransformer.StartTransformation();
        }
    }
}

[System.Serializable]
public class Building : MonoBehaviour
{
    public IComportement Comportement;
    public GameObject Prefab;
    public string Name;

    public Building(string _name, GameObject _prefab, IComportement _comportement)
    {
        Name = _name;
        Prefab = _prefab;
        Comportement = _comportement;
    }
    public void ExecuteComportement()
    {
        Comportement.Execute();
    }
    public GameObject CreateBuilding(Building _buildingWantToCreate, Vector3 _position)
    {
        GameObject newGo = Instantiate(_buildingWantToCreate.Prefab);
        newGo.AddComponent<Building>();
        MapManager.Instance.AccessTileByPos(_position).OnTop = newGo;
        newGo.transform.position = _position;
        newGo.name = Name;

        Comportement.AddRightComponent(newGo);


        return newGo;
    }
}