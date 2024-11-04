using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragUiElementInventory : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform ParentAfterDrag;

    Scriptable_Ressources Scriptable_Ressources;

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

        if (targetItem != null && targetItem.Scriptable_Ressources.Id == this.Scriptable_Ressources.Id)
        {
            ParentAfterDrag.tag = "Empty";
            Debug.Log("Add to stack");
            Destroy(gameObject);
        }
        else if (targetTransform != null && targetTransform.CompareTag("Empty") && targetTransform.childCount == 1)
        {
            transform.SetParent(targetTransform);
            transform.position = targetTransform.position;
            ParentAfterDrag.tag = "Empty";
        }
        else
        {
            transform.SetParent(ParentAfterDrag);
            transform.position = ParentAfterDrag.position;
        }
    }
}
