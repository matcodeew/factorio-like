using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragUIItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private GameObject PrefabToInstantiate;
    [SerializeField] private RectTransform UIDragElement;
    [SerializeField] private RectTransform Canvas;

    private GameObject previewInstance; 
    private Vector2 mOriginalLocalPointerPosition;
    private Vector3 mOriginalPanelLocalPosition;
    private Vector2 mOriginalPosition;
    private CanvasGroup uiElementCanvasGroup; 

    private void Start()
    {
        mOriginalPosition = UIDragElement.localPosition;
        uiElementCanvasGroup = UIDragElement.GetComponent<CanvasGroup>();
        if (uiElementCanvasGroup == null)
        {
            uiElementCanvasGroup = UIDragElement.gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnBeginDrag(PointerEventData data) // drag select UI in panel inventory
    {
        mOriginalPanelLocalPosition = UIDragElement.localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            Canvas,
            data.position,
            data.pressEventCamera,
            out mOriginalLocalPointerPosition);

        // Instantiate the preview object
        if (PrefabToInstantiate != null)
        {
            previewInstance = Instantiate(PrefabToInstantiate);
            var renderer = previewInstance.GetComponent<Renderer>();
            if (renderer != null)
            {
                Color color = renderer.material.color;
                color.a = 0.5f; 
                renderer.material.color = color;
            }
            FadeUIElement(0f); 
        }
    }

    public void OnDrag(PointerEventData data) //press drag ui
    {
        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            Canvas,
            data.position,
            data.pressEventCamera,
            out localPointerPosition))
        {
            Vector3 offsetToOriginal = localPointerPosition - mOriginalLocalPointerPosition;

            // Update preview position
            if (previewInstance != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(data.position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000.0f))
                {
                    Vector3 newPosition = hit.point;
                    newPosition.y = previewInstance.transform.position.y; 
                    previewInstance.transform.position = newPosition;
                }
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData) // world Drag
    {
        UIDragElement.localPosition = mOriginalPosition;

        if (previewInstance != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(eventData.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                Vector3 worldPoint = hit.point;
                CreateObject(worldPoint);
            }
            Destroy(previewInstance);
        }
        FadeUIElement(1f); 
    }

    public void CreateObject(Vector3 position) // create object
    {
        if (PrefabToInstantiate == null)
        {
            Debug.Log("No prefab to instantiate");
            return;
        }

        if (PositionWithinCell(position))
        {
            GameObject obj = Instantiate(PrefabToInstantiate, position, Quaternion.identity);
        }
    }

    private bool PositionWithinCell(Vector3 pos)
    {
        return true;
    }

    private void FadeUIElement(float targetAlpha) // fade panel
    {
        if (uiElementCanvasGroup != null)
        {
            uiElementCanvasGroup.alpha = targetAlpha; 
        }
    }
}
