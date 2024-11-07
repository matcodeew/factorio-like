using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public interface IBehaviour
{
    void Execute();
    void UpdateBuilding();
    void SetComponent(GameObject _object);
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


    public void SetComponent(GameObject _object)
    {
        GenerateEnergy = _object.GetComponent<GenerateEnergy>();
    }
    public void Execute()
    {
    }
    public void UpdateBuilding()
    {
        if (GenerateEnergy != null)
        {
            GenerateEnergy.Timer();
        }
        else
            Debug.LogWarning("GenerateEnergy est null ");
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