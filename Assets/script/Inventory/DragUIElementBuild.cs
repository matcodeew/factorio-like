using UnityEngine;
using UnityEngine.EventSystems;

public class DragUIElementBuild : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private GameObject _prefabToInstantiate;
    private GameObject _uiElements;
    private GameObject _inventoryCase;
    private GameObject _previewPrefabToInstantiate;

    private Vector2 _originalPointerPosition;
    private Vector3 _originalPanelPosition;

    private bool _isOverTrash = false;

    public void Initialize(GameObject prefabToInstantiate, GameObject uiElement, GameObject inventoryCase)
    {
        _prefabToInstantiate = prefabToInstantiate;
        _uiElements = uiElement;
        _inventoryCase = inventoryCase;
    }

    public void OnBeginDrag(PointerEventData data)
    {
        _originalPanelPosition = _uiElements.transform.localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _uiElements.transform as RectTransform, data.position, data.pressEventCamera, out _originalPointerPosition);

        if (_prefabToInstantiate != null)
        {
           _previewPrefabToInstantiate = Instantiate(_prefabToInstantiate);
        }
        InventoryBuildManager.Instance.FadeUIElement(0f);
    }

    public void OnDrag(PointerEventData data)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _uiElements.transform as RectTransform, data.position, data.pressEventCamera, out Vector2 localPointerPosition))
        {
            Vector3 offset = localPointerPosition - _originalPointerPosition;
            _uiElements.transform.localPosition = _originalPanelPosition + offset;
            InventoryBuildManager.Instance.TrashAreaGameObject.SetActive(true);

            if (_previewPrefabToInstantiate != null)
            {
                UpdatePreviewPosition(data);
            }

            _isOverTrash = IsPointerOverTrash(data);
            if (_isOverTrash)
            {
                OnEndDrag(data);
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _uiElements.transform.localPosition = _originalPanelPosition;
        InventoryBuildManager.Instance.TrashAreaGameObject.SetActive(false);
        if (_previewPrefabToInstantiate != null)
        {
            TryPlaceObject(eventData);
            Destroy(_previewPrefabToInstantiate);
        }
        InventoryBuildManager.Instance.FadeUIElement(1f);
    }

    private void UpdatePreviewPosition(PointerEventData data)
    {
        Ray ray = Camera.main.ScreenPointToRay(data.position);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000.0f))
        {
            Vector3 snappedPosition = SnapToGrid(hit.point);
            _previewPrefabToInstantiate.transform.position = snappedPosition;
        }
    }

    private Vector3 SnapToGrid(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x);
        int z = Mathf.RoundToInt(position.z);
        return new Vector3(x, _previewPrefabToInstantiate.transform.position.y, z);
    }

    private void TryPlaceObject(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000.0f))
        {
            Vector3 snappedPosition = SnapToGrid(hit.point);
            InventoryBuildManager.Instance.CreateObjectOnMap(_prefabToInstantiate, snappedPosition);
        }
    }

    private bool IsPointerOverTrash(PointerEventData data)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(
            InventoryBuildManager.Instance.TrashArea, data.position, data.pressEventCamera);
    }
}
