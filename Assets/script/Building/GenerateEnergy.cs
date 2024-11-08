using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnergy : MonoBehaviour
{
    public List<GameObject> TransformBuildingConnected = new List<GameObject>();
    public int MaxConnection;
    [SerializeField] private GameObject _connectPanel;
    private bool _isActive;

    public bool CanConnectBuilding() {  return TransformBuildingConnected.Count < MaxConnection; }
    public void ActivePanel()
    {
        _isActive = !_isActive;
        _connectPanel.SetActive(_isActive);
    }
    private void OnMouseDown()
    {
        ActivePanel();
    }
}
