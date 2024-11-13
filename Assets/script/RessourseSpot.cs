using System.Collections.Generic;
using UnityEngine;

public class RessourseSpot : MonoBehaviour
{
    public float MiningTime;
    public int Quantity;
    public Scriptable_RessourceSpot Spot;
    public Dictionary<int, RessourceData> AvailableResource = new();

    private void Awake()
    {
        AddToList();
    }
    public int PickRandomRessource()
    {
        int totalQuantity = 0;
        foreach(var ressource in AvailableResource.Values)
        {
            if(ressource.StartQuantity > 0)
            {
                totalQuantity += ressource.StartQuantity;
            }
        }
        if(totalQuantity == 0) return -1;

        int randomValue = Random.Range(0, totalQuantity);
        int cumulative = 0;
        foreach(var ressource in AvailableResource.Values)
        {
            cumulative += ressource.StartQuantity;
            if (randomValue < cumulative)
            {
                return ressource.Id;
            }
        }
        return -1;
    }

    private void AddToList()
    {
        foreach(var a in Spot.AvailableResource)
        {
            RessourceData newRessource = new RessourceData(a.Ressources.Id, a.Ressources, a.StartQuantity);
            AvailableResource.Add(a.Ressources.Id , newRessource);
        }
    }
}

[System.Serializable]
public class RessourceData
{
    public int Id;
    public Scriptable_Ressources Ressources;
    public int StartQuantity;

    public RessourceData(int id,Scriptable_Ressources ressources,int Quantity)
    {
        this.Id = id;
        this.Ressources = ressources;
        this.StartQuantity = Quantity;
    }
}