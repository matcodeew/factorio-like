using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;

    public float rotationSpeed;

    private Vector3 _targetPosition;

    private bool _isMoving;

    const int RIGHT_MOUSE_BUTTON = 1;
    void Start()
    {
        _targetPosition = transform.position;
        _isMoving = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(RIGHT_MOUSE_BUTTON))
            SetTargetPosition();

        if (_isMoving)
            MovingPlayer();
    }

    public void SetTargetPosition()
    {
        Plane plane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float point = 0f;

        if (plane.Raycast(ray, out point))
            _targetPosition = ray.GetPoint(point);

        _isMoving = true;
    }

    public void MovingPlayer()
    {
        transform.LookAt(_targetPosition);
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, speed * Time.deltaTime);


        if (transform.position == _targetPosition)
            _isMoving = false;

        Debug.DrawLine(transform.position, _targetPosition, Color.red);
    }
}
