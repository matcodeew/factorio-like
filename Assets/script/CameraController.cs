using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float zoomSpeed;

    public float minZoom;
    public float maxZoom;

    private Camera _cam;

    void Start()
    {
        _cam = GetComponent<Camera>();
    }

    void Update()
    {
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
}
