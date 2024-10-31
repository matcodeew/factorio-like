using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "newRessources", menuName = "Data/New Ressources")]
[System.Serializable]
public class Scriptable_Ressources : ScriptableObject
{
    public int Id;
    public string Name;
    public Sprite Sprite;
    public bool IsPure;
    public Scriptable_Ressources GrinderOutput;
    public Scriptable_Ressources FurnaceOutput;
    public List<Scriptable_Ressources> DisassemblerOutputs;
}
