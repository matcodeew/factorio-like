using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurifierBotController : MonoBehaviour
{
    // GO TO THE RECHARGE STATION WHEN ENERGY IS <= _rechargeThreshold

    private float _botCharge = 100.0f;
    private PurifierRechargeStation _rechargeStation;
    [SerializeField] private float _rechargeThreshold = 5.0f; // %

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
