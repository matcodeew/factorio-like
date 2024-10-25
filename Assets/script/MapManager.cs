using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public Dictionary<Vector3, Chunck> _chuncks = new Dictionary<Vector3, Chunck>();

    [SerializeField] private GameObject _TilePrefab;
    [SerializeField] private GameObject _GroundParent;
    public static MapManager Instance;

    private int _ID;
    private int _mapSize = 32;
    private int _chunckSize = 4;

    private void Start()
    {
        _GroundParent.transform.position = Vector3.zero;
        SetChunck();
    }

    private void SetChunck()
    {
        for (int x = 0; x < _mapSize; x++)
        {
            for (int y = 0; y < _mapSize; y++)
            {
                GameObject newTile = Instantiate(_TilePrefab);
                newTile.transform.position = new Vector3(x, 0, y);
                newTile.transform.parent = _GroundParent.transform;
                newTile.GetComponent<TileData>().ID = _ID;

                int i = x / _chunckSize;
                int j = y / _chunckSize;
                Vector3 chunkKey = new Vector3(i, 0, j);

                if (!_chuncks.ContainsKey(chunkKey))
                    _chuncks.Add(chunkKey, new Chunck());

                _chuncks[chunkKey]._tileInChunk.Add(new Vector3(x, 0, y), newTile.GetComponent<TileData>());
                _ID++;
            }
        }
    }

    private Chunck AccessChunkByTilePos(Vector3 _clikedPos)
    {
        int x = Mathf.FloorToInt(_clikedPos.x);
        int z = Mathf.FloorToInt(_clikedPos.z);

        int i = x / _chunckSize;
        int j = z / _chunckSize;
        Vector3 chunkPos = new Vector3(i, 0, j);

        if (_chuncks.TryGetValue(chunkPos, out Chunck chunk))
            return chunk;

        Debug.LogWarning("Chunk non trouvé pour la position : " + _clikedPos);
        return null;
    }

    public TileData AccessTileByPos(Vector3 _clikedPos)
    {
        Chunck _chunkSelect = AccessChunkByTilePos(_clikedPos);

        int x = Mathf.FloorToInt(_clikedPos.x);
        int z = Mathf.FloorToInt(_clikedPos.z);
        Vector3 tilePos = new Vector3(x, 0, z);

        if (_chunkSelect != null && _chunkSelect._tileInChunk.TryGetValue(tilePos, out TileData tileData))
            return tileData;

        Debug.LogWarning("Tile non trouvée dans le chunk pour la position : " + _clikedPos);
        return null;
    }
}