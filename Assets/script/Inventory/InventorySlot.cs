using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public static InventorySlot Instance;

    public Image Image;

    private void Awake()
    {
        Instance = this;
    }
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DragUiElementInventory draggableItem = dropped.GetComponent<DragUiElementInventory>();
        draggableItem.parentAfterDrag = transform;
    }
}
