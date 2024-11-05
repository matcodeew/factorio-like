using System.Collections.Generic;
using UnityEngine;

public class InventoryPlayerManager : MonoBehaviour
{
    public static InventoryPlayerManager Instance;

    [SerializeField] private RectTransform _inventoryParentSlot;
    [SerializeField] private Scriptable_RessourceList _scriptableRessourceList;
    [SerializeField] private List<GameObject> _objects = new List<GameObject>();
    [SerializeField] private List<GameObject> _inventoryPlayerSlots = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InitializePlayerInventory();
    }

    private void InitializePlayerInventory()
    {
        for (int i = 0; i < _scriptableRessourceList.RessourceList.Count; i++)
        {
            GameObject selectedPrefab = _inventoryPlayerSlots[i % _inventoryPlayerSlots.Count];
            GameObject itemInstance = Instantiate(selectedPrefab, _inventoryParentSlot);
            InvRessource ressourceComponent = itemInstance.AddComponent<InvRessource>();
            ressourceComponent.Ressource = _scriptableRessourceList.RessourceList[i];
            ressourceComponent.Quantity = 1; 
            _objects.Add(itemInstance);
        }
    }
}


public class InvRessource : MonoBehaviour
{
    public Scriptable_Ressources Ressource;
    public int Quantity;
}
