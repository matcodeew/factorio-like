using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingData : MonoBehaviour
{
    public string Name;
    public List<NeededRessource> NeededRessources;


}

[System.Serializable]
public struct NeededRessource
{
    public Scriptable_Ressources Ressource;
    public int Quantity;
}