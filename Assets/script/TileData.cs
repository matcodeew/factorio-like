using UnityEngine;


public class TileData : MonoBehaviour
{
    public int ID;
    public bool isOccupied;
    public TileState state = TileState.contaminated;
    public float progressValue = 0;

    public TileState UpdateState()
    {
        if(progressValue >= 100)
        {
            return state = TileState.cleaned;
        }
        return state = TileState.contaminated;
    }
}

public enum TileState
{
    contaminated,
    cleaned,
    none,
}
