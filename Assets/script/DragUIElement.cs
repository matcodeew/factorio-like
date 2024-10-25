using UnityEngine;
using UnityEngine.EventSystems;

public class DragUIElement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private GameObject _prefabToInstantiate; 
    [SerializeField] private RectTransform _cancelBuild;
    [SerializeField] private RectTransform _uiDragElement;
    [SerializeField] private RectTransform _uiInventory;
    [SerializeField] private RectTransform _canvas;

    private Vector2 _originalPointerPosition;
    private Vector3 _originalPanelPosition;
    private Vector2 _originalElementPosition;

    private GameObject _previewInstance; 
    private CanvasGroup _uiCanvasGroup; 

    private void Start()
    {
        _originalElementPosition = _uiDragElement.localPosition;
        if (_uiCanvasGroup == null)
        {
            _uiCanvasGroup = _uiDragElement.gameObject.AddComponent<CanvasGroup>();
            _uiCanvasGroup = _uiInventory.gameObject.AddComponent<CanvasGroup>();
        }

    }

    public void OnBeginDrag(PointerEventData data) // take 2d object in inventory
    {
        _originalPanelPosition = _uiDragElement.localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas,
            data.position,
            data.pressEventCamera,
            out _originalPointerPosition);

        if (_prefabToInstantiate != null)
        {
            _previewInstance = Instantiate(_prefabToInstantiate);
        }
        FadeUIElement(0f);
    }

    public void OnDrag(PointerEventData data) // mouse press
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas,
            data.position,
            data.pressEventCamera,
            out Vector2 localPointerPosition))
        {
            Vector3 offset = localPointerPosition - _originalPointerPosition;
            _uiDragElement.localPosition = _originalPanelPosition + offset;

            if (_previewInstance != null)
            {
                UpdatePreviewPosition(data);
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _uiDragElement.localPosition = _originalElementPosition;

        if (_previewInstance != null)
        {
            TryPlaceObject(eventData);
            Destroy(_previewInstance);
        }
        FadeUIElement(1f);
    }

    private void UpdatePreviewPosition(PointerEventData data)
    {
        Ray ray = Camera.main.ScreenPointToRay(data.position);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000.0f))
        {
            Vector3 snappedPosition = SnapToGrid(hit.point);
            _previewInstance.transform.position = snappedPosition;
        }
    }

    private Vector3 SnapToGrid(Vector3 position) // place object into grid
    {
        int x = Mathf.RoundToInt(position.x);
        int z = Mathf.RoundToInt(position.z);
        return new Vector3(x, _previewInstance.transform.position.y, z);
    }

    private void TryPlaceObject(PointerEventData eventData) // place object in world
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000.0f))
        {
            Vector3 snappedPosition = SnapToGrid(hit.point);
            CreateObject(snappedPosition);
        }
    }

    private void CreateObject(Vector3 position) // placed object
    {
        if (PositionWithinCell(position))
        {
            Instantiate(_prefabToInstantiate, position, Quaternion.identity);
        }
    }

    private bool PositionWithinCell(Vector3 pos)
    {
        return true;
    }

    private void FadeUIElement(float targetAlpha)
    {
        if (_uiCanvasGroup != null)
        {
            _uiCanvasGroup.alpha = targetAlpha; // Set the alpha to the target value
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject)
            Destroy(collision.gameObject);

    }

    public void OnDelete(PointerEventData data) // take 2d object in inventory
    {
        _originalPanelPosition = _cancelBuild.localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas,
            data.position,
            data.pressEventCamera,
            out _originalPointerPosition);

        Debug.Log("MEOW");

    }
}
