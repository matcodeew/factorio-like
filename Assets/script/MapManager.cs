using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private Dictionary<Vector3, TileData> _tiles = new Dictionary<Vector3, TileData>();
    private int _chunckSize = 4;
    private int _mapSize = 32;
    private void Awake()
    {
        List<TileData> list = new List<TileData>(FindObjectsOfType<TileData>());
        
        for (int i = 0; i < list.Count; i++)
        {
            TileData data = list[i];
            data.ID = i;
            _tiles.Add(data.transform.position, data);
        }
        print("list count : " + list.Count);
    }

    private void AddToChunk()
    {
        for(int y = 0;  y < _mapSize / _chunckSize; y++)
        {
            for(int x = 0;  x < _mapSize / _chunckSize; x++)
            {

            }
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
