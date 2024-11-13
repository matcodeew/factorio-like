using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingRessourceAccessor : MonoBehaviour
{
    public MachineType MachineType;
   // public List<Scriptable_Ressources> TransformationList;
    [SerializeField, Min(0.0f)] public float ProcessTime;
}
public enum MachineType
{
    Grinder,
    Disassembler,
    Furnace,
    None
}