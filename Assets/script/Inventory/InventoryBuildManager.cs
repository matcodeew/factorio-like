using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static BuildingData;

public class InventoryBuildManager : MonoBehaviour
{
    public static InventoryBuildManager Instance;
    [SerializeField] public Transform ImageParentDrag;
    [SerializeField] private RectTransform _panelBuildingParent;
    [SerializeField] private RectTransform _parentToparent;
    [SerializeField] private RectTransform _PrefabPanelInfo;
    private RectTransform BuildingPrefabItemsParent;

    public RectTransform TrashArea;

    public GameObject InventoryButton;
    public GameObject TrashAreaGameObject;
    public GameObject InventoryBuildPanel;
    // public GameObject BuildStatPanel;

    public List<GameObject> _buildingPrefabs = new List<GameObject>();
    public List<Sprite> _buildingImage = new List<Sprite>();
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
            itemInstance.tag = "Build";
            RectTransform rectTransform = Instantiate(_PrefabPanelInfo, _parentToparent);
            rectTransform.gameObject.tag = "Build";
            rectTransform.transform.position = itemInstance.transform.position;
            TooltipScript.instance.objectsToTooltip.Add(rectTransform, _buildingPrefabs[i]);
            if (itemInstance.transform.childCount > 0)
            {
                GameObject child = itemInstance.transform.GetChild(0).gameObject;
                DragUIElementBuild dragComponent = child.AddComponent<DragUIElementBuild>();
                dragComponent.Initialize(_buildingPrefabs[i], child, itemInstance);
            }
            itemInstance.GetComponent<Image>().sprite = _buildingImage[i];
            _invSlots.Add(itemInstance);
        }
    }

    public void CreateObjectOnMap(GameObject prefab, Vector3 position, BuildingData data)
    {
        if (!MapManager.Instance.AccessTileByPos(position).IsOccupied)
        {
            ReduceRessource(data);
            GameObject newGo = Instantiate(prefab, position, Quaternion.identity, BuildingPrefabItemsParent);
            MapManager.Instance.AccessTileByPos(position).OnTop = newGo;
            MapManager.Instance.AccessTileByPos(position).IsOccupied = true;
            newGo.GetComponent<ISubscrireEvent>().SubscrireEvent();
        }
    }

    public void ReduceRessource(BuildingData buildingData)
    {
        Dictionary<Scriptable_Ressources, int> invRessources = InventoryPlayerManager.Instance.SlotGameobjectList
           .ToDictionary(ressource => ressource.Ressource, ressource => ressource.Quantity);

        foreach (NeededRessource needed in buildingData.NeededRessources)
        {
            if (invRessources.ContainsKey(needed.Ressource))
            {
                invRessources[needed.Ressource] -= needed.Quantity;
            }
        }

        foreach (InvRessource ressource in InventoryPlayerManager.Instance.SlotGameobjectList)
        {
            if (invRessources.ContainsKey(ressource.Ressource))
            {
                ressource.Quantity = invRessources[ressource.Ressource];
            }
        }
        UpdateBuildingCase();
    }

    public void AddOnDestroy(GameObject buildingData)
    {
        Dictionary<Scriptable_Ressources, int> invRessources = InventoryPlayerManager.Instance.SlotGameobjectList
           .ToDictionary(ressource => ressource.Ressource, ressource => ressource.Quantity);

        foreach (NeededRessource needed in buildingData.GetComponent<BuildingData>().NeededRessources)
        {
            if (invRessources.ContainsKey(needed.Ressource))
            {
                invRessources[needed.Ressource] += needed.Quantity;
            }
        }
        foreach (InvRessource ressource in InventoryPlayerManager.Instance.SlotGameobjectList)
        {
            if (invRessources.ContainsKey(ressource.Ressource))
            {
                ressource.Quantity = invRessources[ressource.Ressource];
            }
        }

        RessourceTransformer ressourceTransformer = buildingData?.GetComponent<RessourceTransformer>();
        if (ressourceTransformer != null)
        {
            foreach (var needed in ressourceTransformer.TakeRessourceFromInput())
            {
                InventoryPlayerManager.Instance.CreateNewInventorySlot(new RessourceData(needed.Ressource.Id, needed.Ressource, needed.Quantity));
            }
        }
        UpdateBuildingCase();
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
