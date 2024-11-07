using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DragUiElementInventory draggableItem = dropped.GetComponent<DragUiElementInventory>();
        draggableItem.ParentAfterDrag = transform;
    }
}
