using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float Speed;
    public float RotationSpeed;

    private Vector3 _targetPosition;
    private Vector3 _clikedTarget;

    private bool _isMoving;

    const int RIGHT_MOUSE_BUTTON = 1;

    [SerializeField] private MapManager _mapManager;

    public GameObject _actualBuilding;
    public float PlayerRange;

    void Start()
    {
        _targetPosition = transform.position;
        _isMoving = false;
    }

    void Update()
    {
        if (Menu.GameIsPaused)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(RIGHT_MOUSE_BUTTON))
        {
            SetTargetPosition();
            if (_actualBuilding != null)
                _actualBuilding.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(RIGHT_MOUSE_BUTTON) && hit.collider.CompareTag("Build"))
            {
                _actualBuilding = hit.collider.gameObject;
                SetTargetPosition();

                _actualBuilding.transform.GetChild(0).gameObject.SetActive(true);
                InventoryBuildManager.Instance.InventoryBuildPanel.SetActive(false);
                InventoryBuildManager.Instance.InventoryButton.SetActive(true);

            }

            if (Input.GetMouseButtonDown(RIGHT_MOUSE_BUTTON) && hit.collider.CompareTag("Transformer"))
            {
                _actualBuilding = hit.collider.gameObject;
                SetTargetPosition();

                _actualBuilding.transform.GetChild(0).gameObject.SetActive(true);
                InventoryBuildManager.Instance.InventoryBuildPanel.SetActive(false);
                InventoryBuildManager.Instance.InventoryButton.SetActive(true);

            }
        }
        if (_isMoving)
            MovingPlayer();
    }

    private bool IsInRangeOfTarget()
    {
        float distance = (_clikedTarget - transform.position).magnitude;
        return distance <= PlayerRange;
    }

    public void SetTargetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green, 2f);
            _targetPosition = hit.point;
            _clikedTarget = new Vector3(_targetPosition.x, 0, _targetPosition.z);

            MapManager.Instance.PickingRessource = false;
            MapManager.Instance.CheckRessourceOnClick(_clikedTarget, hit);

            _isMoving = true;
            Debug.Log($"Raycast hit at: {_targetPosition}");
        }
        else
        {
            Debug.LogWarning("Raycast did not hit anything.");
        }
    }




    public void MovingPlayer()
    {
        transform.LookAt(_targetPosition);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x - 80, transform.rotation.eulerAngles.y - 180, transform.rotation.eulerAngles.z - 157);

        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Speed * Time.deltaTime);

        if (transform.position == _targetPosition)
            _isMoving = false;

        Debug.DrawLine(transform.position, _targetPosition, Color.red);
    }

}
