using System.Collections.Generic;
using UnityEngine;

public class InventoryPlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryPlayerCase;
    [SerializeField] private List<Scriptable_Ressources> _scripts = new List<Scriptable_Ressources>();
    [SerializeField] private List<GameObject> _objects = new List<GameObject>();

    [SerializeField] private List<int> _quantities = new List<int>();

    void Start()
    {
        InitializePlayerInventory();
    }

    private void InitializePlayerInventory()
    {
        for (int i = 0; i < _scripts.Count; i++)
        {
            GameObject itemInstance = Instantiate(_inventoryPlayerCase);
            Ressource ressourceComponent = itemInstance.AddComponent<Ressource>();

            int itemQuantity = (i < _quantities.Count) ? _quantities[i] : 1; 
            ressourceComponent.SetAllParameters(_scripts[i], itemQuantity);

            _objects.Add(itemInstance);
        }
        RecupAllInfo();
    }

    public void RecupAllInfo()
    {
        for (int i = 0; i < _objects.Count; i++)
        {
            Ressource ressource = _objects[i].GetComponent<Ressource>();
            if (ressource != null)
            {
                ressource.SetAllParameters(_scripts[i], ressource.Quantity);
            }
        }
    }
}

public class Ressource : MonoBehaviour
{
    public int Id;
    public string Name;
    public Sprite Sprite;
    public bool IsPure;
    public ScriptableObject GrounderOutput;
    public List<ScriptableObject> DisassenblerOutputs;
    public int Quantity;

    public void SetAllParameters(Scriptable_Ressources ressources, int quantity)
    {
        Id = ressources.Id;
        Name = ressources.Name;
        Sprite = ressources.Sprite;
        IsPure = ressources.IsPure;
        GrounderOutput = ressources.GrinderOutput;
        DisassenblerOutputs = ressources.DisassemblerOutputs;
        Quantity = quantity; 
    }
}
