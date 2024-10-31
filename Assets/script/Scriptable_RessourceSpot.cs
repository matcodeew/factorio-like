using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "newRessourceSpot", menuName = "Data/New Ressources Spot")]
[System.Serializable]
public class Scriptable_RessourceSpot : ScriptableObject
{
    public string Name;
    public GameObject Prefab;
    public List<RessourceData> AvailableResource;
    public float MiningTime;
    public int MaxOnMap;

    [System.Serializable]
    public struct RessourceData
    {
        public int Id;
        public Scriptable_Ressources Ressources;
        public int StartQuantity;
    }
}