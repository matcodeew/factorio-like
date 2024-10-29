using UnityEngine;


public class TileData : MonoBehaviour
{
    public int ID;
    public int TileObstacle;
    public bool IsOccupied;
    public GameObject OnTop;
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

//public enum TileObstacle
//{

//}
