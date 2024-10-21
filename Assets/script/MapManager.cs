using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private List<TileData> tiles;
    private void Awake()
    {
        tiles = new List<TileData>(FindObjectsOfType<TileData>());
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].ID = i;
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);            
        }
    }
}
