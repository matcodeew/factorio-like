using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateFloorShader : MonoBehaviour
{
    [SerializeField, Range(0.0f, 1.0f)] private float _purifiedValue;
    [SerializeField] private Material _floorMat;
    [SerializeField] private bool _purifiedView = false;
    public bool IsPurified = false;


    void Start()
    {
        gameObject.GetComponent<Renderer>().material = new Material(_floorMat);
        _floorMat = gameObject.GetComponent<Renderer>().material;

        _floorMat.SetFloat("_PurifiedValue", _purifiedValue);
        _floorMat.SetInt("_IsPurified", IsPurified ? 1 : 0);
        _floorMat.SetInt("_IsInPurifiedView", _purifiedView ? 1 : 0);
    }

    public void AddPurify(float value)
    {
        _purifiedValue = Mathf.Clamp(_purifiedValue + value, 0.0f, 1.0f);
        _floorMat.SetFloat("_PurifiedValue", _purifiedValue);

        if (_purifiedValue == 1.0f)
        {
            IsPurified = true;
            _floorMat.SetInt("_IsPurified", _purifiedValue == 1.0f ? 1 : 0);
        }

        // CALL UPDATE AVERAGE (WIN CON)
    }


    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            _purifiedView = !_purifiedView;
            _floorMat.SetInt("_IsInPurifiedView", _purifiedView ? 1 : 0);
        }
    }
}
