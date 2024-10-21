using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Dictionary<Vector3, TileData> tiles = new Dictionary<Vector3, TileData>();
    private PlayerController playerRay;
    private void Awake()
    {
        List<TileData> list = new List<TileData>(FindObjectsOfType<TileData>());
        
        for (int i = 0; i < list.Count; i++)
        {
            TileData data = list[i];
            data.ID = i;
            tiles.Add(data.transform.position, data);
        }
    }

    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 100f;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        

        if(Input.GetMouseButtonDown(0))
        {
        }
    }
}
