using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotStation : MonoBehaviour
{
    [HideInInspector] public GameObject _robotConveyor;

    private void Awake()
    {
        _robotConveyor = transform.GetChild(1).gameObject;
    }
}
