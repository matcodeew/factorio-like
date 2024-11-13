
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private Dictionary<Vector3, Chunck> Chunks = new Dictionary<Vector3, Chunck>();

    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private List<Scriptable_RessourceSpot> _allRessorceSpot;
    [SerializeField] private GameObject _groundParent;
    [SerializeField] private GameObject _dumpsterPrefab;

    public static MapManager Instance;
    public bool PickingRessource;
    private bool _createDumpster = true;

    private int _id;
    private int _mapSize = 32;
    private int _chunckSize = 6;
    private RessourseSpot _currentMiningSpot;
    private int _currentPlacedSpot;

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
                CreateDumpster(newTile.GetComponent<TileData>());

                int i = x / _chunckSize;
                int j = y / _chunckSize;
                Vector3 chunkKey = new Vector3(i, 0, j);

                if (!Chunks.ContainsKey(chunkKey))
                {
                    Chunks.Add(chunkKey, new Chunck());
                }

                Chunks[chunkKey].TileChunk.Add(new Vector3(x, 0, y), newTile.GetComponent<TileData>());
                _id++;
            }
        }
        AllocatedTileObstacle();
    }

    public void CreateDumpster(TileData tile)
    {
        if (_createDumpster == true)
        {
            GameObject newGo = Instantiate(_dumpsterPrefab);
            newGo.transform.parent = _groundParent.transform;
            newGo.transform.position = tile.transform.position + new Vector3(0, 1.5f, 0);
            tile.OnTop = newGo;
            tile.IsOccupied = true;
            _createDumpster = false;
        }

    }

    private void AllocatedTileObstacle()
    {
        foreach(Chunck chunk in Chunks.Values)
        {
            if(Random.Range(0, 100) < 100 / 4) // une chance sur 4 
            {
                chunk.state = (ChunkState)Random.Range(0, System.Enum.GetValues(typeof(ChunkState)).Length);
            }
            else
                chunk.state = ChunkState.None;
            foreach(TileData tileInChunk in chunk.TileChunk.Values)
            {
                tileInChunk.ChunkState = chunk.state;
            }
        }
    }
    public Chunck AccessChunkByTilePos(Vector3 _clikedPos)
    {
        int i = Mathf.FloorToInt(_clikedPos.x) / _chunckSize;
        int j = Mathf.FloorToInt(_clikedPos.z) / _chunckSize;
        Vector3 chunkPos = new Vector3(i, 0, j);
        if(Chunks.TryGetValue(chunkPos, out Chunck chunk))
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
    public void CheckRessourceOnClick(Vector3 _clikedTarget)
    {
        TileData clickedTile = AccessTileByPos(_clikedTarget);
        if (clickedTile != null && clickedTile.IsOccupied)
        {
            RessourseSpot newSpot = clickedTile.OnTop.GetComponent<RessourseSpot>();

            if (newSpot != null && _currentMiningSpot != newSpot)
            {
                StopAllCoroutines();
                _currentMiningSpot = newSpot;
                StartCoroutine(StillRessource(newSpot));
            }
        }
        else
        {
            StopAllCoroutines();
            PickingRessource = false;
            _currentMiningSpot = null;
        }
    }
    private IEnumerator StillRessource(RessourseSpot _ressourceSpot)
    {
        PickingRessource = true;
        while (PickingRessource && _ressourceSpot != null && _ressourceSpot.Spot.AvailableResource.Count > 0)
        {
            yield return new WaitForSeconds(_ressourceSpot.MiningTime);
            int resourceIndex = _ressourceSpot.PickRandomRessource();
            if (resourceIndex != -1)
            {
                if (_ressourceSpot.AvailableResource.TryGetValue(resourceIndex, out RessourceData ressource))
                {
                    if (ressource.StartQuantity > 0)
                    {
                        InventoryPlayerManager.Instance.CreateNewInventorySlot(ressource);
                        ressource.StartQuantity--;
                    }
                    else
                    {
                        PickingRessource = false;
                        _currentMiningSpot = null;
                        StopAllCoroutines();
                    }
                }
            }
            else
            {
                Destroy(_ressourceSpot.gameObject);
                break;
            }
        }
    }
}