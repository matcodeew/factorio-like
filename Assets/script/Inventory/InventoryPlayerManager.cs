using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Scriptable_RessourceSpot;

public class InventoryPlayerManager : MonoBehaviour
{
    public static InventoryPlayerManager Instance;
    Scriptable_Ressources Scriptable_Ressources;

    public GameObject _EmptyPrefab;
    public RectTransform _inventoryParentSlot;
    [SerializeField] private Scriptable_RessourceList _scriptableRessourceList;
    [SerializeField] private List<GameObject> _objects = new List<GameObject>();
    [SerializeField] private List<GameObject> _inventoryPlayerSlots = new List<GameObject>();

    [SerializeField] private GameObject _globalPrefab;

    public List<InvRessource> _slotGameobjectList = new();

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
        //for (int i = 0; i < _scriptableRessourceList.RessourceList.Count; i++)
        //{
        //    GameObject selectedPrefab = _inventoryPlayerSlots[i % _inventoryPlayerSlots.Count];
        //    GameObject itemInstance = Instantiate(selectedPrefab, _inventoryParentSlot);
        //    if (itemInstance.transform.childCount == 1)
        //    {
        //        Transform secondChild = itemInstance.transform.GetChild(0);
        //        InvRessource ressourceComponent = secondChild.gameObject.AddComponent<InvRessource>();
        //        //ressourceComponent.Ressource = _scriptableRessourceList.RessourceList[i];
        //        ressourceComponent.Quantity = 1;
        //        _objects.Add(secondChild.gameObject);
        //    }
        //}
    }

    public void AddToInventory(GameObject newItem)
    {
        _objects.Add(newItem);
    }

    public void CreateNewInventorySlot(RessourceData ressources) // mettre le stack des ressources 
    {
        if(_slotGameobjectList.Count > 0)
        {
            foreach(InvRessource item in _slotGameobjectList)
            {
                if (item.Ressource.Id != ressources.Id)
                {
                    GameObject newSlot = Instantiate(_globalPrefab, _inventoryParentSlot.transform);
                    InvRessource invRessource = newSlot.transform.GetChild(0).AddComponent<InvRessource>();
                    _slotGameobjectList.Add(invRessource);
                    invRessource.Ressource = ressources.Ressources;
                    // newSlot.GetComponentInChildren<Image>().sprite = ressources.Ressources.Sprite;
                    invRessource.Quantity = 1;
                }
                else
                {
                    item.Quantity++;
                }
                break;
            }
        }
        else
        {
            GameObject newSlot = Instantiate(_globalPrefab, _inventoryParentSlot.transform);
            InvRessource invRessource = newSlot.transform.GetChild(0).AddComponent<InvRessource>();
            _slotGameobjectList.Add(invRessource);
            invRessource.Ressource = ressources.Ressources;
            // newSlot.GetComponentInChildren<Image>().sprite = ressources.Ressources.Sprite;
            invRessource.Quantity = 1;
        }
    } 
}

public class InvRessource : MonoBehaviour
{
    public Scriptable_Ressources Ressource;
    public int Quantity;
}


