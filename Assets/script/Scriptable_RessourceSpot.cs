using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newRessourceSpot", menuName = "Data/New Ressources Spot")]
[System.Serializable]
public class Scriptable_RessourceSpot : ScriptableObject
{
    public List<Scriptable_Ressources> AvailableResource;
    public int Quantity;
    public int MiningTime;
}
