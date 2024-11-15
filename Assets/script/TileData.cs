using UnityEngine;


public class TileData : MonoBehaviour
{
    public int ID;
    public bool IsOccupied;
    public GameObject OnTop;
    public ChunkState ChunkState;


    [SerializeField, Range(0.0f, 1.0f)] public float PurifiedValue; // value that vary between 0.0 and 1.0
    [SerializeField] private Material _floorMat;
    [SerializeField] private bool _purifiedView = false;
    public bool IsPurified = false;

    void Start()
    {
        gameObject.GetComponent<Renderer>().material = new Material(_floorMat);
        _floorMat = gameObject.GetComponent<Renderer>().material;

        _floorMat.SetFloat("_PurifiedValue", PurifiedValue);
        _floorMat.SetInt("_IsPurified", IsPurified ? 1 : 0);
        _floorMat.SetInt("_IsInPurifiedView", _purifiedView ? 1 : 0);

        MapManager.Instance.TogglePurifyViewEvent += TogglePurifyView;
    }

    public void AddPurify(float value)
    {
        PurifiedValue = Mathf.Clamp(PurifiedValue + value, 0.0f, 1.0f);
        _floorMat.SetFloat("_PurifiedValue", PurifiedValue);

        if (PurifiedValue == 1.0f)
        {
            IsPurified = true;
            _floorMat.SetInt("_IsPurified", PurifiedValue == 1.0f ? 1 : 0);
        }

        // CALL UPDATE AVERAGE (WIN CON)
        MapManager.Instance.UpdateChunkByTilePos(transform.position);
    }


    void TogglePurifyView()
    {
        _purifiedView = !_purifiedView;
        _floorMat.SetInt("_IsInPurifiedView", _purifiedView ? 1 : 0);
    }
}

