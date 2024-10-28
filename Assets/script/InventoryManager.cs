using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public RectTransform InventoryParent;
    public RectTransform BuildingParent;
    public RectTransform TrashArea;

    public GameObject TrashAreaGameObject;
    public GameObject InventoryPanelHide;

    public List<GameObject> _prefabItems = new List<GameObject>(); 
    public List<GameObject> _inventoryCases = new List<GameObject>();

    private CanvasGroup _uiCanvasGroup;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InitializeInventory();

        if (_uiCanvasGroup == null)
        {
            _uiCanvasGroup = InventoryPanelHide.gameObject.AddComponent<CanvasGroup>();
        }
    }

    private void InitializeInventory()
    {
        for (int i = 0; i < _inventoryCases.Count; i++)
        {
            GameObject itemInstance = Instantiate(_inventoryCases[i], InventoryParent);

            if (itemInstance.transform.childCount > 0)
            {
                GameObject child = itemInstance.transform.GetChild(0).gameObject;

                DragUIElement dragComponent = child.AddComponent<DragUIElement>();
                dragComponent.Initialize(_prefabItems[i], child, itemInstance);
            }
        }
    }

    public void CreateObject(GameObject prefab, Vector3 position)
    {
        if (!MapManager.Instance.AccessTileByPos(position).isOccupied)
        {
            Instantiate(prefab, position, Quaternion.identity, BuildingParent);
            MapManager.Instance.AccessTileByPos(position).isOccupied = true;
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
