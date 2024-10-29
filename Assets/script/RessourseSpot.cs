using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourseSpot : MonoBehaviour
{
     public string Name;
     public GameObject Prefab;
     public List<RessourceData> AvailableResource;
     public float MiningTime;
     public int MaxOnMap;

    public void SetAllParameter(Scriptable_RessourceSpot RessourceSpot)
    {
        Name = RessourceSpot.Name;
        Prefab = RessourceSpot.Prefab;
        AvailableResource = RessourceSpot.AvailableResource;
        MiningTime = RessourceSpot.MiningTime;
        MaxOnMap = RessourceSpot.MaxOnMap;
    }

    public int PickRandomRessource()
    {
        int totalQuantity = 0;
        foreach (var ressource in AvailableResource)
        {
            totalQuantity += ressource.Quantity;
        }
        int randomValue = Random.Range(0, totalQuantity);
        int cumulative = 0;
        foreach (var ressource in AvailableResource)
        {
            cumulative += ressource.Quantity;
            if (randomValue < cumulative)
            {
                print("name : " + ressource.Ressources.name + " this ID : " + ressource.ID);
                return ressource.ID;
            }
        }
        return 5000;
    }
}