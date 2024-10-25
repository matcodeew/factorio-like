using UnityEngine;


public class TileData : MonoBehaviour
{
    public int ID;
    public bool IsOccupied;
    public TileState State = TileState.Contaminated;
    public float ProgressValue = 0;
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
