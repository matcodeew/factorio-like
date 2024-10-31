using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DragUiElementInventory : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform ParentAfterDrag;

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

        foreach (var result in results)
        {
            if (result.gameObject != gameObject && result.gameObject.CompareTag("Item"))
            {
                targetTransform = result.gameObject.transform.parent; 
                break;
            }
        }

        if (targetTransform != null && targetTransform.childCount > 0)
        {
            Transform targetChild = targetTransform.GetChild(1); 
            targetChild.SetParent(ParentAfterDrag); 
            targetChild.position = ParentAfterDrag.position; 
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
