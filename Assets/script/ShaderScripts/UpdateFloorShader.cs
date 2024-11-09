using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateFloorShader : MonoBehaviour
{
    [SerializeField, Range(0.0f, 1.0f)] private float _purifiedValue;
    [SerializeField] private Material _floorMat;
    [SerializeField] private bool _purifiedView = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
            _purifiedView = !_purifiedView;


        _floorMat.SetFloat("_PurifiedValue", _purifiedValue);
        _floorMat.SetInt("_IsPurified", _purifiedValue == 1.0f ? 1 : 0);
        _floorMat.SetInt("_IsInPurifiedView", _purifiedView ? 1 : 0);

    }
}
