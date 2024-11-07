using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragUiElementInventory : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform ParentAfterDrag;

    [SerializeField] Scriptable_Ressources Scriptable_Ressources;
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
            InventoryPlayerManager inventorySlot = targetItem.GetComponentInParent<InventoryPlayerManager>();

            if (inventorySlot != null)
            {
                targetItem.AddToQuantity();
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
    }

    public void AddToQuantity()
    {
        InvRessource currentRessource = GetComponent<InvRessource>();
        InvRessource otherRessource = GetComponent<InvRessource>();

        if (currentRessource != null && otherRessource != null && currentRessource.Ressource.Id == otherRessource.Ressource.Id)
        {
            currentRessource.Quantity += otherRessource.Quantity;
        }
    }
    private void SetScripts(Scriptable_Ressources scriptable_Ressources, InventorySlot slot)
    {
        scriptable_Ressources = Scriptable_Ressources;
        slot = InventorySlot;
    }
}
