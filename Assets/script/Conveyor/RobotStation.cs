using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotStation : MonoBehaviour, ISubscrireEvent
{
    [HideInInspector] public GameObject _robotConveyor;

    public void SubscrireEvent()
    {
    }

    private void Start()
    {
        _robotConveyor = transform.GetChild(1).gameObject;
        _robotConveyor.GetComponent<RobotBehaviour>().Init();
    }
}
