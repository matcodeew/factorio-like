using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float ZoomSpeed;
    public float MinZoom;
    public float MaxZoom;
    public float MinX, MaxX, MinY, MaxY;

    private float _maxSpeed = 50f;
    private float _accelerationRate = 20f;
    private float _speed = 30f;
    private float _border = 50.0f;
    private float _activationBorder = 10.0f; // Nouvelle marge pour l'activation du mouvement
    private float _accelerationX = 5f;
    private float _accelerationY = 5f;

    private Camera _cam;

    private void Start()
    {
        _cam = GetComponent<Camera>();
    }

    void Update()
    {
        MoveCamera();
        HandleZoom();

        // Toggle Purified View
        if (Input.GetKeyDown(KeyCode.V))
        {
            MapManager.Instance.FireTogglePurifyViewEvent();
        }
    }

    private void HandleZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (_cam.orthographic)
        {
            _cam.orthographicSize -= scrollInput * ZoomSpeed;
            _cam.orthographicSize = Mathf.Clamp(_cam.orthographicSize, MinZoom, MaxZoom);
        }
        else
        {
            _cam.fieldOfView -= scrollInput * ZoomSpeed;
            _cam.fieldOfView = Mathf.Clamp(_cam.fieldOfView, MinZoom, MaxZoom);
        }
    }

    private void MoveCamera()
    {
        if (_cam != null)
        {
            Vector3 newPosition = _cam.transform.position;

            // Vérifier si la souris est en dehors de l'écran à droite
            if (Input.mousePosition.x >= Screen.width + _border)
            {
                _accelerationX += _accelerationRate * Time.deltaTime;
                _accelerationX = Mathf.Min(_accelerationX, _maxSpeed);

                newPosition.x += _accelerationX * Time.deltaTime;
            }
            // Vérifier si la souris est en dehors de l'écran à gauche
            else if (Input.mousePosition.x <= -_border)
            {
                _accelerationX += _accelerationRate * Time.deltaTime;
                _accelerationX = Mathf.Min(_accelerationX, _maxSpeed);

                newPosition.x -= _accelerationX * Time.deltaTime;
            }
            else
            {
                _accelerationX = 0f;
            }

            // Vérifier si la souris est en dehors de l'écran en haut
            if (Input.mousePosition.y >= Screen.height + _border)
            {
                _accelerationY += _accelerationRate * Time.deltaTime;
                _accelerationY = Mathf.Min(_accelerationY, _maxSpeed);

                newPosition.z += _accelerationY * Time.deltaTime;
            }
            // Vérifier si la souris est en dehors de l'écran en bas
            else if (Input.mousePosition.y <= -_border)
            {
                _accelerationY += _accelerationRate * Time.deltaTime;
                _accelerationY = Mathf.Min(_accelerationY, _maxSpeed);

                newPosition.z -= _accelerationY * Time.deltaTime;
            }
            else
            {
                _accelerationY = 0f;
            }

            // Clamper les limites de la caméra
            newPosition.x = Mathf.Clamp(newPosition.x, MinX, MaxX);
            newPosition.y = Mathf.Clamp(newPosition.y, MinY, MaxY);

            _cam.transform.position = newPosition;
        }
    }
}
