using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newRessourceList", menuName = "Data/New Ressource List")]
[System.Serializable]
public class Scriptable_RessourceList : ScriptableObject
{
    public List<Scriptable_Ressources> RessourceList;
}
