using System.Collections.Generic;
using UnityEngine;

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

   [SerializeField] private List<GameObject> _prefabItems = new List<GameObject>(); 
   [SerializeField] private List<GameObject> _inventoryCases = new List<GameObject>();

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
        for (int i = 0; i < _inventoryCases.Count; i++)
        {
            GameObject itemInstance = Instantiate(_inventoryCases[i], _panelBuildingParent);

            if (itemInstance.transform.childCount > 0)
            {
                GameObject child = itemInstance.transform.GetChild(0).gameObject;

                DragUIElementBuild dragComponent = child.AddComponent<DragUIElementBuild>();
                dragComponent.Initialize(_prefabItems[i], child, itemInstance);
            }
        }
    }

    public void CreateObject(GameObject prefab, Vector3 position)
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
}
