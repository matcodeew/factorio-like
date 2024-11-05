using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragUiElementInventory : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform ParentAfterDrag;
    [SerializeField] private Scriptable_Ressources Scriptable_Ressources;

    public void OnBeginDrag(PointerEventData eventData)
    {
        ParentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        Transform targetTransform = null;
        DragUiElementInventory targetItem = null;

        foreach (var result in results)
        {
            if (result.gameObject.CompareTag("Empty"))
            {
                targetTransform = result.gameObject.transform;
                break;
            }
            else if (result.gameObject != gameObject && result.gameObject.CompareTag("Item"))
            {
                ParentAfterDrag.tag = "InventorySlot";
                targetTransform = result.gameObject.transform.parent;
                targetItem = result.gameObject.GetComponent<DragUiElementInventory>();
                break;
            }
        }

        if (targetItem != null && targetItem.Scriptable_Ressources != null && targetItem.Scriptable_Ressources.Id == this.Scriptable_Ressources.Id)
        {
            InventoryPlayerManager.Instance.AddToQuantity(Scriptable_Ressources);
            ParentAfterDrag.tag = "Empty";
            Destroy(gameObject);
        }
        else
        {
                        transform.SetParent(ParentAfterDrag);
            transform.position = ParentAfterDrag.position;
        }

        if (targetTransform != null && targetTransform.CompareTag("Empty") && targetTransform.childCount == 1)
        {
            targetTransform.tag = "InventorySlot";
            ParentAfterDrag.tag = "Empty";
            transform.SetParent(targetTransform);
            transform.position = targetTransform.position;
        }
        else
        {
            transform.SetParent(ParentAfterDrag);
            transform.position = ParentAfterDrag.position;
        }
    }

}
