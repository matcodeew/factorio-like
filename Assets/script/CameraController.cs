using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float _speed = 30f;
    private float _border = 50.0f;

    public float zoomSpeed;

    public float minZoom;
    public float maxZoom;
    public float minX, maxX, minY, maxY;

    private float _maxSpeed = 50f;     
    private float _accelerationRate = 20f;
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
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (_cam.orthographic)
        {
            _cam.orthographicSize -= scrollInput * zoomSpeed;
            _cam.orthographicSize = Mathf.Clamp(_cam.orthographicSize, minZoom, maxZoom);
        }
        else
        {
            _cam.fieldOfView -= scrollInput * zoomSpeed;
            _cam.fieldOfView = Mathf.Clamp(_cam.fieldOfView, minZoom, maxZoom);
        }
    }

    private void MoveCamera()
    {
        if (_cam != null)
        {
            Vector3 newPosition = _cam.transform.position;

            // Droite
            if (Input.mousePosition.x >= Screen.width - _border)
            {
                _accelerationX += _accelerationRate * Time.deltaTime;
                _accelerationX = Mathf.Min(_accelerationX, _maxSpeed);

                newPosition.x += _accelerationX * Time.deltaTime;
            }
            // Gauche
            else if (Input.mousePosition.x <= 0 + _border)
            {
                _accelerationX += _accelerationRate * Time.deltaTime;
                _accelerationX = Mathf.Min(_accelerationX, _maxSpeed);

                newPosition.x -= _accelerationX * Time.deltaTime;
            }
            else
            {
                _accelerationX = 0f;
            }

            // Haut
            if (Input.mousePosition.y >= Screen.height - _border)
            {
                _accelerationY += _accelerationRate * Time.deltaTime;
                _accelerationY = Mathf.Min(_accelerationY, _maxSpeed);

                newPosition.z += _accelerationY * Time.deltaTime;
            }
            // Bas
            else if (Input.mousePosition.y <= 0 + _border)
            {
                _accelerationY += _accelerationRate * Time.deltaTime;
                _accelerationY = Mathf.Min(_accelerationY, _maxSpeed);

                newPosition.z -= _accelerationY * Time.deltaTime;
            }
            else
            {
                _accelerationY = 0f;
            }
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

            _cam.transform.position = newPosition;
        }
    }

}
