using UnityEngine;


public class TileData : MonoBehaviour
{
    public int ID;
    public bool isOccupied;
    public TileState state;
    public float progressValue = 0;
}


public enum TileState
{
    contaminated,
    cleaned
}
