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

    public void OnBeginDrag(PointerEventData data)
    {
        mOriginalPanelLocalPosition = UIDragElement.localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            Canvas,
            data.position,
            data.pressEventCamera,
            out mOriginalLocalPointerPosition);

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

    public void OnDrag(PointerEventData data)
    {
        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            Canvas,
            data.position,
            data.pressEventCamera,
            out localPointerPosition))
        {
            Vector3 offsetToOriginal = localPointerPosition - mOriginalLocalPointerPosition;

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

    public IEnumerator Coroutine_MoveUIElement(RectTransform r, Vector2 targetPosition, float duration = 0.1f)
    {
        float elapsedTime = 0;
        Vector2 startingPos = r.localPosition;

        while (elapsedTime < duration)
        {
            r.localPosition = Vector2.Lerp(startingPos, targetPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        r.localPosition = targetPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
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

    public void CreateObject(Vector3 position)
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

    private void FadeUIElement(float targetAlpha)
    {
        if (uiElementCanvasGroup != null)
        {
            uiElementCanvasGroup.alpha = targetAlpha; 
        }
    }
}
