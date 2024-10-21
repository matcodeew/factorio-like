using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TileData : MonoBehaviour
{
    public int ID;
    public bool isOccuped;
    public TileState state;
    public float progressValue;
}


public enum TileState
{
    contaminated,
    cleaned
}
