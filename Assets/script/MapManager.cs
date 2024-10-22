using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private Dictionary<Vector2, Chunck> _chuncks = new Dictionary<Vector2, Chunck>();
    [SerializeField] private GameObject _TilePrefab;
    public static MapManager Instance;

    private int _mapSize = 32;
    private int _chunckSize = 4;

    private void Start()
    {
        SetChunck();
    }

    private void SetChunck()
    {
        for(int x = 0; x < _mapSize - 1; x++)
        {
            for (int y = 0; y < _mapSize - 1; y++)
            {
                GameObject newTile = Instantiate(_TilePrefab);
                newTile.transform.position = new Vector3(x, 0, y);

                int i = x / _chunckSize;
                int j = y / _chunckSize;

                if (!_chuncks.ContainsKey(new Vector2(i, j)))
                    _chuncks.Add(new Vector2(i,j), new Chunck());

                _chuncks[new Vector2(i, j)]._tileInChunk.Add(new Vector2(x, y), newTile.GetComponent<TileData>());
            }
        }
    }

    public Chunck GetChunckThanksToTile(TileData _useTile)
    {
        if(_useTile!=null)
        {
            if(_chuncks.ContainsKey(_useTile.transform.position))
            {
                return _chuncks[_useTile.transform.position];
            }
        }
        return null;
    }
    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.y = 100f;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        

        if(Input.GetMouseButtonDown(0))
        {
        }
    }
}
