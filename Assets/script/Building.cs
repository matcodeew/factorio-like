using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public interface IBehaviour
{
    void Execute();
}

public class EnergyGenerator : IBehaviour
{
    private int EnergyMinimum;
    private int EnergyMaximum;
    public int EnergyGenerated { get; private set; }
    public int MaxConnection { get; private set; }
    public GenerateEnergy GenerateEnergy;
    public EnergyGenerator(int _maxConnection, int _energyminimum, int energyMaximum)
    {
        MaxConnection = _maxConnection;
        EnergyMinimum = _energyminimum;
        EnergyMaximum = energyMaximum;
    }
    public int GenerateRandomEnergy() => EnergyGenerated = Random.Range(EnergyMinimum, EnergyMaximum);

    public void Execute()
    {
        //GeneratedEnergy.UpdateEnergyValue();
    }
}

public class TransformRessources : IBehaviour
{
    public RessourceTransformer RessourceTransformer;
    public int ReceivedEnergy{ get; set; }
    public int NeededEnergy { get; private set; }
    public TransformRessources(int receivedEnergy, int neededEnergy)
    {
        ReceivedEnergy = receivedEnergy;
        NeededEnergy = neededEnergy;
    }

    public void Execute()
    {
        if(ReceivedEnergy >= NeededEnergy)
        {
            RessourceTransformer.StartTransformation();
        }
    }
}

public class Building : MonoBehaviour
{
    public IBehaviour Behaviour;
    public GameObject Prefab;
    public string Name;

    public void Initialize(string _name, GameObject _prefab, IBehaviour _behaviour)
    {
        Name = _name;
        Prefab = _prefab;
        Behaviour = _behaviour;
    }

    public T GetBehaviour<T>() where T : class, IBehaviour /////////////
    {
        return Behaviour as T;
    }

    public GameObject CreateBuilding(Building _buildingWantToCreate, Vector3 _position, GameObject newGo) //////////////
    {
        MapManager.Instance.AccessTileByPos(_position).OnTop = newGo;
        newGo.transform.position = _position;
        newGo.name = Name;
        return newGo;
    }

    public void ExecuteComportement()
    {
        Behaviour.Execute();
    }
    public void DebugComportement()
    {
        if (Behaviour == null)
        {
            Debug.LogWarning($"No comportement assigned to Building '{Name}'.");
        }
        else
        {
            Debug.Log($"Building '{Name}' has a comportement of type: {Behaviour.GetType()}");
        }
    }
}