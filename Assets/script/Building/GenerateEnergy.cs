using System.Collections.Generic;
using UnityEngine;

public enum TypeOfGenerator
{
    eolienne,
    solarPannel,
}
public class GenerateEnergy : MonoBehaviour, ISubscrireEvent
{
    private BuildingManager instance;

    [Header("Energy utils")]
    [SerializeField] private TypeOfGenerator GenType;
    [SerializeField, Min(1.0f)] public int MaxConnection = 2;
    public int Energy;
    public List<BuildingReceivedEnergy> TransformBuildingConnected = new List<BuildingReceivedEnergy>();
    public List<GameObject> _linkConnect = new List<GameObject>();
    private bool _isActive;

    [Header("eolienne Anim")]
    [SerializeField] private GameObject _eolienne;
    [SerializeField] private float _speedRotate = 100;

    public bool CanConnectBuilding() { return TransformBuildingConnected.Count < MaxConnection; }
    public void SubscrireEvent()
    {
        instance = BuildingManager.Instance;
        instance.UpdateSharedEnergy += UpdateEnergy;
    }
    private void UpdateEnergy()
    {
        if (GenType == TypeOfGenerator.solarPannel)
        {
            Energy = instance.GenerateSolarEnergy();
        }
        else
        {
            Energy = instance.GenerateWindEnergy();
        }
        instance.UpdateBuildingEnergy?.Invoke();
    }
    private void Update()
    {
        if (GenType == TypeOfGenerator.eolienne)
        {
            if (_linkConnect.Count <= 0) { return; }
            else
            {
                _eolienne.transform.Rotate(0, 0, -(_speedRotate * Time.deltaTime));
            }
        }
    }
    public void AddGenToList()
    {
        foreach (BuildingReceivedEnergy energy in TransformBuildingConnected)
        {
            if (!energy.ConnectGenerator.Contains(this))
            {
                energy.ConnectGenerator.Add(this);
                instance.UpdateSharedEnergy?.Invoke();
            }
        }
    }
    public void OnMouseDown()
    {
        //ActivePanel();
        if (instance.CanDestroyBuilding)
        {
            InventoryBuildManager.Instance.AddOnDestroy(this.gameObject);
            Destroy(gameObject);
            DestroyGenerator();
        }
    }
    public void ActivePanel()
    {
        _isActive = !_isActive;
        transform.GetChild(0).gameObject.SetActive(_isActive);
    }

    private void DestroyGenerator()
    {
        foreach (var transformer in TransformBuildingConnected)
        {
            if (transformer.ConnectGenerator.Contains(this))
            {
                transformer.ConnectGenerator.Remove(this);
            }

            foreach (GameObject link in _linkConnect)
            {
                if (_linkConnect.Contains(link))
                {
                    _linkConnect.Remove(link);
                    Destroy(link);
                }
                break;
            }
        }
        instance.UpdateBuildingEnergy?.Invoke();
        instance.UpdateSharedEnergy -= UpdateEnergy;
    }
}
