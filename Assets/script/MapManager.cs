using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public Dictionary<Vector3, Chunck> Chuncks = new Dictionary<Vector3, Chunck>();

    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private GameObject _groundParent;

    public static MapManager Instance;

    private int _id;
    private int _mapSize = 32;
    private int _chunckSize = 4;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void Start()
    {
        _groundParent.transform.position = Vector3.zero;
        SetChunck();
    }

    private void SetChunck()
    {
        for (int x = 0; x < _mapSize; x++)
        {
            for (int y = 0; y < _mapSize; y++)
            {
                GameObject newTile = Instantiate(_tilePrefab);
                newTile.transform.position = new Vector3(x, 0, y);
                newTile.transform.parent = _groundParent.transform;
                newTile.name = "Tile (" + x + " , "+ y + ")";
                newTile.GetComponent<TileData>().ID = _id;

                int i = x / _chunckSize;
                int j = y / _chunckSize;
                Vector3 chunkKey = new Vector3(i, 0, j);

                if (!Chuncks.ContainsKey(chunkKey))
                    Chuncks.Add(chunkKey, new Chunck());

                Chuncks[chunkKey].TileChunk.Add(new Vector3(x, 0, y), newTile.GetComponent<TileData>());
                _id++;
            }
        }
    }

    public Chunck AccessChunkByTilePos(Vector3 _clikedPos)
    {
        int i = Mathf.FloorToInt(_clikedPos.x) / _chunckSize;
        int j = Mathf.FloorToInt(_clikedPos.z) / _chunckSize;
        Vector3 chunkPos = new Vector3(i, 0, j);
        if (Chuncks.TryGetValue(chunkPos, out Chunck chunk))
            return chunk;
        else return null;
    }
    public TileData AccessTileByPos(Vector3 _clikedPos)
    {
        Chunck _chunkSelect = AccessChunkByTilePos(_clikedPos);

        int x = Mathf.FloorToInt(_clikedPos.x);
        int z = Mathf.FloorToInt(_clikedPos.z);
        Vector3 tilePos = new Vector3(x, 0, z);

        if(_chunkSelect.TileChunk.TryGetValue(tilePos, out TileData tileData))
            return tileData;
        else return null;
    }
}