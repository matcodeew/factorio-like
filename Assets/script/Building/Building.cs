using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IBehaviour
{
    void Execute();
    void SetComponent(GameObject _object);
    void UpdateBuilding();
}

public class EnergyGenerator : IBehaviour
{
    private int EnergyMinimum;
    private int EnergyMaximum;
    public int EnergyGenerated { get; private set; }
    public int MaxConnection { get; private set; }
    public ConnectBuilding connectBuilding;
    public EnergyGenerator(int _maxConnection, int _energyMinimum, int energyMaximum)
    {
        MaxConnection = _maxConnection;
        EnergyMinimum = _energyMinimum;
        EnergyMaximum = energyMaximum;
    }
    public int GenerateRandomEnergy() => EnergyGenerated = UnityEngine.Random.Range(EnergyMinimum, EnergyMaximum);
    public void SetComponent(GameObject _object)
    {
        connectBuilding = _object.GetComponent<ConnectBuilding>();
    }
    public void Execute()
    {
    }
    public void UpdateBuilding()
    {
        if(connectBuilding != null)
        {
            connectBuilding.Timer();
        }
    }
}

public class TransformRessources : IBehaviour
{
    public RessourceTransformer RessourceTransformer;
    public List<EnergyGenerator> LinkedBuilding = new();
    public int ReceivedEnergy{ get; set; }
    public int NeededEnergy { get; private set; }
    private int CalculateEnergyReceived()
    {
        ReceivedEnergy = 0;
        foreach(var a in LinkedBuilding)
        {
            ReceivedEnergy = LinkedBuilding.Sum(Valeur => Valeur.EnergyGenerated) / a.connectBuilding.LinkTransformationBuilding.Count();
        }
        return ReceivedEnergy;
    }
    public TransformRessources(int neededEnergy)
    {
        NeededEnergy = neededEnergy;
    }
    public void SetComponent(GameObject _object)
    {
        RessourceTransformer = _object.GetComponent<RessourceTransformer>();
    }
    public void Execute()
    {
        if(ReceivedEnergy >= NeededEnergy)
        {
            RessourceTransformer.StartTransformation();
        }
    }
    public void UpdateBuilding()
    {
        Debug.Log("the building " + RessourceTransformer.gameObject.name + "  received : " + CalculateEnergyReceived() + " energy");
        //Debug.Log("the building " + RessourceTransformer.gameObject.name + "  num of building connect : " + LinkedBuilding.Count);
    }
}

public class Building : MonoBehaviour
{
    public IBehaviour Behaviour;
    public string Name;

    public Building Initialize(string _name, GameObject newGo, Vector3 _position, IBehaviour _behaviour)
    {
        newGo.name = _name;
        Behaviour = _behaviour;
        MapManager.Instance.AccessTileByPos(_position).OnTop = newGo;
        newGo.transform.position = _position;
        Behaviour.SetComponent(newGo);
        return this;
    }
    //private void Start()
    //{
    //    if (Behaviour is EnergyGenerator)
    //    {
    //        MapManager.Instance.UpdateSharedEnergy += UpdateBehaviour;
    //    }
    //}

    public T GetBehaviour<T>() where T : class, IBehaviour
    {
        return Behaviour as T;
    }

    public void ExecuteComportement()
    {
        Behaviour.Execute();
    }
    public void UpdateBehaviour()
    {
        Behaviour.UpdateBuilding();
    }
}