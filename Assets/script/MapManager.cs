
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
    [SerializeField] private GameObject _dumpsterPrefab;

    public static MapManager Instance;
    public bool PickingRessource;
    private bool _createDumpster = true;

    private int _id;
    private int _mapSize = 32;
    private int _chunckSize = 8;
    private RessourseSpot _currentMiningSpot;
    private int _currentPlacedSpot;

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
                CreateDumpster(newTile.GetComponent<TileData>());

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
        UpdateProgressBar();
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















    // public void UpdateProgressBar()
    // {
    //     _ProgressBarDecontamination.fillAmount = CalculPourcentageDecontaminationChunck();
    //     float progressPercentage = Mathf.Round(_ProgressBarDecontamination.fillAmount * 100);
    //     _ProgressBarValue.text = progressPercentage.ToString() + "%";
    //     if (_ProgressBarDecontamination.fillAmount >= 1) // ou 100%
    //     {
    //         _endShipGameObject.SetActive(true);
    //     }
    // }

    // public float CalculPourcentageDecontaminationChunck()
    // {
    //     float totalDecontamination = 0;

    //     foreach (Chunck chunck in Chuncks.Values)
    //     {
    //         float chunkDecontamination = 0;
    //         // Additionne la valeur de d�contamination pour chaque tuile du chunk
    //         foreach (TileData tileData in chunck.TileChunk.Values)
    //         {
    //             chunkDecontamination += tileData.ProgressValue;
    //         }

    //         // Calcule la moyenne de d�contamination pour ce chunk
    //         float AverrageChunckDecontamination = chunkDecontamination / chunck.TileChunk.Count;
    //         // Ajoute la moyenne du chunk au total
    //         totalDecontamination += AverrageChunckDecontamination;
    //     }

    //     // Calcule la moyenne sur tous les chunks et retourne en pourcentage
    //     return (totalDecontamination / Chuncks.Count) / 100;
    // }

    #endregion
}