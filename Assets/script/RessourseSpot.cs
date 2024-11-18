using System.Collections.Generic;
using UnityEngine;

public class RessourseSpot : MonoBehaviour
{
    public float MiningTime;
    public Scriptable_RessourceSpot Spot;
    public Dictionary<int, RessourceData> AvailableResource = new();
    public bool IsEmpty = false;
    private void Start()
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
        foreach(var ressource in Spot.AvailableResource)
        {
            RessourceData newRessource = new RessourceData(ressource.Ressources.Id, ressource.Ressources, ressource.StartQuantity);
            AvailableResource.Add(ressource.Ressources.Id , newRessource);
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