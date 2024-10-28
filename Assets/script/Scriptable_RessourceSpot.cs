using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newRessourceSpot", menuName = "Data/New Ressources Spot")]
[System.Serializable]
public class Scriptable_RessourceSpot : ScriptableObject
{
    public string Name;
    public GameObject Prefab;
    public List<RessourceData> AvailableResource;
    public float MiningTime;

    [System.Serializable]
    public struct RessourceData
    {
        public int ID;
        public Scriptable_Ressources Ressources;
        public int StartQuantity;
    }

    public RessourceData PickRandomRessource()
    {
        int totalQuantity = 0;
        foreach (var ressource in AvailableResource)
        {
            totalQuantity += ressource.StartQuantity;
        }
        int randomValue = Random.Range(0, totalQuantity);
        int cumulative = 0;
        foreach (var ressource in AvailableResource)
        {
            cumulative += ressource.StartQuantity;
            if (randomValue < cumulative)
            {
                return ressource;
            }
        }
        return AvailableResource[0];
    }
}