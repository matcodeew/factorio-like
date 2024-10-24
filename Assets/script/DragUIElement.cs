using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragUIItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // Reference to the prefab to instantiate on drag end
    [SerializeField] private GameObject PrefabToInstantiate;

    // Reference to the RectTransform of the UI element to be dragged
    [SerializeField] private RectTransform UIDragElement;

    // Reference to the RectTransform of the canvas containing the UI element
    [SerializeField] private RectTransform Canvas;

    // Variables for tracking original positions during the drag
    private Vector2 mOriginalLocalPointerPosition;
    private Vector3 mOriginalPanelLocalPosition;
    private Vector2 mOriginalPosition;
    private void Start()
    {
        mOriginalPosition = UIDragElement.localPosition;
    }
    public void OnBeginDrag(PointerEventData data)
    {
        // Record the original local pointer position when the drag starts
        mOriginalPanelLocalPosition = UIDragElement.localPosition;

        // Convert screen point to local point in canvas space
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            Canvas,
            data.position,
            data.pressEventCamera,
            out mOriginalLocalPointerPosition);
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

            // Update the UI element's position based on the drag movement
            UIDragElement.localPosition = mOriginalPanelLocalPosition + offsetToOriginal;
        }
    }
    public IEnumerator Coroutine_MoveUIElement(RectTransform r, Vector2 targetPosition, float duration = 0.1f)
    {
        float elapsedTime = 0;
        Vector2 startingPos = r.localPosition;

        // Lerp (linear interpolation) to smoothly move the UI element
        while (elapsedTime < duration)
        {
            r.localPosition = Vector2.Lerp(startingPos, targetPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;

            // Wait until the end of the frame before updating again
            yield return new WaitForEndOfFrame();
        }

        // Ensure the UI element reaches the exact target position
        r.localPosition = targetPosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        StartCoroutine(Coroutine_MoveUIElement(UIDragElement, mOriginalPosition, 0.5f));
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        if (Physics.Raycast(ray, out hit, 1000.0f))
        {
            Vector3 worldPoint = hit.point;
            CreateObject(worldPoint);
        }
    }
    public void CreateObject(Vector3 position)
    {
        // Check if the prefab is defined
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
        // Placeholder logic: Always return true. Implement your own logic here.
        return true;
    }
}