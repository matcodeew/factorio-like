using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private Dictionary<Vector3, Chunck> Chuncks = new Dictionary<Vector3, Chunck>();

    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private List<Scriptable_RessourceSpot> _allRessorceSpot;
    [SerializeField] private GameObject _groundParent;

    public static MapManager Instance;

    private int _id;
    private int _mapSize = 32;
    private int _chunckSize = 4;
    private List<TileData> OccupedTiles = new();
    private RessourseSpot SpotResource;
    private bool PickingRessource = false;

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

                GetRandomObstacleOnTile(newTile.GetComponent<TileData>());

                int i = x / _chunckSize;
                int j = y / _chunckSize;
                Vector3 chunkKey = new Vector3(i, 0, j);

                if (!Chuncks.ContainsKey(chunkKey))
                    Chuncks.Add(chunkKey, new Chunck());

                Chuncks[chunkKey].TileChunk.Add(new Vector3(x, 0, y), newTile.GetComponent<TileData>());
                _id++;
            }
        }
        AddRessourceOnTile();
    }
    public Chunck AccessChunkByTilePos(Vector3 _clikedPos)
    {
        int i = Mathf.FloorToInt(_clikedPos.x) / _chunckSize;
        int j = Mathf.FloorToInt(_clikedPos.z) / _chunckSize;
        Vector3 chunkPos = new Vector3(i, 0, j);
        if(Chuncks.TryGetValue(chunkPos, out Chunck chunk))
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


    private void GetRandomObstacleOnTile(TileData _thisTile)
    {

            _thisTile.TileObstacle = Random.Range(0, 10);
        

        if (!OccupedTiles.Contains(_thisTile))
            OccupedTiles.Add(_thisTile);
    }
    private void AddRessourceOnTile()
    {
        List<GameObject> spot = new();
        int _id = 0;
        foreach (TileData tile in OccupedTiles)
        {
            if (tile.TileObstacle == 1 && spot.Count < _allRessorceSpot[0].MaxOnMap)
            {
                tile.IsOccupied = true;
                GameObject newSpot = Instantiate(_allRessorceSpot[0].Prefab); //a remplacer
                spot.Add(newSpot);
                newSpot.transform.position = tile.transform.position + new Vector3(0, 1, 0);
                AccessTileByPos(newSpot.transform.position).OnTop = newSpot;
                newSpot.name = _allRessorceSpot[0].Name + " " + _id; // a remplacer
                newSpot.transform.SetParent(_groundParent.transform);
                newSpot.GetComponent<RessourseSpot>().SetAllParameter(_allRessorceSpot[0]);
                _id++;
            }
        }
    }


    public void CheckRessourceOnClick(Vector3 _clikedTarget)
    {
        if (AccessTileByPos(_clikedTarget).IsOccupied)
        {
            RessourseSpot newSpot = AccessTileByPos(_clikedTarget).OnTop.GetComponent<RessourseSpot>();

            if (newSpot != null)
            {
                StartCoroutine(StillRessource(newSpot));
            }
        }
    }
    private IEnumerator StillRessource(RessourseSpot _ressourceSpot)
    {
        yield return new WaitForSeconds(_ressourceSpot.MiningTime);
        _ressourceSpot.AvailableResource[_ressourceSpot.PickRandomRessource()].Quantity--;

    }







}