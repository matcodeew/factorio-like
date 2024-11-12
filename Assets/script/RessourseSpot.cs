using System.Collections.Generic;
using UnityEngine;

public class RessourseSpot : MonoBehaviour
{
    public float MiningTime;
    public int Quantity;
    public Scriptable_RessourceSpot Spot;
    public List<RessourceData> AvailableResource = new();

    private void Awake()
    {
        AddToList();
    }
    public int PickRandomRessource()
    {
        int totalQuantity = 0;
        foreach(var ressource in Spot.AvailableResource)
        {
            if(ressource.StartQuantity > 0)
            {
                totalQuantity += ressource.StartQuantity;
            }
        }
        if(totalQuantity == 0) return -1;

        int randomValue = Random.Range(0, totalQuantity);
        int cumulative = 0;
        foreach (var ressource in Spot.AvailableResource)
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
            AvailableResource.Add(new RessourceData(a.Id, a.Ressources, a.StartQuantity));
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