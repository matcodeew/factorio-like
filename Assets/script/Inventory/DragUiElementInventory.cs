using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragUiElementInventory : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform ParentAfterDrag;

    [SerializeField] private TextMeshProUGUI _quantityText;

    InventorySlot InventorySlot;

    private void Update()
    {
        InvRessource invRessource = GetComponent<InvRessource>();
        if (_quantityText != null)
        {
            _quantityText.text = invRessource.Quantity.ToString();
        }
    }

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
            if (result.gameObject.CompareTag("Panel"))
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
            else if (result.gameObject.CompareTag("Item") && result.gameObject != gameObject)
            {
                targetTransform = result.gameObject.transform.parent;
                targetItem = result.gameObject.GetComponent<DragUiElementInventory>();
                break;
            }
        }

        if (targetItem != null && targetItem.GetComponent<InvRessource>() != null && targetItem.GetComponent<InvRessource>().Ressource.Id == GetComponent<InvRessource>().Ressource.Id)
        {
            InventoryPlayerManager inventorySlot = targetItem.GetComponentInParent<InventoryPlayerManager>();

            if (inventorySlot != null)
            {
                targetItem.AddToQuantity(targetItem.GetComponent<InvRessource>());
            }
            ParentAfterDrag.tag = "Empty";
            Destroy(ParentAfterDrag.gameObject);
        }
        else
        {
            transform.SetParent(ParentAfterDrag);
            transform.position = ParentAfterDrag.position;
        }

        if (targetTransform != null && targetTransform.CompareTag("Empty") && targetTransform.childCount == 0)
        {
            Image itemImage = gameObject.GetComponent<Image>();
            bool isImageEnabled = itemImage != null && itemImage.enabled;
            Destroy(ParentAfterDrag.gameObject);
            ParentAfterDrag.tag = "Empty";
            transform.SetParent(targetTransform);  
            transform.position = targetTransform.position; 

            if (itemImage != null)
            {
                itemImage.enabled = isImageEnabled;  
            }

            DragUiElementInventory dragScript = gameObject.GetComponent<DragUiElementInventory>();
            if (dragScript != null)
            {
                dragScript.enabled = true;  
            }
            targetTransform.tag = "InventorySlot";
        }
        else
        {
            transform.SetParent(ParentAfterDrag);
            transform.position = ParentAfterDrag.position;
        }

        if (targetTransform.CompareTag("Panel"))
        {
            targetTransform = targetTransform.gameObject.transform;
            GameObject newItem = Instantiate(InventoryPlayerManager.Instance.EmptyPrefab, targetTransform);
            transform.SetParent(newItem.transform);
            transform.position = newItem.transform.position;
            Image itemImage = gameObject.GetComponent<Image>();
            bool isImageEnabled = itemImage != null && itemImage.enabled;
            if (itemImage != null)
            {
                itemImage.enabled = isImageEnabled;
            }
            DragUiElementInventory dragScript = gameObject.GetComponent<DragUiElementInventory>();
            if (dragScript != null)
            {
                dragScript.enabled = true;
            }
            ParentAfterDrag.tag = "Empty";
        }

    }

    public void AddToQuantity(InvRessource otherRessource)
    {
        InvRessource currentRessource = GetComponent<InvRessource>();

        if (currentRessource != null && otherRessource != null && currentRessource.Ressource.Id == otherRessource.Ressource.Id)
        {
            currentRessource.Quantity += otherRessource.Quantity;
        }
    }
}
