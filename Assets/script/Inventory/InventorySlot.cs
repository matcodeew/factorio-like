using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private TextMeshProUGUI _quantityText;

    private void Update()
    {
        InvRessource invRessource = GetComponent<InvRessource>();
        _quantityText.text = invRessource.Quantity.ToString();
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DragUiElementInventory draggableItem = dropped.GetComponent<DragUiElementInventory>();
        draggableItem.ParentAfterDrag = transform;
    }

    public void AddToQuantity(Scriptable_Ressources ressource)
    {
        InvRessource ressources = GetComponent<InvRessource>();
        ressources.Quantity++;
    }

}
