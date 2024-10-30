using System.Collections.Generic;
using UnityEngine;

public class InventoryPlayerManager : MonoBehaviour
{

    [SerializeField] private GameObject _inventoryPlayerCase;

    [SerializeField] private List<Scriptable_Ressources> _scripts = new List<Scriptable_Ressources>();

    [SerializeField] private List<GameObject> _objects = new List<GameObject>();
    void Start()
    {
        InitializePlayerInventory();
    }

    private void InitializePlayerInventory()
    {
        for (int i = 0; i < _scripts.Count; i++)
        {
            GameObject itemInstance = Instantiate(_inventoryPlayerCase);
            itemInstance.AddComponent<Ressource>();
            _objects.Add(itemInstance);
        }
        RecupAllInfo();
    }

    public void RecupAllInfo()
    {
        foreach (GameObject item in _objects)
        {
            foreach (Scriptable_Ressources scripts in _scripts)
            {
                item.GetComponent<Ressource>().SetAllParameters(scripts);            
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

    public void SetAllParameters(Scriptable_Ressources ressources)
    {
        Id = ressources.Id;
        Name = ressources.Name;
        Sprite = ressources.Sprite;
        IsPure = ressources.IsPure;
        GrounderOutput = ressources.GrinderOutput;
        DisassenblerOutputs = ressources.DisassemblerOutputs;
    }
}