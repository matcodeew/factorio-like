using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryPlayerManager : MonoBehaviour
{
    public static InventoryPlayerManager Instance;
    public RectTransform _inventoryParentSlot;
    public List<InvRessource> SlotGameobjectList = new();
    [SerializeField] private GameObject _globalPrefab;
    [SerializeField] public GameObject EmptyPrefab;

    private void Awake()
    {
        Instance = this;
    }
    public void CreateNewInventorySlot(RessourceData ressources)
    {
        bool _itemFounded = false;

        foreach(var item in SlotGameobjectList)
        {
            if(ressources.Id == item.Ressource.Id)
            {
                _itemFounded = true;
                item.Quantity++;
                break;
            }
        }
        if(!_itemFounded)
        {
            GameObject newSlot = Instantiate(_globalPrefab, _inventoryParentSlot.transform);
            InvRessource invRessource = newSlot.transform.GetChild(0).AddComponent<InvRessource>();
            SlotGameobjectList.Add(invRessource);
            invRessource.Ressource = ressources.Ressources;
            //newSlot.transform.GetChild(0).GetComponent<Image>().sprite = ressources.Ressources.Sprite;
            invRessource.Quantity = 1;
        }
    }

}
public class InvRessource : MonoBehaviour
{
    public Scriptable_Ressources Ressource;
    public int Quantity;
}


