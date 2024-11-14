using UnityEngine;

public class ShipCameraController : MonoBehaviour
{
    public Transform ship;           
    public Transform target;        
    public float speed = 10f;        
    public float cameraDistance = 10f; 
    public float rotationSpeed = 100f; 
    public float controlSensitivity = 2f; 

    private Vector3 currentRotation; 

    private void Start()
    {
        transform.position = ship.position - ship.forward * cameraDistance + Vector3.up;
        currentRotation = transform.eulerAngles;
    }

    private void Update()
    {
        Vector3 direction = (target.position - ship.position).normalized;
        ship.position += direction * speed * Time.deltaTime;

        float horizontal = Input.GetAxis("Horizontal") * controlSensitivity;
        float vertical = Input.GetAxis("Vertical") * controlSensitivity;

        currentRotation.x = Mathf.Clamp(currentRotation.x - vertical, -30f, 60f); 
        currentRotation.y += horizontal; 

        Quaternion rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, 0);
        transform.position = ship.position - (rotation * Vector3.forward * cameraDistance);
        transform.LookAt(ship.position);
    }
}
