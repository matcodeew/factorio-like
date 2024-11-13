using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UnityEditor.Progress;
using static BuildingData;

public class InventoryBuildManager : MonoBehaviour
{
    public static InventoryBuildManager Instance;

    [SerializeField] private RectTransform _panelBuildingParent;
    private RectTransform BuildingPrefabItemsParent;

    public RectTransform TrashArea;

    public GameObject InventoryButton;
    public GameObject TrashAreaGameObject;
    public GameObject InventoryBuildPanel;
    public GameObject BuildStatPanel;
    public GameObject BuildInventoryButton;

    [SerializeField] private List<GameObject> _buildingPrefabs = new List<GameObject>();
    [SerializeField] private GameObject _inventoryCasePrefab;

    [SerializeField] private List<GameObject> _invSlots = new List<GameObject>();
    private CanvasGroup _uiCanvasGroup;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {   
        InitializeBuildInventory();

        if (_uiCanvasGroup == null)
        {
            _uiCanvasGroup = InventoryBuildPanel.gameObject.AddComponent<CanvasGroup>();
        }
    }

    private void InitializeBuildInventory()
    {
        for (int i = 0; i < _buildingPrefabs.Count; i++)
        {
            GameObject itemInstance = Instantiate(_inventoryCasePrefab, _panelBuildingParent);

            if (itemInstance.transform.childCount > 0)
            {
                GameObject child = itemInstance.transform.GetChild(0).gameObject;

                DragUIElementBuild dragComponent = child.AddComponent<DragUIElementBuild>();
                dragComponent.Initialize(_buildingPrefabs[i], child, itemInstance);
            }

            _invSlots.Add(itemInstance);
        }
    }

    public void CreateObjectOnMap(GameObject prefab, Vector3 position)
    {
        if (!MapManager.Instance.AccessTileByPos(position).IsOccupied)
        {
            GameObject newGo = Instantiate(prefab, position, Quaternion.identity, BuildingPrefabItemsParent);
            MapManager.Instance.AccessTileByPos(position).OnTop = newGo;
            MapManager.Instance.AccessTileByPos(position).IsOccupied = true;
        }
    }

    public void FadeUIElement(float targetAlpha)
    {
        if (_uiCanvasGroup != null)
        {
           _uiCanvasGroup.alpha = targetAlpha;
        }
    }
    private void CanBuild()
    {

    }
    public void UpdateBuildingCase()
    {
        //List<Scriptable_Ressources> _ressourceNeed = new();
        //List<Scriptable_Ressources> _stillRessource = new();

        //foreach(var Building in _prefabItems)
        //{
        //    BuildingRessourceAccessor data = Building?.GetComponent<BuildingRessourceAccessor>();
        //    if (data != null)
        //    {
        //        foreach(var ItemNeed in data.RessourceNeedToBuild)
        //        {
        //            _ressourceNeed.Add(ItemNeed);
        //        }
        //        foreach(var ItemHave in InventoryPlayerManager.Instance.SlotGameobjectList)
        //        {
        //            foreach(var ItemNeed in _ressourceNeed)
        //            {
        //                if(ItemNeed == ItemHave.Ressource)
        //                {
        //                    _stillRessource.Add(ItemNeed);
        //                    break; // si il trouve la ressource
        //                }
        //            }
        //        }
        //    }
        //    if (_stillRessource.Count == _ressourceNeed.Count)
        //    {
        //        print($"can build {Building.name}");
        //    }
        //    else { print($"cant build {Building.name}"); }
        //}












        Dictionary<Scriptable_Ressources, int> invRessources = new();
        foreach (InvRessource ressource in InventoryPlayerManager.Instance.SlotGameobjectList)
        {
               invRessources.Add(ressource.Ressource, ressource.Quantity);
        }
        for (int i = 0; i < _buildingPrefabs.Count; i++)
        {
            BuildingData data = _buildingPrefabs[i].GetComponent<BuildingData>();

            bool canBuild = true;
            foreach (NeededRessource neededRessource in data.NeededRessources)
            {
                if (!invRessources.ContainsKey(neededRessource.Ressource))
                {
                    canBuild = false;
                    break;
                }
                canBuild &= invRessources[neededRessource.Ressource] >= neededRessource.Quantity;
            }

            _invSlots[i].transform.GetChild(1).gameObject.SetActive(!canBuild);

            Debug.Log("COULD" + (canBuild ? "" : "N'T") + " BUILD " + data.Name);
        }
    }
}
