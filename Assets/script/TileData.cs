using UnityEngine;


public class TileData : MonoBehaviour
{
    public int ID;
    public bool IsOccupied;
    public GameObject OnTop;
    public TileState State = TileState.Contaminated;
    public TileObstacle TileObstacle = TileObstacle.None;
    public float ProgressValue = 0;
    public ChunkState ChunkState;

    //private void OnMouseEnter()
    //{

    //}

    //private void OnMouseExit()
    //{

    //}
}

public enum TileState
{   
    Contaminated,
    Cleaned,
    None,
}

public enum TileObstacle
{
    Dumpster,
    building,
    career,
    tree,
    rock,
    None,
}
