using System.Collections.Generic;
using UnityEngine;

public class InventoryPlayerManager : MonoBehaviour
{
    [SerializeField] private RectTransform _inventoryParentSlot; 

    [SerializeField] private List<Scriptable_Ressources> _scripts = new List<Scriptable_Ressources>(); 
    [SerializeField] private List<GameObject> _objects = new List<GameObject>(); 
    [SerializeField] private List<GameObject> _inventoryPlayerSlots = new List<GameObject>(); 

    void Start()
    {
        InitializePlayerInventory();
    }

    private void InitializePlayerInventory()
    {
        for (int i = 0; i < _scripts.Count; i++)
        {
            GameObject selectedPrefab = _inventoryPlayerSlots[i % _inventoryPlayerSlots.Count]; 
            GameObject itemInstance = Instantiate(selectedPrefab, _inventoryParentSlot);
            InvRessource ressourceComponent = itemInstance.AddComponent<InvRessource>();
            ressourceComponent.Ressource = _scripts[i]; 
            ressourceComponent.Quantity = i + 1; 
            _objects.Add(itemInstance); 
        }
    }
}

public class InvRessource : MonoBehaviour
{
    public Scriptable_Ressources Ressource; 
    public int Quantity; 
}
