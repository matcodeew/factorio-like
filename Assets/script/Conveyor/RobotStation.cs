using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotStation : MonoBehaviour
{
    [HideInInspector] public GameObject _robotConveyor;
    [HideInInspector] public bool BuildPosed = false;

    public void Init()
    {
        _robotConveyor = transform.GetChild(1).gameObject;
        _robotConveyor.GetComponent<RobotBehaviour>().Init();
        BuildPosed = true;
    }
}
