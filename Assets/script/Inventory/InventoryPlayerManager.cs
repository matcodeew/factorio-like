using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPlayerManager : MonoBehaviour
{
    public static InventoryPlayerManager Instance;

    [Header("Inventory")]
    [SerializeField] private List<RessourceData> _cheatAllInventoryRessource = new();
    public RectTransform _inventoryParentSlot;
    public List<InvRessource> SlotGameobjectList = new();
    [SerializeField] public GameObject _globalPrefab;
    [SerializeField] public GameObject EmptyPrefab;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        foreach (RessourceData ressource in _cheatAllInventoryRessource)
        {
            ressource.Id = ressource.Ressources.Id;
            for(int i = 0; i < 100; i++)
            {
                CreateNewInventorySlot(ressource);
            }
        }
    }
    public void CreateNewInventorySlot(RessourceData ressources)
    {
        bool _itemFounded = false;

        foreach (var item in SlotGameobjectList)
        {
            if (ressources.Id == item.Ressource.Id)
            {
                _itemFounded = true;
                item.Quantity += ressources.StartQuantity;
                break;
            }
        }
        if (!_itemFounded)
        {
            GameObject newSlot = Instantiate(_globalPrefab, _inventoryParentSlot.transform);
            InvRessource invRessource = newSlot.transform.GetChild(0).AddComponent<InvRessource>();
            SlotGameobjectList.Add(invRessource);
            invRessource.Ressource = ressources.Ressources;
            newSlot.transform.GetChild(0).GetComponent<Image>().sprite = ressources.Ressources.Sprite;
            invRessource.Quantity = 1;
        }
    }




}
public class InvRessource : MonoBehaviour
{
    public Scriptable_Ressources Ressource;
    public int Quantity;
}