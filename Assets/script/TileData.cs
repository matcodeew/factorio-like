using UnityEngine;


public class TileData : MonoBehaviour
{
    public int ID;
    public bool isOccupied;
    public TileState state = TileState.contaminated;
    public float progressValue = 0;
}

public enum TileState
{
    contaminated,
    cleaned,
    none,
}
