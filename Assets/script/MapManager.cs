
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    private Dictionary<Vector3, Chunk> Chunks = new Dictionary<Vector3, Chunk>();

    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private GameObject _groundParent;

    public static MapManager Instance;
    public bool PickingRessource;

    private int _id;
    private int _mapSize = 32;
    private int _chunckSize = 8;
    private RessourseSpot _currentMiningSpot;
    private int _currentPlacedSpot;

    [Header("Map Ressource Spot")]
    [SerializeField] private List<GameObject> _prefabToInstantiate;
    private List<TileData> _spotToRandomize = new();
    [SerializeField] private int _maxSpotOnMap;

    [Header("Win Conditions")]
    [SerializeField] private Image _ProgressBarDecontamination;
    [SerializeField] private TextMeshProUGUI _ProgressBarValue;
    [SerializeField] private GameObject _endShipGameObject;
    public Action TogglePurifyViewEvent;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void Start()
    {
        _groundParent.transform.position = Vector3.zero;
        SetChunck();
    }

    #region Map Creator

    private void SetChunck()
    {
        for (int x = 0; x < _mapSize; x++)
        {
            for (int y = 0; y < _mapSize; y++)
            {
                GameObject newTile = Instantiate(_tilePrefab);
                newTile.transform.position = new Vector3(x, 0, y);
                newTile.transform.parent = _groundParent.transform;
                newTile.name = "Tile (" + x + " , " + y + ")";
                newTile.GetComponent<TileData>().ID = _id;
                AlocateTileState(newTile.GetComponent<TileData>());

                int i = x / _chunckSize;
                int j = y / _chunckSize;
                Vector3 chunkKey = new Vector3(i, 0, j);

                if (!Chunks.ContainsKey(chunkKey))
                {
                    Chunks.Add(chunkKey, new Chunk());
                }

                Chunks[chunkKey].Tiles.Add(new Vector3(x, 0, y), newTile.GetComponent<TileData>());
                _id++;
            }
        }
        PlaceRandomBuildings();
        UpdateProgressBar();
    }

    private void AlocateTileState(TileData tile)
    {
        int randomValue = UnityEngine.Random.Range(0, 100);

        if (randomValue < 94) // 94 % de chance
        {
            tile.TileState = TileBuildingOnTop.None;
            return;
        }
        else if (randomValue < 96)
        {
            tile.TileState = TileBuildingOnTop.Dumpster;
        }
        else if (randomValue < 98)
        {
            tile.TileState = TileBuildingOnTop.RuinedBuilding;
        }
        else 
        {
            tile.TileState = TileBuildingOnTop.AbandonedPowerStation;
        }
        _spotToRandomize.Add(tile);
    }
    private void PlaceRandomBuildings()
    {
        int count = Mathf.Min(_spotToRandomize.Count, _maxSpotOnMap);
        var randomElements = _spotToRandomize.OrderBy(x => Guid.NewGuid()).Take(count).ToList();
        List<GameObject> objectPlaced = new();
        foreach (var tile in randomElements)
        {
            if (objectPlaced.Any(spot => spot.transform.position == tile.transform.position))
                continue;

            int index = (int)tile.TileState;
            if (index <= _prefabToInstantiate.Count - 1)
            {
                GameObject newGo = Instantiate(_prefabToInstantiate[index]);
                newGo.transform.parent = _groundParent.transform;
                newGo.transform.position = tile.transform.position + new Vector3(0, 1, 0);
                tile.OnTop = newGo;
                tile.IsOccupied = true;
                objectPlaced.Add(newGo);
            }
        }
        _spotToRandomize.Clear();
    }


    #endregion

    #region Tile & Chunk Acessors

    public Chunk AccessChunkByTilePos(Vector3 _clikedPos)
    {
        int i = Mathf.FloorToInt(_clikedPos.x) / _chunckSize;
        int j = Mathf.FloorToInt(_clikedPos.z) / _chunckSize;
        Vector3 chunkPos = new Vector3(i, 0, j);
        if (Chunks.TryGetValue(chunkPos, out Chunk chunk))
            return chunk;
        else return null;
    }
    public TileData AccessTileByPos(Vector3 _clikedPos)
    {
        Chunk _chunkSelect = AccessChunkByTilePos(_clikedPos);

        int x = Mathf.FloorToInt(_clikedPos.x);
        int z = Mathf.FloorToInt(_clikedPos.z);
        Vector3 tilePos = new Vector3(x, 0, z);

        if (_chunkSelect.Tiles.TryGetValue(tilePos, out TileData tileData))
            return tileData;
        else return null;
    }

    #endregion

    #region Gather Ressources

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
                if (_ressourceSpot.AvailableResource[resourceIndex].StartQuantity > 0)
                {
                    RessourceData ressourceTransfered = new RessourceData(_ressourceSpot.AvailableResource[resourceIndex].Id,
                        _ressourceSpot.AvailableResource[resourceIndex].Ressources, 1);

                    InventoryPlayerManager.Instance.CreateNewInventorySlot(ressourceTransfered);
                    _ressourceSpot.AvailableResource[resourceIndex].StartQuantity--;
                }
                else
                {
                    PickingRessource = false;
                    _currentMiningSpot = null;
                    StopAllCoroutines();
                }
            }
            else
            {
                Destroy(_ressourceSpot.gameObject);
                _ressourceSpot.IsEmpty = true;
                AccessTileByPos(_ressourceSpot.transform.position).IsOccupied = false;
                AccessTileByPos(_ressourceSpot.transform.position).OnTop = null;
                break;
            }
        }
    }

    #endregion

    #region Chunk Update

    public void UpdateChunkByTilePos(Vector3 tilePos)
    {
        // Receive the pos of the modified tile
        // call the update func of the chunck in which the tile is stored
        AccessChunkByTilePos(tilePos).UpdateChunkDecontaminationValue();
        UpdateProgressBar();
    }

    private void UpdateProgressBar()
    {
        // iterate through all the chunks and average the purified value
        float decontaminationValue = Chunks.Values.Average(chunk => chunk.ChunckDecontaminationValue); // value that vary between 0.0 and 1.0

        _ProgressBarDecontamination.fillAmount = decontaminationValue;
        _ProgressBarValue.text = (decontaminationValue * 100.0f).ToString("0.0") + "%";

        if (decontaminationValue >= 1.0f)
        {
            _endShipGameObject.SetActive(true);
        }
    }


    public void FireTogglePurifyViewEvent()
    {
        // Fire an event that will be listened by every tiles
        TogglePurifyViewEvent?.Invoke();
    }
    #endregion
}