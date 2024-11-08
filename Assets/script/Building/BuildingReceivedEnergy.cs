using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingReceivedEnergy : MonoBehaviour
{
    public List<GenerateEnergy> ConnectGenerator = new();
    public bool IsLinked;
}
