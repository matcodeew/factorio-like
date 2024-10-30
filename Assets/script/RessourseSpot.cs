using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class UseRessourceData
{
    public int ID;
    public Scriptable_Ressources Ressource;
    public int Quantity;
    public UseRessourceData(int _id, Scriptable_Ressources _ressources, int _startQuantity)
    {
        ID = _id;
        Ressource = _ressources;
        Quantity = _startQuantity;
    }
}


public class RessourseSpot : MonoBehaviour
{
    public string Name;
    public GameObject Prefab;
    public float MiningTime;
    public int MaxOnMap;
    [SerializeField] public List<UseRessourceData> AvailableResource;

    public void SetAllParameter(Scriptable_RessourceSpot RessourceSpot)
    {
        Name = RessourceSpot.Name;
        Prefab = RessourceSpot.Prefab;
        MiningTime = RessourceSpot.MiningTime;
        MaxOnMap = RessourceSpot.MaxOnMap;

        AvailableResource = new List<UseRessourceData>();
        foreach (var ressource in RessourceSpot.AvailableResource)
        {
            AvailableResource.Add(new UseRessourceData(ressource.Id, ressource.Ressources, ressource.StartQuantity));
        }
    }
    public int PickRandomRessource()
    {
        int totalQuantity = 0;
        foreach(var ressource in AvailableResource)
        {
            if(ressource.Quantity > 0)
            {
                totalQuantity += ressource.Quantity;
            }
        }
        if(totalQuantity == 0) return -1;

        int randomValue = Random.Range(0, totalQuantity);
        int cumulative = 0;
        foreach (var ressource in AvailableResource)
        {
            cumulative += ressource.Quantity;
            if (randomValue < cumulative)
            {
                return ressource.ID;
            }
        }
        return -1;
    }
}